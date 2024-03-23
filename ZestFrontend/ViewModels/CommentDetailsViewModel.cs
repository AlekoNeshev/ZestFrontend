using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ZestFrontend.DTOs;
using ZestFrontend.Pages;
using ZestFrontend.Parameters;
using ZestFrontend.Partial;
using ZestFrontend.Services;

namespace ZestFrontend.ViewModels
{
	[QueryProperty(nameof(Comment), "Comment")]
	public partial class CommentDetailsViewModel : ObservableObject
	{
        private readonly CommentService _commentService;
		private readonly AuthService _authService;
		private readonly LikesService _likesService;
        public CommentDetailsViewModel(CommentService commentService, AuthService authService, LikesService likesService)
        {
            _commentService = commentService;
			_authService = authService;
			_likesService = likesService;
			ReplyCommand = new ReplyCommand(ExecuteReplyCommand);
		}
        [ObservableProperty]
        CommentDTO comment;
		public ObservableCollection<CommentDTO> Replies { get; private set; } = new();
		public ICommand ReplyCommand { get; }
		async partial void OnCommentChanged(CommentDTO value)
		{
			Replies.Clear();
			var comment = await _commentService.GetSingleComment(Comment.Id);
			foreach (var item in comment.Replies)
			{
				Replies.Add(item);
			}
		}
		public async Task IsOwner(IEnumerable<CommentDTO> comments, int level)
		{
			foreach (var comment in comments)
			{

				comment.IsOwner = comment.Publisher==_authService.Username;
				comment.AreRepliesVisible = level<=4;
				if (comment.Replies == null)
				{
					return;
				}
				await IsOwner(comment.Replies, level + 1);

			}

		}
		
		[RelayCommand]
		async Task LikeCommentAsync(CommentDTO commentDTO)
		{
			if (commentDTO.Like == null)
			{

				await _likesService.Like(0, commentDTO.Id, true);
			}
			else if (commentDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, 0, commentDTO.Id);
			}
			else if (commentDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, 0, commentDTO.Id);
				await _likesService.Like(0, commentDTO.Id, true); //Needs fixing
			}
		}
		[RelayCommand]
		async Task DislikeCommentAsync(CommentDTO commentDTO) //Same
		{
			if (commentDTO.Like == null)
			{

				await _likesService.Like(0, commentDTO.Id, false); //Same
			}
			else if (commentDTO.Like.Value == false)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, 0, commentDTO.Id);
			}
			else if (commentDTO.Like.Value == true)
			{
				await _likesService.RemoveLike(commentDTO.Like.Id, 0, commentDTO.Id);
				await _likesService.Like(0, commentDTO.Id, false);
			}
		}
		[RelayCommand]
		async Task ReplyCommentAsync(CommentDTO comment)
		{
			comment.IsReplyVisible = !comment.IsReplyVisible;

		}
		[RelayCommand]
		async Task GoToCommentDetailPageAsync(CommentDTO comment)
		{
			if (comment == null) return;

			await Shell.Current.GoToAsync($"{nameof(CommentDetailsPage)}?id={comment.Id}", true,
				new Dictionary<string, object>
			{
			{"Comment", comment }
			});

		}

		public async Task SendReplyAsync(int comment, string text)
		{
			var response = await _commentService.PostComment(0, text, comment); //Same


			var content = await response.Content.ReadAsStringAsync();
			string[] parts = content.Trim('[', ']').Split(',');
			int firstNumber = int.Parse(parts[0]);
			int secondNumber = int.Parse(parts[1]);
			var reply = await _commentService.GetSingleComment(firstNumber);
			reply.IsOwner = reply.Publisher == _authService.Username;
			var commentToFind = FindCommentById(secondNumber, Comment.Replies);
			commentToFind.Replies.Add(reply);

		}
		public CommentDTO FindCommentById(int id, IEnumerable<CommentDTO> comments)
		{
			foreach (var comment in comments)
			{
				if (comment.Id == id)
				{
					return comment;
				}

				var foundInReplies = FindCommentById(id, comment.Replies);
				if (foundInReplies != null)
				{
					return foundInReplies;
				}
			}

			return null;
		}
		public async void UpdateComment(int id)
		{
			var updatedComment = await _commentService.GetSingleComment(id);
			var comment = FindCommentById(id, Comment.Replies);
			comment.Likes = updatedComment.Likes;
			comment.Dislikes = updatedComment.Dislikes;
			comment.Like = updatedComment.Like;
		}
		[RelayCommand]
		async Task DeleteCommentAsync(CommentDTO commentDTO)
		{
			await _commentService.DeleteComment(commentDTO.Id);
		}
		private async void ExecuteReplyCommand(ReplyCommandParameter parameter)
		{
			var comment = int.Parse(parameter.Comment);
			var text = parameter.ReplyText;
			await SendReplyAsync(comment, text);
			var commentToFind = FindCommentById(comment, Comment.Replies);
			commentToFind.IsReplyVisible = false;

		}

	}
}
