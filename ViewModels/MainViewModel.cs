using Impression.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Impression.Models;

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


		private readonly Unix _unix_view_model;
		public Unix CurrentDate => _unix_view_model;


		private readonly Database database = new();
		public Database Database => database;


		public MainViewModel() {
			UpdateViewCommand = new UpdateViewCommand(this);
			_unix_view_model = new Unix();
		}
	}
}
