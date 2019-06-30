using System;
using System.Collections.Generic;

namespace PatternGame
{
    class Battlefield
    {
        public IStrategy Strategy { get; set; }
        public Army FirstArmy { get; set; }
        public Army SecondArmy { get; set; }

        public string MoveInfo { get; set; }
        public string GameInfo { get; set; }
        public bool EndOfGame { get; set; }

        public Battlefield(Army first, Army second)
        {
            FirstArmy = first;
            SecondArmy = second;
            Strategy = new OneToOneStrategy();
        }

        public string GetArmyInfo()
        {
            return String.Format("{0}\n\n in opposition \n\n{1}", Strategy.GetInfo(FirstArmy), Strategy.GetInfo(SecondArmy));
        }

        public void Subscribe()
        {
            for (int i = 0; i < FirstArmy.Count(); i++)
                FirstArmy[i].EventHandler += PlayMusic;
            for (int i = 0; i < SecondArmy.Count(); i++)
                SecondArmy[i].EventHandler += PlayMusic;
        }

        public void UnSubscribe()
        {
            for (int i = 0; i < FirstArmy.Count(); i++)
                FirstArmy[i].EventHandler -= PlayMusic;
            for (int i = 0; i < SecondArmy.Count(); i++)
                SecondArmy[i].EventHandler -= PlayMusic;
        }

        private void PlayMusic(object sender, EventArgs e)
        {
            Console.Beep();
        }

        public void Move()
        {
            if (EndOfGame)
            {
                MoveInfo = "Game over. ";
                return;
            }

            MoveInfo = "\nBattle. ";

            Fight(FirstArmy, SecondArmy);
            Fight(SecondArmy, FirstArmy);
            DoSpecialAction(FirstArmy, SecondArmy);
            DoSpecialAction(SecondArmy, FirstArmy);
        }

        public void PlayToTheEnd()
        {
            while (!EndOfGame)
            {
                Move();
                GameInfo += MoveInfo;
            }
        }

        public List<IUnit> GetFirstLine(Army army)
        {
            var firstLine = new List<IUnit>();
            var min = Math.Min(Strategy.rowSize, army.Count());
            for (int i = 0; i < min; i++)
                firstLine.Add(army[army.Count() - 1 - i]);
            return firstLine;
        }

        public List<ISpecialAction> GetSpecialUnitsInRow(Army army, int row)
        {
            var specials = new List<ISpecialAction>();
            for (int i = army.Count() - row - 1; i >= 0; i -= Strategy.rowSize)
                if (army[i] is ISpecialAction)
                    specials.Add(army[i] as ISpecialAction);
            return specials;
        }

        public List<IUnit> GetTargets(Army first, Army second, ISpecialAction unit)
        {
            if (unit is BowmanUnit)
                return Strategy.GetOutsideTargets(first, second, unit);
            else
                return Strategy.GetInsideTargets(first, unit);
        }

        public void Fight(Army first, Army second)
        {
            if (IsGameFinished())
                return;

            List<IUnit> firstLineInFirst = GetFirstLine(first);
            List<IUnit> firstLineInSecond = GetFirstLine(second);

            var min = Math.Min(Strategy.rowSize, Math.Min(first.Count(), second.Count())); 
            for (int i = 0; i < min; i++)
            {
                IUnit attacker = firstLineInFirst[i];
                IUnit victim = firstLineInSecond[i];

                MoveInfo += String.Format("\n\nArmy {0}. {1}\n\n in opposition \n\nArmy {2}. {3}", first.Name, attacker.GetInfo(), second.Name, victim.GetInfo());
                IUnit dead = attacker.Fight(victim);

                if (dead != null)
                {
                    MoveInfo += string.Format("\n\nArmy {0}. {1} dead.\n", second.Name, dead.Name);
                    dead.NotifyObservers();
                    second.Remove(dead);
                }
                else
                    MoveInfo += String.Format("\n\nArmy {0}. {1} attacked.\n", second.Name, victim.GetInfo());
            }
        }

