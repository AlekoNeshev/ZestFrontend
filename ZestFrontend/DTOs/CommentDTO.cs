using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class CommentDTO : ObservableObject
    {
        
        public string Publisher { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
    }
}
