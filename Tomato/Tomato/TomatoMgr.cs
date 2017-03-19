using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    class TomatoMgr
    {
        public static event Action OnTimeLoseSecond;

        public static void TimeLoseSecond()
        {
            OnTimeLoseSecond?.Invoke();
        }
    }
}
