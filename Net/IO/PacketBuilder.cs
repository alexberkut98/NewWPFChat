using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    //Генератор пакетов — компьютерная программа,
    //генерирующая случайные пакеты или позволяющая пользователю сформировать
    //и отправить в компьютерную сеть произвольный пакет.
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        //WriteByte Запись байта в текущую позицию в потоке файла.
        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        //Write Записывает последовательность байтов
        //и перемещает текущую позицию внутри этого потока в памяти на число записанных байтов.
        //BitConverter Преобразует базовые типы данных в массив байтов, а массив байтов — в базовые типы данных.
        //GetBytes возвращает указаное число в виде массива байтов.
        public void WriteString(string msg)
        {
            var msglength = msg.Length;
            _ms.Write(BitConverter.GetBytes(msglength));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }
        public byte[] GetPacetBytes()
        {
            return _ms.ToArray();
        }
    }
}
