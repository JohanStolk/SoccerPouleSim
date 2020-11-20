using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerPouleSim.Core
{
    class SoccerSimException : Exception
    {
        public SoccerSimException(string message)
            : base(message)
        {
        }
        public SoccerSimException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}
