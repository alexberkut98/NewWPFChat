using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        }

        private void Sending()
        {
            if(!string.IsNullOrEmpty(Message))
            {

            }
        }

        //Здесь текст сообщения считывается и добавляется в список всех сообщений
        private void MessageReceived()
        {

        }

        private void UserConnected()
        {
            
            var contact = new ContactModel()
            {

            };

            //Нужно сделать так, чтобы у каждого пользователя в списке контактов не выводился он сам.
            //Нужно добыть имя пользовате

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
