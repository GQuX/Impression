﻿using System.Data.SQLite;
using System.Diagnostics;

namespace Impression.Models {
    public class Database {
        private string connection_string = new SQLiteConnectionStringBuilder() {
			DataSource = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\LocalData.db")
		}.ConnectionString;
		
		// Test Database connection
		public void PrintAllTableNames() {
			Trace.WriteLine("Database.PrintAllTableNames()");
			using (var connection = new SQLiteConnection(connection_string)) {
				connection.Open();

				var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table';", connection);

				using (var reader = command.ExecuteReader()) {
					Trace.WriteLine("Tables in database:");
					while (reader.Read()) {
						Trace.WriteLine(reader.GetString(0));
					}
				}
			}
		}

		// Get the color of an Emotion (recursive)
		private static string GetEmotionColor(SQLiteConnection connection, int? emotion_id) {
			Trace.WriteLine("Database.GetEmotionColor()");
			if (emotion_id == null) { return "#E6E6FA"; }
						
			var command = new SQLiteCommand($"SELECT color, parent_id FROM emotions WHERE id = {emotion_id}", connection);

			using (var reader = command.ExecuteReader()) {
				if (reader.Read()) {
					var color = reader.IsDBNull(0) ? null : reader.GetString(0);
					var parent_id = reader.IsDBNull(1) ? (int?)null : reader.GetInt16(1);

					if (!string.IsNullOrEmpty(color)) {
						return color;
					}

					return GetEmotionColor(connection, parent_id);
				}
			}

			return "#E6E6FA";
		}

		// Get a list of all Emotions
		public List<Emotion> GetEmotions() {
			Trace.WriteLine("Database.GetEmotions()");
			var color_cache = new Dictionary<int, string>();
			var emotions = new List<Emotion>();

			using (var connection = new SQLiteConnection(connection_string)) {
				connection.Open();

				var command = new SQLiteCommand("SELECT * FROM emotions", connection);

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
							if (!color_cache.TryGetValue(emotion.ParentId.Value, out var color)) {
								color = GetEmotionColor(connection, emotion.ParentId);
								color_cache[emotion.ParentId.Value] = color;
							}

							emotion.Color = GetEmotionColor(connection, emotion.ParentId);
						}

						emotions.Add(emotion);
					}
				}
			}

			return emotions;
		}

		// Get a list of all Emotions under another
		public List<Emotion> GetChildEmotions(int? emotion_id) {
			Trace.WriteLine("Database.GetChildEmotions("+ emotion_id.ToString() + ")");
			var color_cache = new Dictionary<int, string>();
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
							if (!color_cache.TryGetValue(emotion.ParentId.Value, out var color)) {
								color = GetEmotionColor(connection, emotion.ParentId);
								color_cache[emotion.ParentId.Value] = color;
							}

							emotion.Color = color;
						}

						emotions.Add(emotion);
					}
				}
			}

			return emotions;
		}

		// Get a list of all Entries
		public List<Entry> GetEntries() {
			Trace.WriteLine("Database.GetEntries()");
			var entires = new List<Entry>();

			using (var connection = new SQLiteConnection(connection_string)) {
				connection.Open();

				var command = new SQLiteCommand("SELECT * FROM entries", connection);

				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						entires.Add(new Entry {
							Id = Convert.ToInt16( reader.GetInt64(0) ),
							EmotionId = Convert.ToInt16( reader.GetInt64(1) ),
							Timestamp = Convert.ToInt32( reader.GetInt64(2) )
						});
					}
				}
			}

			return entires;
		}

		// Get a list of all Entries
		public List<Entry> GetEntriesFromLast30Days(long current_time) {
			Trace.WriteLine("Database.GetEntriesFromLast30Days(" + current_time.ToString() + ")");
			long thirty_days_ago = current_time - (30 * 24 * 60 * 60);
			var entires = new List<Entry>();

			using (var connection = new SQLiteConnection(connection_string)) {
				connection.Open();

				var command = new SQLiteCommand($"SELECT * FROM entries WHERE timestamp >= {thirty_days_ago}", connection);

				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						entires.Add(new Entry {
							Id = Convert.ToInt16( reader.GetInt64(0) ),
							EmotionId = Convert.ToInt16( reader.GetInt64(1) ),
							Timestamp = Convert.ToInt32( reader.GetInt64(2) )
						});
					}
				}
			}

			return entires;
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
