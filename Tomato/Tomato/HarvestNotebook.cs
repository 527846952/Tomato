using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato
{
    class HarvestNotebook
    {
        private List<TomatoFruit> fruits;
        public List<TomatoFruit> Fruits
        {
            get
            {
                return fruits;
            }
        }

        public HarvestNotebook()
        {
            fruits = new List<TomatoFruit>();
        }

        public TomatoFruit AddTodayGoal(TomatoFruit fruit)
        {
            fruits.Add(fruit);
            return fruit;
        }
    }
}
