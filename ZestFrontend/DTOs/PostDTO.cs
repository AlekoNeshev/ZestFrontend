using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class PostDTO : PostBaseDTO
    {
        private int _likes;
        private int _dislikes;
        private LikeDTO _like;

        public int Id { get; set; }

        private string _publisher;

        public string Publisher
        {
			get => _publisher;
			set => SetProperty(ref _publisher, value);
		}

		private bool _isOwner;

		public bool IsOwner
		{
			get => _isOwner;
			set => SetProperty(ref _isOwner, value);
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
        public LikeDTO Like
		{
			get => _like;
			set => SetProperty(ref _like, value);
		}

		public string ResourceType { get; set; }
		public bool IsModerator { get; set; }
		public DateTime PostedOn { get; set; }
        public string CommunityName { get; set; }
		public int CommunityId { get; set; }

	}
}
