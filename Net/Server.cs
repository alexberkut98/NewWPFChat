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
        public PacketReader packetReader;
        public string User = " ";
        public Server()
        {
            _client = new TcpClient();
        }

        public event Action ConnectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectEvent;

        public void ConnectToServer(string username)
        {
            if(!_client.Connected)
            {
                //В дальнейшем я налажу возможность ввода данных параметров через клавиатуру,
                //Пока что ограничимся этим, для простоты тестирования.
                _client.Connect("127.0.0.1", 7891);
                packetReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    User = username;
                    var ConnectPacket = new PacketBuilder();
                    ConnectPacket.WriteOpCode(0);
                    ConnectPacket.WriteMessage(username);
                    _client.Client.Send(ConnectPacket.GetPacetBytes());
                }
                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while(true)
                {
                    var opcode = packetReader.ReadByte();
                    switch(opcode)
                    {
                        case 1:
                            ConnectedEvent?.Invoke();
                            break;

                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;

                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine("Ah... yes");
                            break;
                    }
                }
            }
            );
        }

        public string GetUser()
        {
            return User;
        }

        //Сообщение отправляется на сервер
        public void SendMessageToServer(string message)
        {
            var MessagePacket = new PacketBuilder();
            MessagePacket.WriteOpCode(5);
            MessagePacket.WriteMessage(message);
            _client.Client.Send(MessagePacket.GetPacetBytes());
        }
    }
}
