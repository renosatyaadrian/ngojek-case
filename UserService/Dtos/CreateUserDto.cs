using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}