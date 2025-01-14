using Impression.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Impression.ViewModels {
	public class MainViewModel : BaseViewModel {
		private BaseViewModel _selected_view_model;

		public BaseViewModel SelectedViewModel {
			get { return _selected_view_model; }
			set { 
				_selected_view_model = value;
				OnPropertyChanged(nameof(SelectedViewModel));
			}
		}

		public ICommand UpdateViewCommand { get; set; }

		public MainViewModel() {
			UpdateViewCommand = new UpdateViewCommand(this);
		}
	}
}
