using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPFinal.Models
{
    public class Enums
    {
        public enum GeneratorType
        {
            Congruential = 0,
            Language
        }

        public enum ShovelStatus
        {
            Free = 0,
            Busy
        }

        public enum TruckStatus
        {
            InTravel = 0,
            Waiting,
            InLoad
        }
    }
}
