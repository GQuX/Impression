using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Impression.ViewModels {
	public class Unix : BaseViewModel {
		private long _unix_timestamp;
		private string _formatted_date;

		public long Timestamp {
			get => _unix_timestamp;
			set {
				_unix_timestamp = value;
				OnPropertyChanged();
			}
		}

		public string Formatted {
			get => _formatted_date;
			set {
				_formatted_date = value;
				OnPropertyChanged();
			}
		}

		public Unix() {
			Set();
		}

		public void Set() {
			Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(Timestamp).ToLocalTime().DateTime;
			Formatted = dateTime.ToString("dddd, MMMM dd, h:mm tt");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string property_name = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
		}
	}
}