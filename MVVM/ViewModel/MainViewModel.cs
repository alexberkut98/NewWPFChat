using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Вторая_попытка_в_чат.Core;
using Вторая_попытка_в_чат.MVVM.Model;
using Вторая_попытка_в_чат.Net;

namespace Вторая_попытка_в_чат.MVVM.ViewModel
{
    class MainViewModel: ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public RelayCommand SendCommand { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }

        public ObservableCollection<string> Messages1 { get; set; }
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
            _server.ConnectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(UserName),o=>!string.IsNullOrEmpty(UserName));

            Users = new ObservableCollection<UserModel>();
            Messages1 = new ObservableCollection<string>();

            SendCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        private void RemoveUser()
        {
            var uid = _server.packetReader.ReadMessage();
            var user = Users.Where(x=>x.UID==uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void MessageReceived()
        {
            var msg = _server.packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages1.Add(msg));
        }

        private void UserConnected()
        {
            var user = new UserModel()
            {
                UserName = _server.packetReader.ReadMessage(),
                UID = _server.packetReader.ReadMessage()
            };
            if(!Users.Any(x=>x.UID==user.UID))
            {
                Application.Current.Dispatcher.Invoke(()=>Users.Add(user));
            }
        }
    }
}
