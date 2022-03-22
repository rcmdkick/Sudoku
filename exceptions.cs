using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class NincsMegoldasKivetel : Exception
    {
        public NincsMegoldasKivetel(string message="[Kivétel] Nincs megoldás") : base(message)
        {
        }
    }
}
