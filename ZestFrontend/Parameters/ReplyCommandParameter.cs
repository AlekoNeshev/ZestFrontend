using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZestFrontend.DTOs;

namespace ZestFrontend.Parameters
{
	public class ReplyCommandParameter : BindableObject
	{
		public static readonly BindableProperty ReplyTextProperty =
	   BindableProperty.Create(nameof(ReplyText), typeof(string), typeof(ReplyCommandParameter));

		public string ReplyText
		{
			get { return (string)GetValue(ReplyTextProperty); }
			set { SetValue(ReplyTextProperty, value); }
		}
		public static readonly BindableProperty CommentProperty =
		BindableProperty.Create(nameof(Comment), typeof(string), typeof(ReplyCommandParameter));

		public string Comment
		{
			get { return (string)GetValue(CommentProperty); }
			set { SetValue(CommentProperty, value); }
		}
	}
}
