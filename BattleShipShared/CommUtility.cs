using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipShared
{
    namespace UtilityTools
    {
        public class CommUtility
        {
            public static void SerializeAndSend(NetworkStream ns, object o)
            {
                if (o.GetType().IsSerializable)
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ns, o);
                }
            }

            public static object ReadAndDeserialize(NetworkStream ns)
            {
                IFormatter formatter = new BinaryFormatter();
                try
                {
                    return formatter.Deserialize(ns);

                }
                catch (Exception e)
                {

                    LogConsole.Log(e.Message);

                }
                return null;

            }
        }
    }
}
