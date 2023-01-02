using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
{
    internal class ItemsComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x == null && y == null)
                return 1;
            return x.ToString().Length.CompareTo(y.ToString().Length);
        }
    }
}
