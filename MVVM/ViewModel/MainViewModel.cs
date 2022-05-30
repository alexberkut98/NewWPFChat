using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Вторая_попытка_в_чат.Core;
using Вторая_попытка_в_чат.MVVM.Model;
using Вторая_попытка_в_чат.MVVM.View;
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

        public RelayCommand MouseDown1 { get; set; }
        public RelayCommand Mini1 { get; set; }
        public RelayCommand ChangeStation { get; set; }
        public RelayCommand Closing { get; set; }

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
        private ContactModel _selectedContact;
        private Server _server;
        private string _message;

        //Здесь будет храниться название текущего чата
        //All - общий чат.
        //UserName - чья та личка

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
        string UN="";
        public MainViewModel()
        {
            _server = new Server();
            _server.ConnectedEvent += UserConnected;
            _server.msgReceivedEvent += MessageReceived;
            _server.userDisconnectEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(UserName), o => !string.IsNullOrEmpty(UserName));
            MouseDown1 = new RelayCommand(o => MouseDown3());
            Mini1 = new RelayCommand(o => Minimise());
            ChangeStation = new RelayCommand(o => WindowStateClick());
            Closing = new RelayCommand(o => CloseWindow());

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
            Contacts.Add(new ContactModel
            {
                UserName = "Said",
                UserColor = "Coral",
                Messages = new ObservableCollection<MessageModel>()
            });
            Contacts.Add(new ContactModel
            {
                UserName = "Murat",
                UserColor = "Coral",
                Messages = new ObservableCollection<MessageModel>()
            });
            Messages.Add(
                new MessageModel
                {
                    UserName = "Said",
                    Vis = "0",
                    Target = "You",
                    Time = DateTime.Now,
                    Message = "Hello, User!"
                }
                );
            Messages.Add(
    new MessageModel
    {
        UserName = "Said",
        Vis = "1",
        Target = "All",
        Time = DateTime.Now,
        Message = "Long time no see!"
    }
    );
            Messages.Add(
    new MessageModel
    {
        UserName = "Murat",
        Vis = "0",
        Target = "You",
        Time = DateTime.Now,
        Message = "Hello, friend!!"
    }
    );
            Messages.Add(
    new MessageModel
    {
        UserName = "Murat",
        Vis = "1",
        Target = "All",
        Time = DateTime.Now,
        Message = "I need your help!!"
    }
    );
        }
        public void AddMessage()
        {
      
        }
        private void RemoveUser()
        {
            var uid = _server.packetReader.ReadMessage();
            var contact = Contacts.Where(x=>x.UID==uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Contacts.Remove(contact));
        }

        //Мало отправить сообщение на сервер, его еще надо отправить конкретному пользователю.
        //Даже если имя адресата "All"
        private void Sending()
        {
            if(!string.IsNullOrEmpty(Message))
            {
                Messages.Add(
                    new MessageModel
                    {
                        UserName = "You",
                        Vis = "1",
                        Target = SelectedContact.UserName,
                        Time = DateTime.Now,
                        Message = Message
                    }
                    );
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

            for(int i=0;i<Messages.Count;i++)
            {
                if (Messages[i].UserName == SelectedContact.UserName && Messages[i].Target=="You" ||SelectedContact.UserName=="All" && Messages[i].Target == "All" || Messages[i].UserName == "You" && Messages[i].Target==SelectedContact.UserName)
                    Messages[i].Vis = "1";
                else
                    Messages[i].Vis = "0";
            }
            OnPropertyChanged();
        }

        public void MouseDown3()
        {
            Wwork w = new Wwork();
            w.MouseDown4();
        }
        public void Minimise()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public void WindowStateClick()
        {
            //Если окно находится не в максимальном расширении - увеличить его.
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
         public void CloseWindow()
        {
            Application.Current.Shutdown();
        }
    }
}
