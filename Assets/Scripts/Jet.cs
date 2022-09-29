using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Jet : ITransport
    {
        public bool Arrive()
        {
            if (Random.Range(0, 50) < 1)
            {
                return false;
            }
            return true;
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
            //Random random = new Random();
            if (Random.Range(0f, 100f) < .01f)
            {
                return false;
            }
            return true;
        }

        public bool EngineRecovery()
        {
            if (Random.Range(0f, 100f) < .1f)
            {
                return true;
            }
            return false;
        }
    }
}
