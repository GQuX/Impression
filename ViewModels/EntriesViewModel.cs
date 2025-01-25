using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;

using Impression.Models;
using System.Runtime.CompilerServices;

namespace Impression.ViewModels {
    public class EntriesViewModel : BaseViewModel {
        private readonly Unix _unix_view_model;
		public Unix UnixViewModel => _unix_view_model;

		public EntriesViewModel() {
            _unix_view_model = new Unix();
        }
	}
}
