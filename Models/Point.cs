using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.Models
{
    public class Point
    {
        private double x;
        private double y;

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Point() { }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
    }
}
