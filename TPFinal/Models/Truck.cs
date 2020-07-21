using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TPFinal.Models.Enums;

namespace TPFinal.Models
{
    public class Truck
    {
        public int Id { get; set; }
        public double TimeTravel { get; set; }
        public TruckStatus Status { get; set; }
    }
}
