using System;
using System.Collections.Generic;
using System.Text;

namespace FieldBoundary
{
    class BoundaryInfo
    {
        public Feature[] features { get; set; }
    }

    class Feature
    {
        public Geometry geometry { get; set; }
        public int id { get; set; }
    }

    class Geometry
    {
        public double[][][] coordinates { get; set; }
    }
}
