﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
	public class UserDTO
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public bool IsFollowed { get; set; }
		public DateTime CreatedOn1 { get; set; }
	}
}
