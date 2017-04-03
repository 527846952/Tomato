using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    public enum TOMATO_PRI
    {
        S,
        A,
        B,
        C,
        D,
    }
    public enum TOMATO_SEED_STATE
    {
        Ready,
        Sowed,
        Excess,
        Finish
    }

    public class TomatoSeed
    {
        public static readonly int MAX_TOMATO_PLANT_COUNT = 16;
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
        public int SumTomatoCount
        {
            get
            {
                return expectTomatoCount + unexpectTomatoCount;
            }
        }
        private DateTime createTime;
        public DateTime CreateTime
        {
            get
            {
                return createTime;
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
        public int CurGrowPlantIdx
        {
            get
            {
                return curGrowPlantIdx;
            }
        }
        public int RemainTomatoCount
        {
            get
            {
                return expectTomatoCount + unexpectTomatoCount - curGrowPlantIdx;
            }
        }
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
        private TOMATO_PRI priority;
        public TOMATO_PRI Priority
        {
            get
            {
                return priority;
            }

            set
            {
                priority = value;
            }
        }

        public event Action<TomatoPlant> OnPlantFinish;
        public event Action<TomatoSeed> OnFinishAllPlant;

        public TomatoSeed()
        {
            curGrowPlantIdx = -1;
            state = TOMATO_SEED_STATE.Ready;
            allPlants = new List<TomatoPlant>();

            createTime = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", priority, title);
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

        private void OnSelfPlantFinish(TomatoPlant plant)
        {
            var minPlantIdx = GetMinPlantIdx();
            if (minPlantIdx < 0 && curGrowPlantIdx == allPlants.Count - 1)
            {
                OnFinishAllPlant?.Invoke(this);
            }
            else
            {
                OnPlantFinish?.Invoke(plant);
            }
        }

        public TomatoPlant SelectNextPlant()
        {
            var minPlantIdx = GetMinPlantIdx();
            if (minPlantIdx < 0)
            {
                return null;
            }
            curGrowPlantIdx = minPlantIdx;
            var minPlant = allPlants[curGrowPlantIdx];
            return minPlant;
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
                plant.OnFinish += OnSelfPlantFinish;
                plants.Add(plant);
            }
            allPlants.AddRange(plants);

            var curPlant = SelectNextPlant();
            curPlant.StartGrow();
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
                plant.OnFinish += OnSelfPlantFinish;
                plants.Add(plant);
            }
            allPlants.AddRange(plants);

            var curPlant = SelectNextPlant();
            curPlant.StartGrow();
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
