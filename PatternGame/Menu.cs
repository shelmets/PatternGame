using System;
using System.IO;

namespace PatternGame
{
    public class Logic
    {
        Army firstArmy, secondArmy;
        Battlefield battlefield;
        CommandInvoker invoker;
        bool subscribed;
        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("1. Create armies");
            Console.WriteLine("2. Show armies");
            Console.WriteLine("3. Make a move");
            Console.WriteLine("4. Play till the end");
            Console.WriteLine("5. Undo last move");
            Console.WriteLine("6. Redo last move");
            Console.WriteLine("7. Subscribe");
            Console.WriteLine("8. Unsubscribe");
            Console.WriteLine("9. Change stategy");
            Console.WriteLine();

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Enter the amount for which your first army will be purchased:");
                    int money;
                    while (!Int32.TryParse(Console.ReadLine(), out money))
                        Console.WriteLine("Error. Enter the integer.");

                    var factoryArmy = new FactoryArmy();
                    firstArmy = factoryArmy.GetArmy(money, "1");

                    Console.WriteLine("Enter the amount for which your second army will be purchased:");
                    while (!Int32.TryParse(Console.ReadLine(), out money))
                        Console.WriteLine("Error. Enter the integer.");

                    secondArmy = factoryArmy.GetArmy(money, "2");

                    battlefield = new Battlefield(firstArmy, secondArmy);
                    invoker = new CommandInvoker(battlefield);

                    Write(String.Format("Armies created.\n{0}", battlefield.GetArmyInfo()));
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "2":
                    Console.WriteLine(battlefield.GetArmyInfo());
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "3":
                    invoker.Move();

                    Write(battlefield.MoveInfo);
                    Console.ReadLine();
                    if (!battlefield.EndOfGame)
                        ShowMenu();
                    break;
                case "4":
                    invoker.PlayToTheEnd();

                    Write(battlefield.GameInfo);
                    Console.ReadLine();
                    break;
                case "5":
                    invoker.Undo();

                    Write("Last move canceled. ");
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "6":
                    invoker.Redo();

                    Write("Last move repeated. ");
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "7":
                    if (subscribed)
                        Console.WriteLine("Subscription has already been issued. ");
                    else
                    {
                        battlefield.Subscribe();
                        subscribed = true;
                        Write("Subscribed. ");
                    }
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "8":
                    if (!subscribed)
                        Console.WriteLine("Subscription has not already been issued. ");
                    else
                    {
                        battlefield.UnSubscribe();
                        subscribed = false;
                        Write("Unsubscribed. ");
                    }
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "9":
                    Console.WriteLine("Input a number of strategy: 1. 1x1  2. 3x3  3. NxN");
                    string strategy = Console.ReadLine();
                    switch (strategy)
                    {
                        case "1":
                            battlefield.Strategy = new OneToOneStrategy();
                            Write("Strategy 1х1 is established");
                            break;
                        case "2":
                            battlefield.Strategy = new ThreeToThreeStrategy();
                            Write("Strategy 3х3 is established");
                            break;
                        case "3":
                            battlefield.Strategy = new NToNStrategy(firstArmy, secondArmy);
                            Write("Strategy NхN is established");
                            break;
                        default:
                            Console.WriteLine("Error. There is no such item. Plaese try again.");
                            break;
                    }
                    Console.ReadLine();
                    ShowMenu();
                    break;
                default:
                    Console.WriteLine("Error. There is no such item. Please try again.");
                    Console.ReadLine();
                    ShowMenu();
                    break;
            }
        }

        public void Write(string text)
        {
            Console.WriteLine(text);
            using (StreamWriter sw = new StreamWriter("Game.log", true))
            {
                sw.WriteLine(text);
            }
        }
        public void ClearLog()
        {
            File.WriteAllText("Game.log", "");
        }
    }
}
