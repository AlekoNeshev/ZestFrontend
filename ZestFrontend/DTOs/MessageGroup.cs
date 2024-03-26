using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
	public class MessageGroup : ObservableCollection<MessageDTO>
	{
		public DateTime Date { get; set; }
	}

}
