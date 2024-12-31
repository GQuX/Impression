using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Impression.ViewModels {
	public class Unix : ViewModelBase {
		private long unix_timestamp;
		private string formatted_date;

		public long UnixDate {
			get => unix_timestamp;
			set {
				unix_timestamp = value;
				OnPropertyChanged();
			}
		}

		public string FormattedDate {
			get => formatted_date;
			set {
				formatted_date = value;
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

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}