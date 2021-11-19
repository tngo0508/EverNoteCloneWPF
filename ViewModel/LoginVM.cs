using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EverNoteCloneWPF.Model;
using EverNoteCloneWPF.ViewModel.Commands;

namespace EverNoteCloneWPF.ViewModel
{
    public class LoginVM
    {
        private User _user;

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        public DelegateCommand<object> RegisterCommand { get; set; }
        public DelegateCommand<object> LoginCommand { get; set; }

        public LoginVM()
        {
            RegisterCommand = new DelegateCommand<object>(Register, null);
            LoginCommand = new DelegateCommand<object>(Login, null);

        }

        private void Login(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Register(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
