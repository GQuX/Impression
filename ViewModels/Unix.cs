using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Impression.ViewModels {
	public class Unix : BaseViewModel {
		private long _unix_timestamp;
		private string _formatted_date;

		public long UnixDate {
			get => _unix_timestamp;
			set {
				_unix_timestamp = value;
				OnPropertyChanged();
			}
		}

		public string FormattedDate {
			get => _formatted_date;
			set {
				_formatted_date = value;
				OnPropertyChanged();
			}
		}

		public Unix() {
			SetDate();
		}

		public void SetDate() {
			UnixDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(UnixDate).ToLocalTime().DateTime;
			FormattedDate = dateTime.ToString("dddd, MMMM dd, h:mm tt");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string property_name = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
		}
	}
}