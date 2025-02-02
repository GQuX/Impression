using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System;

using Impression.Models;

namespace Impression.ViewModels {
    public class EntriesViewModel : MainViewModel {
		private List<Entry> entries;
		public ICommand ExportCommand { get; }

		private void ExportEntries() {
			try {
				string downloads_folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
				string file_path = Path.Combine(downloads_folder, "Entries.csv");

				using (StreamWriter writer = new(file_path)) {
					foreach (Entry entry in entries) {
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

		public EntriesViewModel() {
			Trace.WriteLine("EntriesViewModel created.");
			entries = Database.GetEntriesFromLast30Days(CurrentDate.Timestamp);

			foreach (Entry entry in entries) {
				Trace.WriteLine($"Day of Week: {entry.DateTime.DayOfWeek.ToString()}, Emotion: {entry.EmotionName}");
			}
			ExportCommand = new RelayCommand(ExportEntries);

		}
	}
}
