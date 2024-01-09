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
        private int _likes;
        private int _dislikes;
        public int Id { get; set; }
        public string Publisher { get; set; }
        public string Text { get; set; }
        public int Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }
        public int Dislikes
        {
            get => _dislikes;
            set => SetProperty(ref _dislikes, value);
        }
        public List<CommentDTO> Replies { get; set; } = new List<CommentDTO>();
        public DateTime PostedOn { get; set; }
    }
}
