using System;
namespace PatternGame
{
    interface IPublisher
    {
        event EventHandler EventHandler;
        void NotifyObservers();
    }
    interface IUnit : IPublisher
    {
        int Health { set; get; }
        int Attack { set; get; }
        int Defence { set; get; }
        int Cost { set; get; }
        string Name { set; get; }
        IUnit Fight(IUnit unit);
        IUnit Copy();
        string GetInfo();
    }

    interface ISpecialAction
    {
        IUnit DoSpecialAction(IUnit unit);
        int Strength { set; get; }
        int Range { set; get; }
    }

    interface ICurable
    {
        void Heal(int health);
    }

    interface ICloneable
    {
        IUnit Clone();
    }

    abstract class Unit : IUnit
    {
        public int Health { set; get; }
        public int Attack { set; get; }
        public int Defence { set; get; }
        public int Cost { set; get; }
        public string Name { set; get; }
        public event EventHandler EventHandler;
        public virtual IUnit Fight(IUnit unit)
        {
            if (Attack > unit.Defence + unit.Health)
                return unit;
            if (unit.Defence > Attack)
            {
                unit.Defence -= Attack;
                if (unit.Defence < 0)
                    unit.Defence = 0;
                return null;
            }
            unit.Health -= Attack - unit.Defence;
            unit.Defence = 0;
            return null;
        }
        public virtual void NotifyObservers()
        {
            EventHandler?.Invoke(this, EventArgs.Empty);
        }
        public virtual IUnit Copy()
        {
            return null;
        }
        public virtual string GetInfo()
        {
            return string.Format("{0} - attack {1}, defence {2}, health - {3}",Name, Attack, Defence, Health);
        }
    }

    class Rand
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int Get(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
    }

    class LightUnit : Unit, IUnit, ICurable, ICloneable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public LightUnit()
        {
            Health = SettingsUnit.LightUnit.Health;
            Attack = Rand.Get(SettingsUnit.LightUnit.rangeAttack.Item1, SettingsUnit.LightUnit.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.LightUnit.rangeDefence.Item1, SettingsUnit.LightUnit.rangeDefence.Item2);
            Cost = SettingsUnit.LightUnit.Cost;
            Name = SettingsUnit.LightUnit.Name;
        }

