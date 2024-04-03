using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskThree.DA.Models;
using TaskThree.PL.Models;


namespace TaskThree.DA.Data
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseSqlServer("Server=.; Database= MVCApplication; Trusted_Connection= True;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet <Department> Departments { get; set; }
        public DbSet <Employee> Employees { get; set; }
        //public DbSet <IdentityUser> Users { get; set; }
        //public DbSet <IdentityRole> Roles { get; set; }

    }
}
