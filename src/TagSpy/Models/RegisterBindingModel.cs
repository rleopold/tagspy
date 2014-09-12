using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TagSpy.Models
{
    public class RegisterBindingModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}