using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapsLibrary
{
    public static partial class SnapsEngine
    {
        static Random rand = new Random();

        public static int ThrowDice()
        {
            return rand.Next(1, 7);
        }
    }
}