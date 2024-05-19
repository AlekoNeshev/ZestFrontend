using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class PostBaseDTO : ObservableObject
    {
		private string _text;

		private string _title;
		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}
		public string Text
		{
			get => _text;
			set => SetProperty(ref _text, value);
		}
	}
}
