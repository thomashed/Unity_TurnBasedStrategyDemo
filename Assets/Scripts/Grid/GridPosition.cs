using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolBeans.Grid
{
    public struct GridPosition 
    {
        public int X;
        public int Z;

        public GridPosition(int x, int z)
        {
            X = x;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X}, {Z}"; 
        }

    } 
}
