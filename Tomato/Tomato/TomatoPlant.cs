using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    public enum TOMATO_PLANT_STATE
    {
        None,
        Growing,
        Pause,
        Giveup,
        Reaped,
        Rest,
        Finish
    }

    public class TomatoPlant
    {
        private readonly int TomatoPlantLifeSeconds = 15;
        private readonly int TomatoPlantRestSeconds = 3;
        private TomatoSeed seed;
        public TomatoSeed Seed
        {
            get
            {
                return seed;
            }
        }
        private int remainLifeSeconds;
        public int RemainLifeSeconds
        {
            get
            {
                return remainLifeSeconds;
            }
        }
        private int remainRestSeconds;
        public int RemainRestSeconds
        {
            get
            {
                return remainRestSeconds;
            }
        }
        private TOMATO_PLANT_STATE state;
        public TOMATO_PLANT_STATE State
        {
            get
            {
                return state;
            }
        }
        private string giveupRecord;
        public string GiveupRecord
        {
            get
            {
                return giveupRecord;
            }
        }
        private TomatoFruit fruit;
        public TomatoFruit Fruit
        {
            get
            {
                return fruit;
            }
        }
        public double GrowRate
        {
            get
            {
                return 1 - remainLifeSeconds / (double)TomatoPlantLifeSeconds;
            }
        }
        public double RestRate
        {
            get
            {
                return 1 - remainRestSeconds / (double)TomatoPlantRestSeconds;
            }
        }
        public double Rate
        {
            get
            {
                var growWeight = TomatoPlantLifeSeconds / (double)(TomatoPlantLifeSeconds + TomatoPlantRestSeconds);
                var restWeight = 1 - growWeight;
                return GrowRate * growWeight + RestRate * restWeight;
            }
        }
        private List<string> pasuseRecords;
        public List<string> PasuseRecords
        {
            get
            {
                return pasuseRecords;
            }
        }
        
        public event Action<double> OnGrowingRateChange;
        public event Action<double> OnRestRateChange;
        public event Action<TomatoFruit> OnReapFruit;
        public event Action<TomatoPlant> OnFinish;

        public TomatoPlant(TomatoSeed oriSeed)
        {
            seed = oriSeed;
            remainLifeSeconds = TomatoPlantLifeSeconds;
            remainRestSeconds = TomatoPlantRestSeconds;

            giveupRecord = string.Empty;
            pasuseRecords = new List<string>();
        }

        private void TimeLoseSecond()
        {
            switch (state)
            {
                case TOMATO_PLANT_STATE.Growing:
                    GrowingTimeLoseSecond();
                    break;
                case TOMATO_PLANT_STATE.Rest:
                    RestTimeLoseSecond();
                    break;
            }
        }

        private void GrowingTimeLoseSecond()
        {
            remainLifeSeconds = Math.Max(0, remainLifeSeconds - 1);
            OnGrowingRateChange(GrowRate);
            if (remainLifeSeconds <= 0)
            {
                state = TOMATO_PLANT_STATE.Reaped;
                
                fruit = new TomatoFruit(this);
                OnReapFruit?.Invoke(fruit);
            }
        }

        private void RestTimeLoseSecond()
        {
            remainRestSeconds = Math.Max(0, remainRestSeconds - 1);
            OnRestRateChange(1 - remainRestSeconds / (double)TomatoPlantRestSeconds);
            if (remainRestSeconds <= 0)
            {
                TomatoMgr.OnTimeLoseSecond -= TimeLoseSecond;

                state = TOMATO_PLANT_STATE.Finish;
                OnFinish?.Invoke(this);
            }
        }

        public void StartGrow()
        {
            state = TOMATO_PLANT_STATE.Growing;
            TomatoMgr.OnTimeLoseSecond += TimeLoseSecond;
        }

        public void StartRest()
        {
            state = TOMATO_PLANT_STATE.Rest;
        }

        public void Giveup(string giveupTip)
        {
            if ((state != TOMATO_PLANT_STATE.Growing ||
                state != TOMATO_PLANT_STATE.Pause) &&
                state < TOMATO_PLANT_STATE.Reaped)
            {
                throw new Exception("TomatoPlant giveup fail, state is " + state);
            }
            if (state < TOMATO_PLANT_STATE.Reaped)
            {
                state = TOMATO_PLANT_STATE.Giveup;
            }
            else
            {
                state = TOMATO_PLANT_STATE.Finish;
            }
            OnFinish?.Invoke(this);

            TomatoMgr.OnTimeLoseSecond -= TimeLoseSecond;

            if (string.IsNullOrEmpty(giveupTip))
            {
                throw new Exception("TomatoPlant giveup unexpect, giveupTip is empty");
            }
            giveupRecord = giveupTip;
        }

        public void Pause(string pauseTip)
        {
            if (state != TOMATO_PLANT_STATE.Growing)
            {
                throw new Exception("TomatoPlant pause fail, state is " + state);
            }
            state = TOMATO_PLANT_STATE.Pause;

            if (string.IsNullOrEmpty(pauseTip))
            {
                throw new Exception("TomatoPlant pause unexpect, pauseTip is empty");
            }
            pasuseRecords.Add(pauseTip);
        }

        public void Replay()
        {
            if (state != TOMATO_PLANT_STATE.Pause)
            {
                throw new Exception("TomatoPlant replay fail, state is " + state);
            }
            state = TOMATO_PLANT_STATE.Growing;
        }
    }
}
