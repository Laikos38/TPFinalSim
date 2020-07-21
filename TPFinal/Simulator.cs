using GeneradorDeNumerosAleatorios;
using RandomVarGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TPFinal.Models;
using static TPFinal.Models.Enums;

namespace TPFinal
{
    public class Simulator
    {
        public GeneratorType GeneratorType { get; set; }
        public UniformGenerator UniformGeneratorLoad { get; set; }
        public ExponentialGenerator ExponentialGeneratorTravel { get; set; }
        public Generator Congruential { get; set; }
        public Random Random { get; set; }

        public Simulator(GeneratorType generatorType)
        {
            this.GeneratorType = generatorType;
            UniformGeneratorLoad = new UniformGenerator();
            UniformGeneratorLoad.a = 10;
            UniformGeneratorLoad.b = 12;
            ExponentialGeneratorTravel = new ExponentialGenerator();
            ExponentialGeneratorTravel.lambda = ((double)1 / (double)15);
            Congruential = new Generator();
            Random = new Random();
        }

        int truck = 0;
        public StateRow NextStateRow(StateRow previous, int i)
        {
            StateRow current = new StateRow();

            Dictionary<string, double> times = new Dictionary<string, double>();
            var nextEvent = this.GetNextEvent(times, previous);

            switch (nextEvent.Key)
            {
                case "EndLoad":
                    current = CreateStateRowEndLoad(previous, nextEvent.Value);
                    break;
                case var val when new Regex(@"EndTravel_*").IsMatch(val):
                    truck = Convert.ToInt32(nextEvent.Key.Split('_')[1]);
                    current = CreateStateRowEndTravel(previous, nextEvent.Value, truck);
                    break;
            }

            return current;
        }

        private KeyValuePair<string, double> GetNextEvent(Dictionary<string, double> times, StateRow previous)
        {
            if (previous.NextEndLoad != 0)
                times.Add("EndLoad", previous.NextEndLoad);

            for (int i=0; i<previous.Trucks.Count; i++)
            {
                if (previous.Trucks[i].TimeTravel != 0)
                    times.Add("EndTravel_" + previous.Trucks[i].Id, previous.Trucks[i].TimeTravel);
            }

            var orderedTimes = times.OrderBy(obj => obj.Value).ToDictionary(obj => obj.Key, obj => obj.Value);
            KeyValuePair<string, double> nextEvent = orderedTimes.First();

            return nextEvent;
        }

        private StateRow CreateStateRowEndLoad(StateRow previous, double time)
        {
            StateRow newStateRow = new StateRow();

            newStateRow.Event = "Fin carga";
            newStateRow.Watch = time;

            if (previous.Shovel.TruckQueue.Count > 0)
            {
                newStateRow.Shovel = new Shovel();
                newStateRow.Shovel.Status = ShovelStatus.Busy;
                Truck truck = previous.Shovel.TruckQueue.Dequeue();
                newStateRow.Shovel.TruckQueue = new Queue<Truck>(previous.Shovel.TruckQueue);

                // Load
                newStateRow.LoadRnd = GetRandom();
                newStateRow.TimeEndLoad = this.UniformGeneratorLoad.Generate(newStateRow.LoadRnd);
                newStateRow.NextEndLoad = newStateRow.TimeEndLoad + newStateRow.Watch;

                // Travel
                newStateRow.TravelRnd = GetRandom();
                newStateRow.TimeEndTravel = this.ExponentialGeneratorTravel.Generate(newStateRow.TravelRnd);
                newStateRow.NextEndTravel = newStateRow.TimeEndTravel + newStateRow.Watch;

                newStateRow.Trucks = new List<Truck>();
                foreach(Truck t in previous.Trucks)
                {
                    if (t.Status == TruckStatus.InLoad)
                    {
                        t.Status = TruckStatus.InTravel;
                        t.TimeTravel = newStateRow.NextEndTravel;
                    }

                    if (t.Id == truck.Id)
                    {
                        t.Status = TruckStatus.InLoad;
                        t.TimeTravel = 0;
                    }
                    newStateRow.Trucks.Add(t);
                }
            }
            else
            {
                newStateRow.Shovel = new Shovel();
                newStateRow.Shovel.Status = ShovelStatus.Free;
                newStateRow.Shovel.TruckQueue = new Queue<Truck>();

                // Travel
                newStateRow.TravelRnd = GetRandom();
                newStateRow.TimeEndTravel = this.ExponentialGeneratorTravel.Generate(newStateRow.TravelRnd);
                newStateRow.NextEndTravel = newStateRow.TimeEndTravel + newStateRow.Watch;

                newStateRow.Trucks = new List<Truck>();
                foreach (Truck t in previous.Trucks)
                {
                    if (t.Status == TruckStatus.InLoad)
                    {
                        t.Status = TruckStatus.InTravel;
                        t.TimeTravel = newStateRow.NextEndTravel;
                    }
                    newStateRow.Trucks.Add(t);
                }
            }

            newStateRow.Stat1 = previous.Stat1;
            if (previous.Shovel.Status == ShovelStatus.Free)
            {
                newStateRow.Stat2 = newStateRow.Watch - previous.Watch;
                newStateRow.Stat3 = previous.Stat3 + newStateRow.Stat2;
            }
            else
            {
                newStateRow.Stat2 = 0;
                newStateRow.Stat3 = previous.Stat3;
            }

            return newStateRow;
        }

