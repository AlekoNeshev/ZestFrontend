using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class PostDTO : ObservableObject
    {
        private int _likes;
        private int _dislikes;
        private LikeDTO _like;
		public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Publisher { get; set; }
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
        public LikeDTO Like
		{
			get => _like;
			set => SetProperty(ref _like, value);
		}
		public string ResourceType { get; set; }
        public bool IsOwner { get; set; }
        public DateTime PostedOn { get; set; }

        
    }
}
