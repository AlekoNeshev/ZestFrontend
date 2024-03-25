using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
	public class MessageGroup : List<MessageDTO>
	{
		public DateTime Date { get; set; }
	}

}
