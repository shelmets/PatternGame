using System;
namespace PatternGame
{
    abstract class HeavyUnitDecorator : Unit
    {
        public IUnit Unit { set; get; }
        public HeavyUnitDecorator(IUnit unit)
        {
            Unit = unit;
        }
    }

    class HorseDecorator : HeavyUnitDecorator
    {
        public HorseDecorator(IUnit unit) : base(unit)
        {
            Unit.Attack += 10;
            Unit.Name += " on horse";
        }
    }

    class ShieldDecorator : HeavyUnitDecorator
    {
        public ShieldDecorator(IUnit unit)
            : base(unit)
        {
            Unit.Defence += 20;
            Unit.Name += " with shield";
        }
    }

    class PikeDecorator : HeavyUnitDecorator
    {
        public PikeDecorator(IUnit unit)
            : base(unit)
        {
            Unit.Attack += 20;
            Unit.Name += " with pike";
        }
    }

    class HelmetDecorator : HeavyUnitDecorator
    {
        public HelmetDecorator(IUnit unit)
            : base(unit)
        {
            Unit.Defence += 10;
            Unit.Name += " in helmet";
        }
    }
}
