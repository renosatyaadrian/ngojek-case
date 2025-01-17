﻿using DriverService.Dtos;
using DriverService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DriverService.Data
{
    public interface IDriverRepo
    {
        bool SaveChanges();

        //Administration
        Task<User> Login(string username, string password);
        Task Registration(DriverForCreateDto driverForCreateDto);
        Task<List<string>> GetRolesFromUser(string username);
        Task AddRole(string rolename);
        void CreateDriver(Driver driver);

        //Profile
        Driver ShowProfile();
        void SetPosition(Driver obj);

        //Order
        void AcceptOrder(CustIdDto custIdDto);
        void FinishOrder(CustIdDto custIdDto);
    }
}
