using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Logic
{
    public class RandHelper
    {
        private int seed;
        private Random rand;

        public RandHelper(int _seed)
        {
            if (_seed == 0)
            {
                _seed = DateTime.Now.Second;
            }
            rand = new Random(_seed);
            seed = _seed;
        }

        public void SetSeed(int _seed)
        {
            seed = _seed;
        }

        public int GetSeed()
        {
            return seed;
        }

        public int Random(int max)
        {
            return rand.Next(max);
        }

        public int Random(int min, int max)
        {
            return rand.Next(min, max);
        }
    }
}
