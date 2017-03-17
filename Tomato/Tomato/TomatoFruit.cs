using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    class TomatoFruit
    {
        private TomatoPlant plant;
        private string reapTip;
        public string ReapTip
        {
            get
            {
                return reapTip;
            }

            set
            {
                reapTip = value;
            }
        }

        public TomatoFruit(TomatoPlant oriPlant)
        {
            plant = oriPlant;
        }
    }
}
