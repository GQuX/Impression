using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;

using Impression.Models;

namespace Impression.ViewModels {
    public class EntriesViewModel : MainViewModel {
        
        public EntriesViewModel() {
			Trace.WriteLine("EntriesViewModel created.");
			Trace.WriteLine( CurrentDate.Timestamp );
			Trace.WriteLine( Database.GetEntriesFromLast30Days(CurrentDate.Timestamp).Count );
		}

	}
}
