using System;
namespace PatternGame
{
    class FactoryUnit
    {
        public IUnit GetUnit()
        {
            int probability = Rand.Get(0, 100);

            if (probability < 50)
                return new LightUnit();
            else if (probability < 70)
                return new Proxy(new HeavyUnit());
            else if (probability < 80)
                return new HealerUnit();
            else if (probability < 90)
                return new BowmanUnit();
            else
                return new WizardUnit();
        }
    }
    class FactoryArmy
    {
        public Army GetArmy(int money, string name)
        {
            var unitFactory = new FactoryUnit();
            var army = new Army(name);

            while (money >= SettingsUnit.MinCost)
            {
                var unit = unitFactory.GetUnit();
                if (money - unit.Cost >= 0)
                {
                    army.Push(unit);
                    money -= unit.Cost;
                }
            }
            return army;
        }
    }
}
