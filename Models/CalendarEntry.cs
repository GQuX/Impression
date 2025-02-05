using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Impression.Models;

namespace Impression.Models {
	public class CalendarEntry {
		public int Row { get; set; }
		public int Column { get; set; }
		public required string AverageColor { get; set; }
		public required string ContrastColor { get; set; }
		public required string Emotions { get; set; }
	}
}