        private StateRow CreateStateRowEndTravel(StateRow previous, double time, int id)
        {
            StateRow newStateRow = new StateRow();

            newStateRow.Event = "Fin viaje (" + id + ")";
            newStateRow.Watch = time;

            if (previous.Shovel.Status == ShovelStatus.Free)
            {
                newStateRow.Shovel = new Shovel();
                newStateRow.Shovel.Status = ShovelStatus.Busy;
                newStateRow.Shovel.TruckQueue = new Queue<Truck>();

                // Load
                newStateRow.LoadRnd = GetRandom();
                newStateRow.TimeEndLoad = this.UniformGeneratorLoad.Generate(newStateRow.LoadRnd);
                newStateRow.NextEndLoad = newStateRow.TimeEndLoad + newStateRow.Watch;

                newStateRow.Trucks = new List<Truck>();
                foreach (Truck t in previous.Trucks)
                {
                    if (t.Id == id)
                    {
                        t.Status = TruckStatus.InLoad;
                        t.TimeTravel = 0;
                    }
                    newStateRow.Trucks.Add(t);
                }
            }
            else
            {
                newStateRow.NextEndLoad = previous.NextEndLoad;

                newStateRow.Shovel = new Shovel();
                newStateRow.Shovel.Status = ShovelStatus.Busy;
                newStateRow.Shovel.TruckQueue = new Queue<Truck>(previous.Shovel.TruckQueue);
                newStateRow.Shovel.TruckQueue.Enqueue(new Truck() { Id = id, Status = TruckStatus.Waiting, TimeTravel = 0 });

                newStateRow.Trucks = new List<Truck>();
                foreach (Truck t in previous.Trucks)
                {
                    if (t.Id == id)
                    {
                        t.Status = TruckStatus.Waiting;
                        t.TimeTravel = 0;
                    }
                    newStateRow.Trucks.Add(t);
                }
            }

            newStateRow.Stat1 = previous.Stat1 + 1;
            if (previous.Shovel.Status == ShovelStatus.Free)
            {
                newStateRow.Stat2 = newStateRow.Watch - previous.Watch;
                newStateRow.Stat3 = previous.Stat3 + newStateRow.Stat2;
            }
            else
            {
                newStateRow.Stat2 = 0;
                newStateRow.Stat3 = previous.Stat3;
            }

            return newStateRow;
        }

        public double GetRandom()
        {
            if (this.GeneratorType == GeneratorType.Congruential)
                return this.Congruential.NextRnd();
            else
                return this.Random.NextDouble();
        }
    }
}
