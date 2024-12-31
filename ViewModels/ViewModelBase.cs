using Impression.Models;
using Impression.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Impression.ViewModels {
	public abstract class ViewModelBase : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string property_name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
		}
	}
}