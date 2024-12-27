using Impression.Models;
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


namespace Impression.Views
{
	/// <summary>
	/// Interaction logic for EmotionEntryView.xaml
	/// </summary>
	public partial class EmotionEntryView : Window {
		private Database _database = new Database();

		public EmotionEntryView() {
            InitializeComponent();
			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr window, int message, int w_param, int l_param);

		private void topbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			WindowInteropHelper helper = new WindowInteropHelper(this);
			SendMessage(helper.Handle, 161, 2, 0);
        }

		private void topbar_MouseEnter(object sender, MouseEventArgs e) {
			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
		}

		private void btn_minimize_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;

            Trace.WriteLine(_database.GetEmotions()[0]);
            //_database.PrintAllTableNames();
		}

		private void btn_close_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
		}

		
	}
}
