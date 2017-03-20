using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    public class GoalNotebook
    {
        private List<TomatoSeed> goalSeeds;
        public List<TomatoSeed> GoalSeeds
        {
            get
            {
                return goalSeeds;
            }
        }

        public event Action<TomatoSeed> OnDecideTodayDo;

        public GoalNotebook()
        {
            goalSeeds = new List<TomatoSeed>();
        }

        public TomatoSeed PlanGoal(string title, string detail, int expectTomatoCount, TOMATO_PRI priority)
        {
            var seed = new TomatoSeed();
            seed.Title = title;
            seed.Detail = detail;
            seed.ExpectTomatoCount = expectTomatoCount;
            seed.Priority = priority;

            goalSeeds.Add(seed);

            return seed;
        }

        public bool GiveupGoal(TomatoSeed seed)
        {
            return goalSeeds.Remove(seed);
        }

        public void TodayDo(TomatoSeed seed)
        {
            if (!goalSeeds.Remove(seed))
            {
                throw new Exception("GoalNotebool todayDo fail, seed not exist in the goalSeeds.");
            }
            OnDecideTodayDo?.Invoke(seed);
        }

        public void GiveupSeed(TomatoSeed seed)
        {
            goalSeeds.Remove(seed);
        }
    }
}
