using System;
using System.IO;
namespace PatternGame
{
    class Proxy: HeavyUnit
    {
        HeavyUnit heavyUnit;
        public Proxy(HeavyUnit unit)
        {
            heavyUnit = unit;
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Cost = unit.Cost;
            Name = unit.Name;
        }
        public override IUnit Fight(IUnit unit)
        {
            var text = String.Format("Battle. {0} vs {1}. ", GetInfo(), unit.GetInfo());
            IUnit dead = heavyUnit.Fight(unit);
            if (dead == null)
                text += String.Format("Before {0} .", unit.GetInfo());
            else
                text += "Противник погиб. ";
            LogProxy(text);
            return dead;
        }
        public void LogProxy(string text)
        {
            using (StreamWriter sw = new StreamWriter("HeavyUnitProxyLog.log", true))
            {
                sw.WriteLine(text);
            }
        }

        public override IUnit Copy()
        {
            return new Proxy(this);
        }
        public override string GetInfo()
        {
            return heavyUnit.GetInfo();
        }
    }
}
