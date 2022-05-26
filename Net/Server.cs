using ChatClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Вторая_попытка_в_чат.Net.IO;

namespace Вторая_попытка_в_чат.Net
{
    class Server
    {
        //Для создания клиентской программы, работающей по протоколу TCP, предназначен класс TcpClient.
        TcpClient _client;
        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if(!_client.Connected)
            {
                //В дальнейшем я налажу возможность ввода данных параметров через клавиатуру,
                //Пока что ограничимся этим, для простоты тестирования.
                _client.Connect("127.0.0.1", 7891);
                var ConnectPacket = new PacketBuilder();
                ConnectPacket.WriteOpCode(0);
                ConnectPacket.WriteString(username);
                _client.Client.Send(ConnectPacket.GetPacetBytes());
            }
        }
    }
}
