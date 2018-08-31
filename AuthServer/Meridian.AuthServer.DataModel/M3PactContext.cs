using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Meridian.AuthServer.DataModel
{
    public partial class M3PactContext : DbContext
    {
        public virtual DbSet<ApiResource> ApiResource { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<ApplicationApiResource> ApplicationApiResource { get; set; }
        public virtual DbSet<ApplicationType> ApplicationType { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Title> Title { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }

        public IConfiguration _Configuration { get; }
        public M3PactContext(DbContextOptions<M3PactContext> options, IConfiguration configuration) : base(options)
        {
            _Configuration = configuration;

        }

        // Unable to generate entity type for table 'dbo.Sheet2$'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiResource>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RedirectUrl).HasColumnName("RedirectURL");

                entity.Property(e => e.Secret)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Applicati__TypeI__39E294A9");
            });

            modelBuilder.Entity<ApplicationApiResource>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApiResourceId).HasColumnName("ApiResourceID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApplicationApiResource)
                    .HasForeignKey(d => d.ApiResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__ApiRe__3CBF0154");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationApiResource)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Appli__3DB3258D");
            });

            modelBuilder.Entity<ApplicationType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.LogLevel).HasMaxLength(50);

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("('A')");

                entity.Property(e => e.RoleCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RoleDesc)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.Property(e => e.TitleId).HasColumnName("TitleID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.TitleCode).HasMaxLength(255);

                entity.Property(e => e.TitleDescription).HasMaxLength(255);

                entity.Property(e => e.TitleName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(60);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.LastPasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.LastSuccessfulLogin).HasColumnType("datetime");

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.MiddleName).HasMaxLength(255);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.RecordStatus)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__UserLogin__RoleI__1F2E9E6D");
            });
        }
    }
}
