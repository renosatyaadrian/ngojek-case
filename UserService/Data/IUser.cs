using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Data
{
    public interface IUser
    {
        Task AddRole(string rolename);
        Task<User> Authenticate(string username, string password);
        List<CreateRoleDto> GetAllRole();
        Task Registration(CreateUserDto user);
        Task<Customer> GetUserProfile();
        Task<Customer> TopupBalance(double amount);
    }
}