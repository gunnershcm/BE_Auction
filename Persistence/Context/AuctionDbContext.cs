﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class AuctionDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;

        public AuctionDbContext(DbContextOptions<AuctionDbContext> options, IConfiguration? configuration)
        : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuctionDB");

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAuction> UserAuctions { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<UrlResource> UrlResources { get; set; }
        public virtual DbSet<AuctionHistory> AuctionHistories { get; set; }
        public virtual DbSet<TransferForm> TransferForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                //other automated configurations left out
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.AuthorPosts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Approver)
                .WithMany(u => u.ApproverPosts)
                .HasForeignKey(p => p.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferForm>()
                .HasOne(p => p.Author)
                .WithMany(u => u.AuthorTransferForms)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferForm>()
                .HasOne(p => p.Approver)
                .WithMany(u => u.ApproverTransferForms)
                .HasForeignKey(p => p.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Property)
                .WithMany()
                .HasForeignKey(a => a.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAuction>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAuctions)
                .HasForeignKey(ua => ua.UserId);

        }


    }
}
