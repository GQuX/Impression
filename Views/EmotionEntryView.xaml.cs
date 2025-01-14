using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;

using Impression.Controls;
using Impression.ViewModels;

namespace Impression.Views {
	/// <summary>
	/// Interaction logic for EmotionEntryView.xaml
	/// </summary>
	public partial class EmotionEntryView : UserControl {
		public EmotionEntryView() {
			InitializeComponent();
		}

		private void EmotionEntryView_Loaded(object sender, RoutedEventArgs e) {
			if (DataContext is EmotionEntryViewModel viewModel) {
				viewModel.LoadEmotionsCommand.Execute(null);
			}
		}
	}
}
