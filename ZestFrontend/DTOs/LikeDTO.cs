using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
	public class LikeDTO
	{
		public int Id { get; set; }

		public bool Value { get; set; }

		public string AccountId { get; set; }

		public int? PostId { get; set; }

		public int? CommentId { get; set; }
	}
}
