using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    enum TOMATO_PLANT_STATE
    {
        Growing,
        Pause,
        Giveup,
        Reaped,
    }

    class TomatoPlant
    {
        private readonly int TomatoPlantLifeSeconds = 1500;
        private TomatoSeed seed;
        private int remainLifeSeconds;
        private TOMATO_PLANT_STATE state;
        public TOMATO_PLANT_STATE State
        {
            get
            {
                return state;
            }
        }

        public event Action<double> OnRateChange;
        public event Action OnReapFruit;

        public TomatoPlant(TomatoSeed oriSeed)
        {
            seed = oriSeed;
            remainLifeSeconds = TomatoPlantLifeSeconds;
        }

        public void TimeLoseSecond()
        {
            if (state != TOMATO_PLANT_STATE.Growing)
            {
                return;
            }
            remainLifeSeconds = Math.Max(0, remainLifeSeconds - 1);
            OnRateChange(1 - remainLifeSeconds / (double)TomatoPlantLifeSeconds);
            if (remainLifeSeconds <= 0)
            {
                state = TOMATO_PLANT_STATE.Reaped;
                OnReapFruit?.Invoke();
            }
        }

        public void Giveup(string giveupTip)
        {
            if (state != TOMATO_PLANT_STATE.Growing ||
                state != TOMATO_PLANT_STATE.Pause)
            {
                throw new Exception("TomatoPlant giveup fail, state is " + state);
            }
            state = TOMATO_PLANT_STATE.Giveup;
        }

        public void Pause(string pauseTip)
        {
            if (state != TOMATO_PLANT_STATE.Growing)
            {
                throw new Exception("TomatoPlant pause fail, state is " + state);
            }
            state = TOMATO_PLANT_STATE.Pause;
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
