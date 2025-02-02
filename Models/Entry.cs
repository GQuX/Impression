namespace Impression.Models {
    public class Entry {
        public int Id { get; set; }
        public int EmotionId { get; set; }
        public long Timestamp { get; set; }
        public DateTime DateTime { get; set; }
		public string? EmotionName { get; set; }
        public string? EmotionColor { get; set; }
	}
}