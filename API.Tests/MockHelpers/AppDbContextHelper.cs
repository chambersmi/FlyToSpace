using API.Domain.Entities;
using API.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.MockHelpers
{
    public static class AppDbContextHelper
    {
        public static AppDbContext CreateInMemoryContext(params ApplicationUser[] users)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "MockDB")
                .Options;

            var context = new AppDbContext(options);

            // Add users to the in-memory database, if provided
            if(users != null && users.Length > 0)
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return context;

        }
    }
}
