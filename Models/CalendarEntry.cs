using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Impression.Models;

namespace Impression.Models {
	public class CalendarEntry {
		public required Entry Entry { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }
	}
}
