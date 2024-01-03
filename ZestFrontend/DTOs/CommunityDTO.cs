﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class CommunityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
