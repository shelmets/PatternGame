using System;
using System.Collections.Generic;

namespace PatternGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Logic logic = new Logic();
            logic.ClearLog();
            logic.ShowMenu();
        }
    }
}
