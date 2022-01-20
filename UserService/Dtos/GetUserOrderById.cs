using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class GetUserOrderByIdDto
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
    }
}