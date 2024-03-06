using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.DTOs
{
    public class AccountDTO
    {
        public string Id { get; set; }
        
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn1 { get; set; }
    }
}
