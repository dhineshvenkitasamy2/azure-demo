using System;
using System.Collections.Generic;
using AzureDemo.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AzureDemo.Models
{
    public partial class ConnectIndividualDemoContext : DbContext
    {
        public DbSet<NotificationModel> Notification { get; set; }
        public ConnectIndividualDemoContext(DbContextOptions<ConnectIndividualDemoContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
            modelBuilder.Entity<NotificationModel>(entity =>
            {
                entity.HasKey(e => e.id);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
