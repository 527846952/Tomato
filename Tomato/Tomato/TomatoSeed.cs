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
        Sowed
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
        private TOMATO_SEED_STATE state;
        public TOMATO_SEED_STATE State
        {
            get
            {
                return state;
            }
        }

        public TomatoSeed()
        {
            state = TOMATO_SEED_STATE.Ready;
        }

        public TomatoPlant Sow()
        {
            if (state != TOMATO_SEED_STATE.Ready)
            {
                throw new Exception("TomatoSeed sow fail, state is " + state);
            }
            state = TOMATO_SEED_STATE.Sowed;
            return new TomatoPlant(this);
        }
    }
}
