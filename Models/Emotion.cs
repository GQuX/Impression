﻿namespace Impression.Models {
    public class Emotion {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Level { get; set; }
        public string? Color { get; set; }
        public int? ParentId { get; set; }
    }
}
