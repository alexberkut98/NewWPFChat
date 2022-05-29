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

        public ContactModel SelectedContact 
        { 
            get
            { 
                return _selectedContact; 
            }
            set
            { 
                _selectedContact = value;
                ChangeColor();
                OnPropertyChanged();
            }
        }
        private string _recipient;
        private ContactModel _selectedContact;
        private Server _server;
        private string _message;

        //Здесь будет храниться название текущего чата
        //All - общий чат.
        //UserName - чья та личка
        public string Recipient 
        { 
            get
            {
                return _recipient;
            }
            set
            {
                value = _recipient;
            }
        }
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
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();
            ContactModel current = new ContactModel();
            current.UserName = "All";
            current.UserColor = "Red";
            SelectedContact = current;
            Contacts.Add(current);
            OnPropertyChanged();
            SendCommand = new RelayCommand(o => Sending());
        }

        private void RemoveUser()
        {
            var uid = _server.packetReader.ReadMessage();
            var contact = Contacts.Where(x=>x.UID==uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Contacts.Remove(contact));
        }

        private void Sending()
        {
            if(!string.IsNullOrEmpty(Message))
            {
                _server.SendMessageToServer(Message);
                MessageReceived();
            }
        }

        //Здесь текст сообщения считывается и добавляется в список всех сообщений
        private void MessageReceived()
        {
            var msg = _server.packetReader.ReadMessage();
            MessageModel current = new MessageModel();
            current.Message = msg;
            current.Time = DateTime.Now;
            current.UserName = UserName;
            current.UserNameColor = "White";
            Application.Current.Dispatcher.Invoke(() => Messages.Add(current));
        }

        private void UserConnected()
        {
            
            var contact = new ContactModel()
            {
                UserName = _server.packetReader.ReadMessage(),
                UID = _server.packetReader.ReadMessage(),
                UserColor = "Coral"
            };

            //Нужно сделать так, чтобы у каждого пользователя в списке контактов не выводился он сам.
            //Нужно добыть имя пользовате
            if(!Contacts.Any(x=>x.UserName==contact.UserName)&&contact.UserName!=_server.User)
            {
                OnPropertyChanged();
                Application.Current.Dispatcher.Invoke(()=>Contacts.Add(contact));
            }
        }

        //Меняем элемент, чей задний фон будет красным.
        //Эта функция должна применяться всякий раз,
        //как переменная SelectedContact сменит сове значение
        private void ChangeColor()
        {
            for(int i=0;i<Contacts.Count;i++)
            {
                if (Contacts[i].UserName == SelectedContact.UserName)
                    Contacts[i].UserColor = "Red";
                else
                    Contacts[i].UserColor = "Coral";
            }
            OnPropertyChanged();
        }
    }
}
