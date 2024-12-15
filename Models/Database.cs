using Microsoft.Data.Sqlite;

namespace Impression.Models {
    public class Database {
        private string connection_string = "Data Source = LocalData.db";

		// Get a list of all Emotions
		public List<Emotion> GetEmotions() {
			var emotions = new List<Emotion>();

			using (var connection = new SqliteConnection(connection_string)) {
				connection.Open();
				var command = new SqliteCommand("SELECT * FROM emotions", connection);
				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						emotions.Add(new Emotion {
							Id = reader.GetInt16(0),
							Name = reader.GetString(1),
							Level = reader.GetInt16(2),
							Color = reader.GetString(3),
							ParentId = reader.GetInt16(4)
						});
					}
				}
			}

			return emotions;
		}

		// Get a list of all Entries
		public List<Entry> GetEntries() {
			var entires = new List<Entry>();

			using (var connection = new SqliteConnection(connection_string)) {
				connection.Open();
				var command = new SqliteCommand("SELECT * FROM entries", connection);
				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						entires.Add(new Entry {
							Id = reader.GetInt16(0),
							EmotionId = reader.GetInt16(1),
							Timestamp = reader.GetInt32(2)
						});
					}
				}
			}

			return entires;
		}

		// Insert an Entry
		public void AddEntry(Entry entry) {
			using (var connection = new SqliteConnection(connection_string)) {
				connection.Open();
				var command = new SqliteCommand("INSERT INTO entries (id, emotion_id, timestamp)" +
					"VALUES (@Id, @EmotionId, @Timestamp)", connection);
				command.Parameters.AddWithValue("@Id", entry.Id);
				command.Parameters.AddWithValue("@EmotionId", entry.EmotionId);
				command.Parameters.AddWithValue("@Timestamp", entry.Timestamp);
				command.ExecuteNonQuery();
			}
		}
	}
}
