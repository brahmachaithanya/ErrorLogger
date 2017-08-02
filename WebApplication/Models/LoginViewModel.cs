using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class LoginViewModel
    {
        public String UserName { set; get; }
        public String Password { set; get; }
        public string ReturnUrl { get; internal set; }
    }
}