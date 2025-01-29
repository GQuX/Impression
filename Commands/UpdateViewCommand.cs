using Impression.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Impression.Commands {
	public class UpdateViewCommand : ICommand {
		private MainViewModel viewModel;

		public UpdateViewCommand(MainViewModel viewModel) => this.viewModel = viewModel;

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter) {
			if (parameter != null) {
				string viewName = parameter.ToString();
				switch (viewName) {
					case "EmotionEntry":
						viewModel.SelectedViewModel = new EmotionEntryViewModel();
						break;
					case "Entries":
						viewModel.SelectedViewModel = new EntriesViewModel();
						break;
					// Add cases for other views as needed
					default:
						break;
				}
			}
		}
	}
}
