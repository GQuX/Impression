using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;

using Impression.Models;
using System;

namespace Impression.ViewModels {
    public class EntriesViewModel : MainViewModel {
		private List<Entry> entries;


		public EntriesViewModel() {
			Trace.WriteLine("EntriesViewModel created.");
			entries = Database.GetEntriesFromLast30Days(CurrentDate.Timestamp);

			foreach (Entry entry in entries) {
				Trace.WriteLine($"Day of Week: {entry.DateTime.DayOfWeek.ToString()}, Emotion: {entry.EmotionName}");
			}

		}
	}
}
