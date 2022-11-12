using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq_feladat
{
    class Ramen
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Brand { get; set; }

        public int CountryFK { get; set; }

        public Country Country { get; set; }

        public double Rating { get; set; }
    }
}
