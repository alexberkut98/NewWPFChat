using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Вторая_попытка_в_чат.Core;
using Вторая_попытка_в_чат.MVVM.Model;
using Вторая_попытка_в_чат.Net;

namespace Вторая_попытка_в_чат.MVVM.ViewModel
{
    class MainViewModel: ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public RelayCommand SendCommand { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public ContactModel SelectedContact { 
            get
            { 
                return _selectedContact; 
            }
            set
            { 
                _selectedContact = value; 
                OnPropertyChanged();
            }
        }
        private ContactModel _selectedContact;
        private Server _server;
        private string _message;

        public string UserName { get; set; }
        public string Message 
        {
            get { return _message; } 
            set 
            { 
                _message = value;
                OnPropertyChanged();
            } 
        }
        public MainViewModel()
        {
            _server = new Server();
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(UserName),o=>!string.IsNullOrEmpty(UserName));

            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();

            SendCommand = new RelayCommand(o =>
            {
                Messages.Add
                (
                    new MessageModel
                    {
                        Message = Message, FirstMessage = false
                    });
                Message = "";
            }
            );

            Messages.Add(new MessageModel
            {
                UserName = "Alexander",
                UserNameColor = "White",
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
