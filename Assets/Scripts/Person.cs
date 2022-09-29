using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class Person : ITransport
    {
        public bool Arrive()
        {
            throw new NotImplementedException();
        }

        public bool PrepareForTravel()
        {
            if (GameManager.Instance.totalLuggage < 50)
            {
                return true;
            }
            return false;
        }

        public bool Travel()
        {
            throw new NotImplementedException();
        }
    }
}
