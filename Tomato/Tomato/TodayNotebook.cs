using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    public class TodayNotebook
    {
        private List<TomatoSeed> goalSeeds;
        public List<TomatoSeed> GoalSeeds
        {
            get
            {
                return goalSeeds;
            }
        }

        public event Action<TomatoSeed> OnLeaveTodayDo;

        public TodayNotebook()
        {
            goalSeeds = new List<TomatoSeed>();
        }

        public TomatoSeed AddTodayGoal(TomatoSeed seed)
        {
            goalSeeds.Add(seed);
            return seed;
        }

        public TomatoSeed LeaveTodayDo(TomatoSeed seed)
        {
            if (!goalSeeds.Remove(seed))
            {
                throw new Exception("GoalNotebool todayDo fail, seed not exist in the goalSeeds.");
            }
            OnLeaveTodayDo?.Invoke(seed);
            return seed;
        }

        public void GiveupSeed(TomatoSeed seed)
        {
            goalSeeds.Remove(seed);
        }
    }
}
