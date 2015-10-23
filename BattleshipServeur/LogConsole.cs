using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer
{
    class LogConsole
    {
        public static void Log(String Message)
        {
            Console.WriteLine(">> " + Message);
        }
        public static void LogWithTime(String Message)
        {
            Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ">> " + Message);
        }
    }
}
