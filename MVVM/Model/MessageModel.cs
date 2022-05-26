using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Вторая_попытка_в_чат.MVVM.Model
{
    class MessageModel
    {
        public string UserName { get; set; }
        public string UserNameColor { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public bool IsNativeOrigin { get; set; }
        public bool FirstMessage { get; set; }
    }
}
