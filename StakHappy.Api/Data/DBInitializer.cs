using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StakHappy.Api.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StakHappy.Api.Middleware.DataModels;
using StakHappy.Api.Middleware;
using Microsoft.Extensions.Options;

//using Microsoft.EntityFrameworkCore;

namespace StakHappy.Api.Data
{
    public class DbInitializer
    {
        public static async Task Initialize()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Database.Migrate();
            var userStore = new UserStore<ApplicationUser>(context);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null, null);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole("Administrator"),
                new IdentityRole("User")
            };
            foreach (IdentityRole role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}