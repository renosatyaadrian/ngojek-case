using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public record TransactionStatus
    (
        bool Succeed,
        string? Message
    );
}