        public LightUnit(LightUnit unit)
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Cost = unit.Cost;
            Name = unit.Name;
        }

        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }

        public IUnit Clone()
        {
            return new LightUnit(this);
        }
        public override IUnit Copy()
        {
            return new LightUnit(this);
        }

        public IUnit DoSpecialAction(IUnit unit)
        {
            HeavyUnit heavy = unit as HeavyUnit;
            if (heavy == null)
                return null;

            if (heavy.Horse && heavy.Shield && heavy.Pike && heavy.Helmet)
                return null;

            switch (Rand.Get(0, 4))
            {
                case 0:
                    if (!heavy.Horse)
                    {
                        heavy.Horse = true;
                        return new HorseDecorator(unit);
                    }
                    break;
                case 1:
                    if (!heavy.Shield)
                    {
                        heavy.Shield = true;
                        return new ShieldDecorator(unit);
                    }
                    break;
                case 2:
                    if (!heavy.Pike)
                    {
                        heavy.Pike = true;
                        return new PikeDecorator(unit);
                    }
                    break;
                case 3:
                    if (!heavy.Helmet)
                    {
                        heavy.Helmet = true;
                        return new HelmetDecorator(unit);
                    }
                    break;
                default:
                    break;
            }
            return null;
        }
    }

    class HeavyUnit : Unit, IUnit
    {
        public bool Horse { get; set; }
        public bool Shield { get; set; }
        public bool Pike { get; set; }
        public bool Helmet { get; set; }
        public HeavyUnit()
        {
            Health = SettingsUnit.HeavyUnit.Health;
            Attack = Rand.Get(SettingsUnit.HeavyUnit.rangeAttack.Item1, SettingsUnit.HeavyUnit.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.HeavyUnit.rangeDefence.Item1, SettingsUnit.HeavyUnit.rangeDefence.Item2);
            Cost = SettingsUnit.HeavyUnit.Cost;
            Name = SettingsUnit.HeavyUnit.Name;
        }

        public HeavyUnit(HeavyUnit unit)
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Cost = unit.Cost;
            Name = unit.Name;
        }
        public override IUnit Copy()
        {
            return new HeavyUnit(this);
        } 
    }

    class BowmanUnit : Unit, IUnit, ICurable, ICloneable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public BowmanUnit()
        {
            Health = SettingsUnit.Bowman.Health;
            Attack = Rand.Get(SettingsUnit.Bowman.rangeAttack.Item1, SettingsUnit.Bowman.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.Bowman.rangeDefence.Item1, SettingsUnit.Bowman.rangeDefence.Item2);
            Range = SettingsUnit.Bowman.range;
            Strength = Rand.Get(SettingsUnit.Bowman.rangeStrength.Item1, SettingsUnit.Bowman.rangeStrength.Item2);
            Cost = SettingsUnit.Bowman.Cost;
            Name = SettingsUnit.Bowman.Name;
        }

        public BowmanUnit(BowmanUnit unit)
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Range = unit.Range;
            Strength = unit.Strength;
            Cost = unit.Cost;
            Name = unit.Name;
        }

        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }

        public IUnit Clone()
        {
            return new BowmanUnit(this);
        }
        public override IUnit Copy()
        {
            return new BowmanUnit(this);
        }
        public IUnit DoSpecialAction(IUnit unit)
        {
            if (Strength > unit.Defence + unit.Health)
                return this;
            unit.Defence -= Strength;
            if (unit.Defence > 0)
            {
                unit.Health += unit.Defence;
                unit.Defence = 0;
            }
            return unit;
        }
        public override string GetInfo()
        {
            return base.GetInfo() + string.Format(", strength - {0}", Strength);
        }
    }

    class HealerUnit : Unit, IUnit, ICurable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public HealerUnit()
        {
            Health = SettingsUnit.HealerUnit.Health;
            Attack = Rand.Get(SettingsUnit.HealerUnit.rangeAttack.Item1, SettingsUnit.HealerUnit.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.HealerUnit.rangeDefence.Item1, SettingsUnit.HealerUnit.rangeDefence.Item2);
            Range = SettingsUnit.HealerUnit.range;
            Strength = Rand.Get(SettingsUnit.HealerUnit.rangeStrength.Item1, SettingsUnit.HealerUnit.rangeStrength.Item2);
            Cost = SettingsUnit.HealerUnit.Cost;
            Name = SettingsUnit.HealerUnit.Name;
        }

        public HealerUnit(HealerUnit unit)
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Range = unit.Range;
            Strength = unit.Strength;
            Cost = unit.Cost;
            Name = unit.Name;
        }

        public IUnit DoSpecialAction(IUnit unit)
        {
            if (unit is ICurable)
            {
                ((ICurable)unit).Heal(Strength);
                return unit;
            }
            return null;
        }

        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }
        public override string GetInfo()
        {
            return base.GetInfo() + string.Format(", strength - {0}",Strength);
        }
    }

    class WizardUnit : Unit, IUnit, ICurable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public WizardUnit()
        {
            Health = SettingsUnit.Wizard.Health;
            Attack = Rand.Get(SettingsUnit.Wizard.rangeAttack.Item1, SettingsUnit.Wizard.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.Wizard.rangeDefence.Item1, SettingsUnit.Wizard.rangeDefence.Item2);
            Strength = Rand.Get(SettingsUnit.Wizard.rangeStrength.Item1, SettingsUnit.Wizard.rangeStrength.Item2);
            Range = SettingsUnit.Wizard.range;
            Cost = SettingsUnit.Wizard.Cost;
            Name = SettingsUnit.Wizard.Name;
        }

        public WizardUnit(WizardUnit unit)
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Range = unit.Range;
            Cost = unit.Cost;
            Name = unit.Name;
        }

        public IUnit DoSpecialAction(IUnit unit)
        {
            if (unit is ICloneable)
            {
                int rand = Rand.Get(0, 100);
                if (rand < Strength)
                    return ((ICloneable)unit).Clone();
            }
            return null;
        }

        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }
        public override string GetInfo()
        {
            return base.GetInfo() + string.Format(", strength - {0}", Strength);
        }
    }
}
