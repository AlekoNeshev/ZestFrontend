using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class PostResourcesDTO : INotifyPropertyChanged
    {
		public int Id { get; set; }
		public string Type { get; set; }
		private string _source;
		public string Source
		{
			get => _source;
			set
			{
				_source = value;
				OnPropertyChanged(nameof(Source));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
