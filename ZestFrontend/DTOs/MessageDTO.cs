using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
	public class MessageDTO
	{
		public string SenderUsername { get; set; }
		public string Text { get; set; }
        public bool IsOwner { get; set; }
        public DateTime CreatedOn { get; set; }
	}
}
