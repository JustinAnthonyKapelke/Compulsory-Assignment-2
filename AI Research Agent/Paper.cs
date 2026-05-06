using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Research_Agent
{
    public class Paper
    {
        public string Title { get; set; } = "";
        public int Year { get; set; }
        public int Citations { get; set; }
        public string Url { get; set; } = "";
        public string? PdfUrl { get; set; }
        public string Authors { get; set; } = "";
    }
}
