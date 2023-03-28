using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BankRepository.BankAppData
{
    public partial class BankAppDataContext : DbContext
    {
        public BankAppDataContext()
        {
        }

        public BankAppDataContext(DbContextOptions<BankAppDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Disposition> Dispositions { get; set; } = null!;
        public virtual DbSet<Loan> Loans { get; set; } = null!;
        public virtual DbSet<PermenentOrder> PermenentOrders { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=BankAppData;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Balance).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Frequency).HasMaxLength(50);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.Ccnumber)
                    .HasMaxLength(50)
                    .HasColumnName("CCNumber");

                entity.Property(e => e.Cctype)
                    .HasMaxLength(50)
                    .HasColumnName("CCType");

                entity.Property(e => e.Cvv2)
                    .HasMaxLength(10)
                    .HasColumnName("CVV2");

                entity.Property(e => e.Issued).HasColumnType("date");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Disposition)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.DispositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cards_Dispositions");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CountryCode).HasMaxLength(2);

                entity.Property(e => e.Emailaddress).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(6);

                entity.Property(e => e.Givenname).HasMaxLength(100);

                entity.Property(e => e.NationalId).HasMaxLength(20);

                entity.Property(e => e.Streetaddress).HasMaxLength(100);

                entity.Property(e => e.Surname).HasMaxLength(100);

                entity.Property(e => e.Telephonecountrycode).HasMaxLength(10);

                entity.Property(e => e.Telephonenumber).HasMaxLength(25);

                entity.Property(e => e.Zipcode).HasMaxLength(15);
            });

            modelBuilder.Entity<Disposition>(entity =>
            {
                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Dispositions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dispositions_Accounts");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Dispositions)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dispositions_Customers");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Payments).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Loans_Accounts");
            });

            modelBuilder.Entity<PermenentOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("PermenentOrder");

                entity.Property(e => e.AccountTo).HasMaxLength(50);

                entity.Property(e => e.Amount).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.BankTo).HasMaxLength(50);

                entity.Property(e => e.Symbol).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.PermenentOrders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PermenentOrder_Accounts");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasIndex(e => e.AccountId, "IX_Transactions_AccountId");

                entity.Property(e => e.Account).HasMaxLength(50);

                entity.Property(e => e.Amount).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Balance).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.Bank).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Operation).HasMaxLength(50);

                entity.Property(e => e.Symbol).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Accounts");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.FirstName).HasMaxLength(40);

                entity.Property(e => e.LastName).HasMaxLength(40);

                entity.Property(e => e.LoginName).HasMaxLength(40);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(64)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
