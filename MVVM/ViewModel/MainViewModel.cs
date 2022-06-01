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

namespace Вторая_попытка_в_чат.MVVM.ViewModel
{
    class MainViewModel: ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public RelayCommand SendCommand { get; set; }
        //public RelayCommand ConnectToServerCommand { get; set; }

        //Выбор режима работы сервера
        public RelayCommand ChooseMode1 { get; set; }
        public RelayCommand ChooseMode2 { get; set; }
        public RelayCommand OK { get; set; }

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
        int ChosenMode = 1;
        public MainViewModel()
        {
            Mini1 = new RelayCommand(o => Minimise());
            ChangeStation = new RelayCommand(o => WindowStateClick());
            Closing = new RelayCommand(o => CloseWindow());

            ChooseMode1 = new RelayCommand(o => Choose1());
            ChooseMode2 = new RelayCommand(o => Choose2());
            OK = new RelayCommand(o => OK1());

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

        //Запуск выбранного режима
        public void OK1()
        {

        }

        private void Choose1()
        {
            ChosenMode = 1;
        }
        public void Choose2()
        {
            ChosenMode = 2;
        }
        //Здесь текст сообщения считывается и добавляется в список всех сообщений
        private void MessageReceived()
        {

        }
        private void UserConnected()
        {

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
