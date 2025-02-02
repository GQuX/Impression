using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Impression.ViewModels {
	public class Unix : BaseViewModel {
		private long _timestamp;
		private DateTime _date_time;
		private string _formatted_date;
		
		public long Timestamp {
			get => _timestamp;
			set {
				_timestamp = value;
				OnPropertyChanged("Timestamp");
			}
		}
		public DateTime DateTime {
			get => _date_time;
			set {
				_date_time = value;
				OnPropertyChanged("DateTime");
			}
		}
		public string Formatted {
			get => _formatted_date;
			set {
				_formatted_date = value;
				OnPropertyChanged("Formatted");
			}
		}



		public string DayOfWeek() {
			return _date_time.DayOfWeek.ToString();
		}



		public Unix(long? timestamp = null) {
			Timestamp = timestamp ?? DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			DateTime = DateTimeOffset.FromUnixTimeSeconds(Timestamp).ToLocalTime().DateTime;
			Formatted = DateTime.ToString("dddd, MMMM dd, h:mm tt");
		}
	}
}