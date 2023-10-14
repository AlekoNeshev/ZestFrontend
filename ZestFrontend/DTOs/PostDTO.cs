using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class PostDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Publisher { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime PostedOn { get; set; }
    }
}
