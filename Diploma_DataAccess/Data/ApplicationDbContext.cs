using Diploma_Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Diploma_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

    }
}
