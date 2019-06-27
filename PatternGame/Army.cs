using System;
using System.Collections.Generic;

namespace PatternGame
{
    class Army
    {
        List<IUnit> army = new List<IUnit>();

        public string Name { get; set; }

        public Army(string name) {
            Name = name;
        }

        public Army(Army copy)
        {
            Name = copy.Name;
            foreach (IUnit unit in copy.army)
            {
                Push(unit.Copy());
            }
        }

        public Army GetSnapshot()
        {
            return new Army(this);
        }
        
        public void Push(IUnit unit)
        {
            army.Add(unit);
        }

        public IUnit Pop()
        {
            if (army.Count != 0)
            {
                IUnit last = army[army.Count - 1];
                army.RemoveAt(army.Count - 1);
                return last;
            }
            return null;
        }

        public int Remove(IUnit unit)
        {
            int index = army.IndexOf(unit);
            army.Remove(unit);
            return index;
        }

        public IUnit Last()
        {
            
            return (army.Count!=0)? army[army.Count - 1]: null;
        }

        public int Count()
        {
            return army.Count;
        }
        public bool IsEmpty()
        {
            return army.Count == 0;
        }
        public int IndexOf(IUnit unit)
        {
            return army.IndexOf(unit);
        }
        public IUnit this[int i]
        {
            get
            {
                return army[i];
            }
        }    
    }
}
