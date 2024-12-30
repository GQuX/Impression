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
using Impression.Controls;


namespace Impression.Views
{
	/// <summary>
	/// Interaction logic for EmotionEntryView.xaml
	/// </summary>
	public partial class EmotionEntryView : Window {
		private Database database = new Database();
		private Stack<int?> parent_emotion_ids_stack;
		
		public EmotionEntryView() {
            InitializeComponent();
			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
			parent_emotion_ids_stack = new Stack<int?>();
			Loaded += EmotionEntryView_Loaded;
        }

		private void EmotionEntryView_Loaded(object sender, RoutedEventArgs e) {
			LoadEmotions();
		}

		private void LoadEmotions(int? emotion_id = null) {
			if (parent_emotion_ids_stack.Count == 0 || parent_emotion_ids_stack.Peek() != emotion_id) {
				parent_emotion_ids_stack.Push(emotion_id);
			}

			var emotions = database.GetChildEmotions(emotion_id);
			if (emotions == null || emotions.Count == 0) {
				EmotionSelected(emotion_id);
				return;
			}

			EmotionStack.Children.Clear();
			foreach (var emotion in emotions) {
				EmotionButton emotion_button = new EmotionButton(emotion);
				emotion_button.Button.Click += (s, e) => LoadEmotions(emotion.Id);
				EmotionStack.Children.Add(emotion_button);
			}

			if (parent_emotion_ids_stack.Count > 1) {
				var go_back_button = new Button { Content = "Go Back" };
				go_back_button.Click += GoBackButton_Click;
				EmotionStack.Children.Add(go_back_button);
			}
		}

		private void GoBackButton_Click(object sender, RoutedEventArgs e)  {
			parent_emotion_ids_stack.Pop();
			var previous_parent_emotion_id = parent_emotion_ids_stack.Pop();
			LoadEmotions(previous_parent_emotion_id);
		}

		private void EmotionSelected(int? emotion_id) {
			if (!emotion_id.HasValue) { return; }

			Trace.WriteLine("Selected Emotion: " + emotion_id.ToString());
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
		}

		private void btn_close_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
		}

		
	}
}
