using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System;

using Impression.Models;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Impression.ViewModels {
    public class EntriesViewModel : MainViewModel {
		private ObservableCollection<CalendarEntry> _calendar_entries;
		private Dictionary<int, List<Entry>> _entries_by_day;
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
					foreach (var kvp in _entries_by_day.OrderBy(k => k.Key)) {
						foreach (Entry entry in kvp.Value) {
							string date = $"\"{entry.DateTime:yyyy.MM.dd}\"";
							string time = $"\"{entry.DateTime:h:mm tt}\"";
							writer.WriteLine($"{date},{time},{entry.EmotionName}");
						}
					}
				}

				Trace.WriteLine($"File exported to: {file_path}");
			} catch (Exception err) {
				Trace.WriteLine($"Error exporting entries: {err.Message}");
			}
		}

		public static Brush TextContrast(string hex_color) {
			Color rgb_color = (Color)ColorConverter.ConvertFromString(hex_color);

			double luminance = (0.299 * rgb_color.R + 0.587 * rgb_color.G + 0.114 * rgb_color.B) / 255;

			return luminance > 0.5 ? Brushes.Black : Brushes.White;
		}

		private string AverageColor(IEnumerable<Entry> entries) {
			if (!entries.Any()) return "#FFFFFF";

			int r = 0, g = 0, b = 0, count = 0;

			foreach (var entry in entries) {
				if (ColorConverter.ConvertFromString(entry.EmotionColor) is Color color) {
					r += color.R;
					g += color.G;
					b += color.B;
					count++;
				}
			}

			if (count == 0) return "#FFFFFF"; // Avoid division by zero

			r /= count;
			g /= count;
			b /= count;

			return $"#{r:X2}{g:X2}{b:X2}"; // Convert to hex
		}


		int total_weeks = 5;
		public EntriesViewModel() {
			Trace.WriteLine("EntriesViewModel created.");
			var entry_list = Database.GetEntriesFromLast30Days(CurrentDate.Timestamp);
			_calendar_entries = new ObservableCollection<CalendarEntry>();
			_entries_by_day = new Dictionary<int, List<Entry>>();

			foreach (Entry entry in entry_list) {
				int days = (CurrentDate.DateTime.Date - entry.DateTime.Date).Days;
				
				if (!_entries_by_day.ContainsKey(days))
					_entries_by_day[days] = new List<Entry>();
				
				_entries_by_day[days].Add(entry);

				Trace.WriteLine(entry.EmotionName + days.ToString());
			}

			int current_day_column = (int)CurrentDate.DateTime.DayOfWeek;
			
			foreach (var kvp in _entries_by_day) {
				string avg_color = AverageColor(kvp.Value);
				string contrast_color = TextContrast(avg_color).ToString();
				string stacked_emotions = string.Join("\n", kvp.Value.Select(e => e.EmotionName));

				int row		= total_weeks - (kvp.Key / 7);
				int column	= (current_day_column - (kvp.Key % 7) + 7) % 7;
				if (column > current_day_column) row--;
				row = Math.Max(0, row);

				//Trace.WriteLine($"Day: {kvp.Key} | {row}, {column}");
				//Trace.WriteLine(stacked_emotions);

				var calendar_entry = new CalendarEntry {
					Row		= row,
					Column	= column,
					AverageColor = avg_color,
					ContrastColor = contrast_color,
					Emotions = stacked_emotions
				};

				_calendar_entries.Add(calendar_entry);
			}

			ExportCommand = new RelayCommand(ExportEntries);
		}
	}
}
