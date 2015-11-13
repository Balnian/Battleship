using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipShared
{
    namespace UtilityTools
    {
        public class LogConsole
        {
            /// <summary>
            /// Écrit dans la console
            /// </summary>
            /// <param name="Message"></param>
            public static void Log(String Message)
            {
                Console.WriteLine(">> " + Message);
            }

            /// <summary>
            /// Écrit dans la console en y indiquant la date et l'heure
            /// </summary>
            /// <param name="Message"></param>
            public static void LogWithTime(String Message)
            {
                Console.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ">> " + Message);
            }
        }
    }
}
