using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer
{
    class ConnUtility
    {
        public static bool TestClient(TcpClient conn)
        {
            if (conn == null)
                return false;
            try
            {
                return !(conn.Client.Poll(50, SelectMode.SelectRead) && conn.Client.Available == 0);
            }
            catch (SocketException) { return false; }


        }

        public static String GetIP(TcpClient conn)
        {
            return IPAddress.Parse(((IPEndPoint)conn.Client.RemoteEndPoint).Address.ToString()).ToString();
        }

        public static void SerializeAndSend(NetworkStream ns, object o)
        {
            if(o.GetType().IsSerializable)
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
