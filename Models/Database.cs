using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace Impression.Models {
    public class Database {
        private string connection_string = new SQLiteConnectionStringBuilder() {
			DataSource = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\LocalData.db")
		}.ConnectionString;

		// Cache of Emotion colors
		private static Dictionary<int, string> color_cache = new();

		// Get the color of an Emotion (recursive)
		private static string GetEmotionColor(SQLiteConnection connection, int? emotion_id) {
			if (emotion_id == null) { return "#E6E6FA"; }
			if (color_cache.TryGetValue(emotion_id.Value, out var cachedColor)) {
				return cachedColor;
			}

			Trace.WriteLine("Database.GetEmotionColor(" + emotion_id.ToString() + ")");

			var command = new SQLiteCommand($"SELECT color, parent_id FROM emotions WHERE id = {emotion_id}", connection);

			using (var reader = command.ExecuteReader()) {
				if (reader.Read()) {
					var color = reader.IsDBNull(0) ? null : reader.GetString(0);
					var parent_id = reader.IsDBNull(1) ? (int?)null : reader.GetInt16(1);

					if (!string.IsNullOrEmpty(color)) {
						color_cache[emotion_id.Value] = color;
						return color;
					}

					var parent_color = GetEmotionColor(connection, parent_id);
					return parent_color;
				}
			}

			return "#E6E6FA";
		}

		// Get a list of all Emotions under another
		public List<Emotion> GetChildEmotions(int? emotion_id) {
			Trace.WriteLine("Database.GetChildEmotions("+ emotion_id.ToString() + ")");
			var emotions = new List<Emotion>();

			using (var connection = new SQLiteConnection(connection_string)) {
				connection.Open();

				var query = emotion_id.HasValue ?
					"SELECT * FROM emotions WHERE parent_id = @emotion_id" :
					"SELECT * FROM emotions WHERE parent_id IS NULL";

				var command = new SQLiteCommand(query, connection);
				command.Parameters.AddWithValue("@emotion_id", emotion_id);

				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						var emotion = new Emotion {
							Id = reader.GetInt16(0),
							Name = reader.GetString(1),
							Level = reader.GetInt16(2),
							Color = reader.IsDBNull(3) ? null : reader.GetString(3),
							ParentId = reader.IsDBNull(4) ? (int?)null : reader.GetInt16(4),
						};

						if (string.IsNullOrEmpty(emotion.Color)) {
							emotion.Color = GetEmotionColor(connection, emotion.ParentId);
						} else {
							color_cache[emotion.Id] = emotion.Color;
						}

						emotions.Add(emotion);
					}
				}
			}

			return emotions;
		}

		// Get a list of all Entries
		public List<Entry> GetEntriesFromLast30Days(long current_time) {
			Trace.WriteLine("Database.GetEntriesFromLast30Days(" + current_time.ToString() + ")");
			long thirty_days_ago = current_time - (29 * 24 * 60 * 60);
			var entries = new List<Entry>();

			using (var connection = new SQLiteConnection(connection_string)) {
				connection.Open();

				var query = @"
					SELECT entry.id, entry.timestamp, entry.emotion_id, emotion.name, emotion.color
					FROM entries AS entry
					JOIN emotions AS emotion
						ON entry.emotion_id = emotion.id
					WHERE entry.timestamp >= @thirty_days_ago
					ORDER BY entry.timestamp ASC";

				var command = new SQLiteCommand(query, connection);
				command.Parameters.AddWithValue("@thirty_days_ago", thirty_days_ago);

				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						var entry = new Entry {
							Id			= Convert.ToInt16(reader.GetInt64(0)),
							Timestamp = Convert.ToInt32(reader.GetInt64(1)),
							EmotionId	= Convert.ToInt16(reader.GetInt64(2)),
							EmotionName	= reader.GetString(3),
							EmotionColor = reader.IsDBNull(4) ? null : reader.GetString(4)
						};

						entry.DateTime = DateTimeOffset.FromUnixTimeSeconds(entry.Timestamp).DateTime;
						if (string.IsNullOrEmpty(entry.EmotionColor)) {
							entry.EmotionColor = GetEmotionColor(connection, entry.EmotionId);
						}

						entries.Add(entry);
					}
				}
			}

			return entries;
		}

		// Insert an Entry
		public void AddEntry(Entry entry) {
			using (var connection = new SQLiteConnection(connection_string)) {
				connection.Open();

				var command = new SQLiteCommand("INSERT INTO entries (id, emotion_id, timestamp)" +
					"VALUES (@Id, @EmotionId, @Timestamp)", connection);
				command.Parameters.AddWithValue("@Id", entry.Id);
				command.Parameters.AddWithValue("@EmotionId", entry.EmotionId);
				command.Parameters.AddWithValue("@Timestamp", entry.Timestamp);

				command.ExecuteNonQuery();
			}
		}
	}
}
