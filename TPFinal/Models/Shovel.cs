using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TPFinal.Models.Enums;

namespace TPFinal.Models
{
    public class Shovel
    {
        public ShovelStatus Status { get; set; }
        public Queue<Truck> TruckQueue { get; set; }
        public double InactiveTime { get; set; }
    }
}
