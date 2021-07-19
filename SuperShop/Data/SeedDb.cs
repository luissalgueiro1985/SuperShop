﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");


            var user = await _userHelper.GetUserByEmailAsync("luisandresalgueiro@gmail.com");
            if(user == null)
            {
                user = new User
                {
                    FirstName = "Luís",
                    LastName = "Salgueiro",
                    Email = "luisandresalgueiro@gmail.com",
                    UserName = "luisandresalgueiro@gmail.com",
                    PhoneNumber = "961240000"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if(result!= IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");

            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");

            }

            if (!_context.Products.Any())
            {
                AddProduct("iPhone X", user);
                AddProduct("Magic Mouse",user);
                AddProduct("iWatch Series 4",user);
                AddProduct("iPad Mini",user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name = name,
                Price = _random.Next(1000),
                IsAvailable = true,
                Stock = _random.Next(100),
                User = user
            });
        }
    }
}