        public void DoSpecialAction(Army first, Army second)
        {
            if (IsGameFinished() || first.Count() < Strategy.rowSize + 1)
                return;

            for (int i = 0; i < Strategy.rowSize; i++)
            {
                var specials = GetSpecialUnitsInRow(first, i);
                if (specials.Count == 0)
                    continue;
                int specialIndex = Rand.Get(0, specials.Count);
                var victims = GetTargets(first, second, specials[specialIndex]);
                if (victims.Count == 0)
                    continue;
                int victimIndex = Rand.Get(0, victims.Count);

                IUnit beforeSpecial = victims[victimIndex].Copy();
                IUnit afterSpecial = specials[specialIndex].DoSpecialAction(victims[victimIndex]);

                MoveInfo += "\nSpecial action.";

                if (specials[specialIndex] is BowmanUnit)
                {
                    MoveInfo += String.Format("\n\nArmy {0}. {1} in opposition \n\nArmy {2}. {3}", first.Name, ((IUnit)specials[specialIndex]).GetInfo(), second.Name, beforeSpecial.GetInfo());
                    if (afterSpecial == specials[specialIndex])
                    {
                        MoveInfo += String.Format("\n\nArmy {0}. {1} dead.\n", second.Name, victims[victimIndex].Name);
                        afterSpecial.NotifyObservers();
                        second.Remove(afterSpecial);
                    }
                    else
                        MoveInfo += String.Format("\n\nArmy {0}. {1} attacked\n", second.Name, victims[victimIndex].GetInfo());
                }
                else if (specials[specialIndex] is HealerUnit)
                {
                    if (afterSpecial != null)
                    {
                        MoveInfo += String.Format("\nArmy {0}. {1}\n\n\t healing \n\nArmy {2}. {3}", first.Name, ((IUnit)specials[specialIndex]).GetInfo(), first.Name, beforeSpecial.GetInfo());
                        MoveInfo += String.Format("\n\nArmy {0}. {1} healed\n", first.Name, victims[victimIndex].GetInfo());
                    }
                    else
                    {
                        MoveInfo += String.Format("\n\nNo one was cured from the army {0}. ", first.Name);
                    }
                }
                else if (specials[specialIndex] is WizardUnit)
                {
                    if (afterSpecial != null)
                    {
                        MoveInfo += String.Format("\nАrmy {0}. {1}\n\n\t cloning \n\nАrmy {2}. {3}", first.Name, ((IUnit)specials[specialIndex]).GetInfo(), first.Name, beforeSpecial.GetInfo());
                        MoveInfo += String.Format("\n\nАrmy {0}. {1} cloned.\n", first.Name, victims[victimIndex].GetInfo());
                    }
                    else
                    {
                        MoveInfo += String.Format("\n\nNo one was cloned from the army {0}.\n", first.Name);
                    }
                }
                else
                {
                    if (specials[specialIndex] is LightUnit)
                    {
                        if (afterSpecial != null)
                        {
                            MoveInfo += String.Format("\nArmy {0}. {1}\n\n\t dressing \n\nArmy {2}. {3}", first.Name, ((IUnit)specials[specialIndex]).GetInfo(), first.Name, beforeSpecial.GetInfo());
                            MoveInfo += String.Format("\n\nArmy {0}. {1} dressed.\n", first.Name, victims[victimIndex].GetInfo());
                        }
                        else
                        {
                            MoveInfo += String.Format("\n\nNo one was dressed from the army {0}. ", first.Name);
                        }
                    }
                }
            }
        }

        public bool IsGameFinished()
        {
            if (EndOfGame)
                return true;

            if (FirstArmy.IsEmpty() || SecondArmy.IsEmpty())
            {
                EndOfGame = true;
                MoveInfo += "\n\n\t******** Game over. ";
                if (FirstArmy.IsEmpty())
                    MoveInfo += "The second army won. ********\n";
                else
                    MoveInfo += "The first army won. ********\n";
                return true;
            }
            return false;
        }
    }
}
