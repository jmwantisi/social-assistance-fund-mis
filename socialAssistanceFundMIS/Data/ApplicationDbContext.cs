using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Models;
using System.Linq.Expressions;

namespace socialAssistanceFundMIS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicantPhoneNumber> ApplicantPhoneNumbers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<AssistanceProgram> AssistancePrograms { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<GeographicLocation> GeographicLocations { get; set; }
        public DbSet<GeographicLocationType> GeographicLocationTypes { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<OfficialRecord> OfficialRecords { get; set; }
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
        public DbSet<Sex> Sexes { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure global query filters
            ConfigureGlobalFilters(modelBuilder);

            // Configure entity relationships and constraints
            ConfigureRelationships(modelBuilder);

            // Configure entity properties
            ConfigureProperties(modelBuilder);

            // Seed initial data
            SeedInitialData(modelBuilder);
        }

        private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
        {
            // Apply soft delete filter for all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsActive));
                    var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(true)), parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // Applicant relationships
            modelBuilder.Entity<Applicant>()
                .HasOne(a => a.Sex)
                .WithMany()
                .HasForeignKey(a => a.SexId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Applicant>()
                .HasOne(a => a.MaritalStatus)
                .WithMany()
                .HasForeignKey(a => a.MaritalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Applicant>()
                .HasOne(a => a.Village)
                .WithMany()
                .HasForeignKey(a => a.VillageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Phone number relationships
            modelBuilder.Entity<ApplicantPhoneNumber>()
                .HasOne(pn => pn.Applicant)
                .WithMany(a => a.PhoneNumbers)
                .HasForeignKey(pn => pn.ApplicantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicantPhoneNumber>()
                .HasOne(pn => pn.PhoneNumberType)
                .WithMany()
                .HasForeignKey(pn => pn.PhoneNumberTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Application relationships
            modelBuilder.Entity<Application>()
                .HasOne(app => app.Applicant)
                .WithMany(a => a.Applications)
                .HasForeignKey(app => app.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Geographic location hierarchy
            modelBuilder.Entity<GeographicLocation>()
                .HasOne(gl => gl.Type)
                .WithMany()
                .HasForeignKey(gl => gl.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureProperties(ModelBuilder modelBuilder)
        {
            // Configure string properties to use nvarchar instead of ntext
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && !property.IsPrimaryKey())
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property(property.Name)
                            .HasColumnType("nvarchar(max)");
                    }
                }
            }

            // Configure indexes for better performance
            modelBuilder.Entity<Applicant>()
                .HasIndex(a => a.IdentityCardNumber)
                .IsUnique();

            modelBuilder.Entity<Applicant>()
                .HasIndex(a => a.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");

            modelBuilder.Entity<Application>()
                .HasIndex(a => new { a.ApplicantId, a.CreatedAt });

            modelBuilder.Entity<GeographicLocation>()
                .HasIndex(gl => new { gl.TypeId, gl.Name });
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            // Seed lookup data
            SeedLookupData(modelBuilder);
        }

        private void SeedLookupData(ModelBuilder modelBuilder)
        {
            // Seed Sex data
            modelBuilder.Entity<Sex>().HasData(
                new Sex { Id = 1, Name = "Male", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Sex { Id = 2, Name = "Female", IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Seed MaritalStatus data
            modelBuilder.Entity<MaritalStatus>().HasData(
                new MaritalStatus { Id = 1, Name = "Single", IsActive = true, CreatedAt = DateTime.UtcNow },
                new MaritalStatus { Id = 2, Name = "Married", IsActive = true, CreatedAt = DateTime.UtcNow },
                new MaritalStatus { Id = 3, Name = "Divorced", IsActive = true, CreatedAt = DateTime.UtcNow },
                new MaritalStatus { Id = 4, Name = "Widowed", IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Seed PhoneNumberType data
            modelBuilder.Entity<PhoneNumberType>().HasData(
                new PhoneNumberType { Id = 1, Name = "Mobile", IsActive = true, CreatedAt = DateTime.UtcNow },
                new PhoneNumberType { Id = 2, Name = "Home", IsActive = true, CreatedAt = DateTime.UtcNow },
                new PhoneNumberType { Id = 3, Name = "Work", IsActive = true, CreatedAt = DateTime.UtcNow }
            );

            // Seed Status data
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Pending", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Status { Id = 2, Name = "Under Review", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Status { Id = 3, Name = "Approved", IsActive = true, CreatedAt = DateTime.UtcNow },
                new Status { Id = 4, Name = "Rejected", IsActive = true, CreatedAt = DateTime.UtcNow }
            );
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
