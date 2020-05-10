using CallingScore.Common.Models;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Data
{
    public class DataContext : IdentityDbContext<UserEntity>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<CallEntity> Calls { get; set; }

        public DbSet<ArrivalsEntity> Arrivals { get; set; }

        public DbSet<MonitoringEntity> Monitorings { get; set; }

        public DbSet<CampaignEntity> Campaigns { get; set; }

        public DbSet<CodificationEntity> Codifications { get; set; }

        public DbQuery<ContactStatistics> ContactStatistics { get; set; }

        public DbQuery<EffectivityStatistics> EffectivityStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEntity>()
                .HasIndex(u => u.UserCode)
                .IsUnique();
        }

    }
}
