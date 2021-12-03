using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using sarapi.Data;
using Sarapi.Configurations.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi
{
    public class DatabaseContext:IdentityDbContext<User>
    {
       
      //  public DbSet<IdentityDbContext> identityDbContexts { get; set; }
        // make constructor in database context
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // add role configuration in model
            builder.ApplyConfiguration(new RoleConfiguration());
        }
        
    }
}
