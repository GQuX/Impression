using System.Security;
using System.Windows.Input;

namespace Impression.ViewModels {
    class LoginViewModel: ViewModelBase {
        // Fields
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        // Properties
		public string Username { 
            get => _username; 
            set {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
		public SecureString Password { 
            get => _password; 
            set {
				_password = value;
				OnPropertyChanged("Password");
			}
		}

		public string ErrorMessage { 
            get => _errorMessage; 
            set {
				_errorMessage = value;
				OnPropertyChanged("ErrorMessage");
			}
		}
		public bool IsViewVisible { 
            get => _isViewVisible; 
            set {
				_isViewVisible = value;
				OnPropertyChanged("IsViewVisible");
			}
		}

        // -> Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        // Constructor
        public LoginViewModel() {
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p=>ExecuteRecoverPasswordCommand("", ""));
        }

		private bool CanExecuteLoginCommand(object obj) {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else validData = true;
            return validData;
		}

		private void ExecuteLoginCommand(object obj) {
			throw new NotImplementedException();
		}

		private void ExecuteRecoverPasswordCommand(string username, string email) {
			throw new NotImplementedException();
		}
	}
}
