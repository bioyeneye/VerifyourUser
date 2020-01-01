using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VerifyourUser.DataAccess.Entities;

namespace VerifyourUser.DataAccess.Context
{
    public class VerifyourUserDbContext : DbContext
    {
        public virtual DbSet<Audience> Audiences { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserVerificationToken> UserVerificationTokens { get; set; }

        public VerifyourUserDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
