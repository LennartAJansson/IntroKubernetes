using Microsoft.EntityFrameworkCore;

using SmhiDb.Abstract;
using SmhiDb.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmhiDb
{
    public class SmhiDbContext : DbContext, IUnitOfWork
    {
        /*
         * En SmhiStation har:
         * ICollection of SmhiValue
         * ICollection of SmhiPosition
         * ICollection of SmhiLink
         * One SmhiParameter
        */

        public DbSet<SmhiStation> Stations { get; set; }
        public DbSet<SmhiParameter> Parameters { get; set; }
        public DbSet<SmhiValue> Values { get; set; }
        public DbSet<SmhiPosition> Positions { get; set; }
        public DbSet<SmhiLink> Links { get; set; }


        private readonly Dictionary<Type, object> repositories = new();

        public SmhiDbContext(DbContextOptions<SmhiDbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmhiValue>()
                    .HasIndex("Date", "SmhiStationId");
            //.HasIndex(p => p.Date);
        }

        

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (repositories.Keys.Contains(typeof(TEntity)) == true)
            {
                return repositories[typeof(TEntity)] as IGenericRepository<TEntity>;
            }

            IGenericRepository<TEntity> repo = new GenericRepository<TEntity>(this);

            repositories.Add(typeof(TEntity), repo);

            return repo;
        }

        public Task AddMigrations()
        {
            var migrations = Database.GetPendingMigrations();
            if (migrations.Any())
            {
                Console.WriteLine($"Applying {migrations.Count()} migrations to database");
                Database.Migrate();
            }
            return Task.CompletedTask;
        }
    }
}

