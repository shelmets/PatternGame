using System;
using System.Linq;

namespace PatternGame
{
    static class SettingsUnit
    {
        public static int MinCost = new int[]{ LightUnit.Cost, HeavyUnit.Cost, Bowman.Cost, HealerUnit.Cost, Wizard.Cost}.Min(n=>n);
        public static class LightUnit
        {
            public static readonly int Health = 100;
            public static readonly int Cost = 10;
            public static readonly string Name = "Light Unit";
            public static readonly Tuple<int, int> rangeAttack = Tuple.Create(30,50);
            public static readonly Tuple<int, int> rangeDefence = Tuple.Create(20,40);
        }
        public static class HeavyUnit
        {
            public static readonly int Health = 100;
            public static readonly int Cost = 10;
            public static readonly string Name = "Heavy Unit";
            public static readonly Tuple<int, int> rangeAttack = Tuple.Create(50, 100);
            public static readonly Tuple<int, int> rangeDefence = Tuple.Create(40, 70);
        }
        public static class Bowman
        {
            public static readonly int Health = 100;
            public static readonly int Cost = 10;
            public static readonly string Name = "Bowman";
            public static readonly Tuple<int, int> rangeAttack = Tuple.Create(30, 50);
            public static readonly Tuple<int, int> rangeDefence = Tuple.Create(10, 30);
            public static readonly int range = 5;
            public static readonly Tuple<int,int> rangeStrength = Tuple.Create(70,100);
        }
        public static class HealerUnit
        {
            public static readonly int Health = 100;
            public static readonly int Cost = 10;
            public static readonly string Name = "Healer Unit";
            public static readonly Tuple<int, int> rangeAttack = Tuple.Create(0, 10);
            public static readonly Tuple<int, int> rangeDefence = Tuple.Create(0, 10);
            public static readonly int range = 1;
            public static readonly Tuple<int, int> rangeStrength = Tuple.Create(70, 100);
        }
        public static class Wizard
        {
            public static readonly int Health = 100;
            public static readonly int Cost = 10;
            public static readonly string Name = "Wizard";
            public static readonly Tuple<int, int> rangeAttack = Tuple.Create(0, 10);
            public static readonly Tuple<int, int> rangeDefence = Tuple.Create(0, 10);
            public static readonly int range = 1;
            public static readonly Tuple<int, int> rangeStrength = Tuple.Create(40, 100);
        }
    }
}
