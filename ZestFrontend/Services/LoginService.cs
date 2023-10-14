using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZestFrontend.Services
{
    internal class LoginService
    {
        HttpClient _httpClient;
        public LoginService (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       
    }
}
