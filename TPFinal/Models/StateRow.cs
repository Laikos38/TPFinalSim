using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPFinal.Models
{
    public class StateRow
    {
        public int IterationNum { get; set; }
        public string Event { get; set; }
        public double Watch { get; set; }
        public double LoadRnd { get; set; }
        public double TimeEndLoad { get; set; }
        public double NextEndLoad { get; set; }
        public double TravelRnd { get; set; }
        public double TimeEndTravel { get; set; }
        public double NextEndTravel { get; set; }
        public Shovel Shovel { get; set; }
        public double Stat1 { get; set; }
        public double Stat2 { get; set; }
        public double Stat3 { get; set; }
        public List<Truck> Trucks { get; set; }
    }
}
