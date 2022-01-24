using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Models;

namespace UserService.Data
{
    public class PrepDb
    {
        public static void PrePopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            };
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {  
                Console.Write("--> Menjalankan migrasi");
                try
                {
                     context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Gagal Menjalankan Migrasi dengan error: {ex.Message}");
                }
            }

            if(!context.Customers.Any())
            {
                Console.WriteLine("--> Seeding data Customers -->");
                context.Customers.AddRange(
                    new Customer{ Username = "renosatyaadrian", FirstName = "Reno", LastName = "Satya", PhoneNumber = "088239185512", Email = "renosatyaadrian@gmail.com", Balance = 100000, CreatedDate = DateTime.Now, Blocked = false },
                    new Customer { Username = "rezaaditya", FirstName = "Reza", LastName = "Aditya", PhoneNumber = "088239185513", Email = "rezaaditya@gmail.com", Balance = 100000, CreatedDate = DateTime.Now, Blocked = false }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have Customers data -->");
            }

            if(!context.Drivers.Any())
            {
                Console.WriteLine("--> Seeding data Drivers -->");
                context.Drivers.AddRange(
                    new Driver(){ Username = "bonigendul", FirstName = "Boni", LastName = "Gendul", Email = "bonigendul@gmail.com", PhoneNumber = "88291282", DriverLatitude = 0, DriverLongitude = 0, Balance = 200000, Blocked = false},
                    new Driver() { Username = "rijaldikurniawan", FirstName = "Rijaldi", LastName = "Kurniawan", Email = "rijaldikurniawan@gmail.com", PhoneNumber = "88291822", DriverLatitude = 0, DriverLongitude = 0, Balance = 30000, Blocked = false }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have Drivers data -->");
            }
            
            if(!context.ConfigApps.Any())
            {
                Console.WriteLine("--> Seeding data ConfigApps -->");
                context.ConfigApps.AddRange(
                    new ConfigApp(){ PricePerKM = 10000 }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have ConfigApps data -->");
            }

            if(!context.Orders.Any())
            {
                Console.WriteLine("--> Seeding data Orders -->");
                context.Orders.AddRange(
                    new Order(){ CustomerId = 1, DriverId = 1, Price = 110000, Distance = 11, Completed = false, PickedUp = true, UserLatitude = 1, UserLongitude = 0 },
                    new Order(){ CustomerId = 2, DriverId = 2, Price = 30000, Distance = 3, Completed = false, PickedUp = true, UserLatitude = 0.3, UserLongitude = 0 }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Already have Orders data -->");
            }
        }
    }
}