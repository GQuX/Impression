using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Impression.Models;
using Impression.Views;

namespace Impression.Controls {
	/// <summary>
	/// Interaction logic for EmotionButton.xaml
	/// </summary>
	public partial class EmotionButton : UserControl {
		public Emotion Emotion { get; set; }

		public EmotionButton(Emotion emotion) {
			InitializeComponent();
			Emotion = emotion;
			DataContext = this;
		}
	}
}
