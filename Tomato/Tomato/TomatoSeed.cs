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
        private int curGrowPlantIdx;
        public double Rate
        {
            get
            {
                if (allPlants.Count <= 0)
                {
                    return 0;
                }
                var sumRate = 0d;
                for (int i = 0; i < allPlants.Count; i++)
                {
                    sumRate += allPlants[i].Rate;
                }
                return sumRate / allPlants.Count;
            }
        }

        public TomatoSeed()
        {
            curGrowPlantIdx = -1;
            state = TOMATO_SEED_STATE.Ready;
            allPlants = new List<TomatoPlant>();
        }

        private int GetMinPlantIdx()
        {
            for (int i = 0; i < allPlants.Count; i++)
            {
                if (allPlants[i].State < TOMATO_PLANT_STATE.Finish)
                {
                    return i;
                }
            }
            return allPlants.Count - 1;
        }

        private void OnPlantFinish(TomatoPlant plant)
        {

        }

        public void GrowNextPlant()
        {
            var minPlantIdx = GetMinPlantIdx();
            if (minPlantIdx < 0 || curGrowPlantIdx == minPlantIdx)
            {
                return;
            }
            curGrowPlantIdx = minPlantIdx;
            allPlants[curGrowPlantIdx].StartGrow();
        }

        public void Sow()
        {
            var plants = new List<TomatoPlant>();
            if (state != TOMATO_SEED_STATE.Ready)
            {
                throw new Exception("TomatoSeed sow fail, state is " + state);
            }
            state = TOMATO_SEED_STATE.Sowed;
            for (int i = 0; i < expectTomatoCount; i++)
            {
                var plant = new TomatoPlant(this);
                plant.OnFinish += OnPlantFinish;
                plants.Add(plant);
            }
            allPlants.AddRange(plants);

            GrowNextPlant();
        }

        public void ExcessSow(int addCount)
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
                var plant = new TomatoPlant(this);
                plant.OnFinish += OnPlantFinish;
                plants.Add(plant);
            }
            allPlants.AddRange(plants);

            GrowNextPlant();
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
