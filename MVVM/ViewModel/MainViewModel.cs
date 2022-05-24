using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Вторая_попытка_в_чат.MVVM.Model;

namespace Вторая_попытка_в_чат.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            Messages.Add(new MessageModel
            {
                UserName = "Alexander",
                Message = "Text",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true
            }

                );
            
            for(int i = 0; i < 5; i++)
            {
                Contacts.Add(new ContactModel
                {
                    UserName = $"Sasha{i}",
                    Messages = Messages
                }
                    );
            }
        }
    }
}
