using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System;

using Impression.Models;

namespace Impression.ViewModels {
    public class EntriesViewModel : MainViewModel {
		private ObservableCollection<CalendarEntry> _calendar_entries;
		public ICommand ExportCommand { get; }

		public ObservableCollection<CalendarEntry> CalendarEntries {
			get { return _calendar_entries; }
			set {
				_calendar_entries = value;
				OnPropertyChanged(nameof(CalendarEntries));
			}
		}

		private void ExportEntries() {
			try {
				string downloads_folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
				string file_path = Path.Combine(downloads_folder, "Entries.csv");

				using (StreamWriter writer = new(file_path)) {
					foreach (Entry entry in CalendarEntries.Select(ce => ce.Entry)) {
						string date = $"\"{entry.DateTime:yyyy.MM.dd}\"";
						string time = $"\"{entry.DateTime:h:mm tt}\"";

						writer.WriteLine($"{date},{time},{entry.EmotionName}");
					}
				}

				Trace.WriteLine($"File exported to: {file_path}");
			} catch (Exception err) {
				Trace.WriteLine($"Error exporting entries: {err.Message}");
			}
		}

		int total_weeks = 5;
		public EntriesViewModel() {
			Trace.WriteLine("EntriesViewModel created.");
			var entry_list = Database.GetEntriesFromLast30Days(CurrentDate.Timestamp);
			_calendar_entries = new ObservableCollection<CalendarEntry>();

			foreach (Entry entry in entry_list) {
				int days = (CurrentDate.DateTime.Date - entry.DateTime.Date).Days;
				int row = total_weeks - (days / 7);
				int column = (((int)CurrentDate.DateTime.DayOfWeek - days) % 7 + 7) % 7;

				Trace.WriteLine(entry.EmotionName);
				Trace.WriteLine($"Row: {row}");
				Trace.WriteLine($"Column: {column}");
				Trace.WriteLine($"{entry.DateTime.DayOfWeek.ToString()}, {entry.DateTime:MM.dd}");

				var calendar_entry = new CalendarEntry {
					Entry	= entry,
					Row		= row,
					Column	= column
				};
				_calendar_entries.Add(calendar_entry);

			}
			ExportCommand = new RelayCommand(ExportEntries);
		}
	}
}
