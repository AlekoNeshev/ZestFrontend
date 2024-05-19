using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class CommunityDTO : CommunityBaseDTO
    {
        public int Id { get; set; }
        public string Creator { get; set; }
		public DateTime CreatedOn { get; set; }
		public bool IsSubscribed { get; set; }
	}
}
