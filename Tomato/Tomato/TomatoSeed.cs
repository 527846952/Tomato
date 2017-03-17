using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    enum TOMATO_PRI
    {
        S,
        A,
        B,
        C,
        D,
    }
    enum TOMATO_SEED_STATE
    {
        Ready,
        Sowed,
        Excess,
        Finish
    }

    class TomatoSeed
    {
        private string title;
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }
        private string detail;
        public string Detail
        {
            get
            {
                return detail;
            }

            set
            {
                detail = value;
            }
        }
        private int expectTomatoCount;
        public int ExpectTomatoCount
        {
            get
            {
                return expectTomatoCount;
            }

            set
            {
                expectTomatoCount = value;
            }
        }
        private int unexpectTomatoCount;
        public int UnexpectTomatoCount
        {
            get
            {
                return unexpectTomatoCount;
            }
        }
        private TOMATO_SEED_STATE state;
        public TOMATO_SEED_STATE State
        {
            get
            {
                return state;
            }
        }
        private List<TomatoPlant> allPlants;
        public List<TomatoPlant> AllPlants
        {
            get
            {
                return allPlants;
            }
        }

        public TomatoSeed()
        {
            state = TOMATO_SEED_STATE.Ready;
            allPlants = new List<TomatoPlant>();
        }

        public List<TomatoPlant> Sow()
        {
            var plants = new List<TomatoPlant>();
            if (state != TOMATO_SEED_STATE.Ready)
            {
                throw new Exception("TomatoSeed sow fail, state is " + state);
            }
            state = TOMATO_SEED_STATE.Sowed;
            for (int i = 0; i < expectTomatoCount; i++)
            {
                plants.Add(new TomatoPlant(this));
            }
            allPlants.AddRange(plants);
            return plants;
        }

        public List<TomatoPlant> ExcessSow(int addCount)
        {
            if (addCount <= 0)
            {
                throw new Exception("TomatoSeed excessSow fail, addCount is " + addCount);
            }
            unexpectTomatoCount += addCount;
            var plants = new List<TomatoPlant>();
            if (state != TOMATO_SEED_STATE.Sowed ||
                state != TOMATO_SEED_STATE.Excess)
            {
                throw new Exception("TomatoSeed excessSow fail, state is " + state);
            }
            state = TOMATO_SEED_STATE.Excess;
            for (int i = 0; i < addCount; i++)
            {
                plants.Add(new TomatoPlant(this));
            }
            allPlants.AddRange(plants);
            return plants;
        }

        public void Finish()
        {
            if (state != TOMATO_SEED_STATE.Sowed ||
                state != TOMATO_SEED_STATE.Excess)
            {
                throw new Exception("TomatoSeed finish fail, state is " + state);
            }
            state = TOMATO_SEED_STATE.Finish;
        }
    }
}
