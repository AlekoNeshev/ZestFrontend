using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class CommentDTO : ObservableObject
    {
        private string _publisher;
        private string _text;
        private int _likes;
        private int _dislikes;
        private bool _isReplyVisible;
		private LikeDTO _like;
		private bool _areRepliesVisible = true;
		public int Id { get; set; }
        public string Publisher 
        { 
            get => _publisher; 
            set => SetProperty(ref _publisher, value); 
        }
        public string Text 
        { 
            get=>_text;
            set => SetProperty(ref _text, value); 
        }
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
		public bool IsReplyVisible
		{
			get => _isReplyVisible;
			set => SetProperty(ref _isReplyVisible, value);
		}
		public bool AreRepliesVisible
		{
			get => _areRepliesVisible;
			set => SetProperty(ref _areRepliesVisible, value);
		}
		public LikeDTO Like
		{
			get => _like;
			set => SetProperty(ref _like, value);
		}
		public bool IsOwner { get; set; }
        public ObservableCollection<CommentDTO> Replies { get; set; } = new ObservableCollection<CommentDTO>();
        public DateTime PostedOn { get; set; }
    }
}
