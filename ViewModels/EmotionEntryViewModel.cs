using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;

using Impression.Models;
using System.Runtime.CompilerServices;

namespace Impression.ViewModels {
    public class EmotionEntryViewModel : ViewModelBase {
        private readonly Database _database = new();

		private Stack<int?> _parent_emotion_ids_stack;
		public ObservableCollection<Emotion> Emotions { get; set; } = [];

		private readonly Unix _unix_view_model;
		public Unix UnixViewModel => _unix_view_model;

		public EmotionEntryViewModel() {
            _parent_emotion_ids_stack = new Stack<int?>();
            LoadEmotionsCommand = new RelayCommand<int?>(LoadEmotions);
            GoBackCommand = new RelayCommand(GoBack);
            _unix_view_model = new Unix();
        }

		private void SelectEmotion(int? emotion_id) {
			if (!emotion_id.HasValue) return;
            // Handle selected emotion logic
            Trace.WriteLine(emotion_id + " Selected.");

            var new_entry = new Entry();
			new_entry.EmotionId = emotion_id.Value;
			new_entry.Timestamp = _unix_view_model.UnixDate;
            _database.AddEntry(new_entry);
		}

		public ICommand LoadEmotionsCommand { get; }
		private void LoadEmotions(int? emotion_id) {
            if (_parent_emotion_ids_stack.Count == 0 || _parent_emotion_ids_stack.Peek() != emotion_id) {
                _parent_emotion_ids_stack.Push(emotion_id);
            }

            var emotions = _database.GetChildEmotions(emotion_id);
            if (emotions == null || emotions.Count == 0)  {
                SelectEmotion(emotion_id);
                return;
            }

            Emotions.Clear();
            foreach (var emotion in emotions)  {
                Emotions.Add(emotion);
            }

            UpdateGoBackVisibility();
        }

		public ICommand GoBackCommand { get; }
		private void GoBack() {
            _parent_emotion_ids_stack.Pop();
            var previous_parent_emotion_id = _parent_emotion_ids_stack.Pop();
            LoadEmotions(previous_parent_emotion_id);
        }

        private bool _can_go_back;
        public bool CanGoBack {
            get => _can_go_back;
            set {
                _can_go_back = value;
                OnPropertyChanged(nameof(CanGoBack));
            }
        }

        private void UpdateGoBackVisibility() {
            CanGoBack = _parent_emotion_ids_stack.Count > 1;
        }
	}
}
