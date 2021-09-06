using Microsoft.EntityFrameworkCore;
using OneTree.Assessment.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace OneTree.Assessment.Domain
{
    public class OneTreeAssessmentContext : DbContext, IQueryableUnitOfWork
    {

        public OneTreeAssessmentContext(DbContextOptions<OneTreeAssessmentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }


            modelBuilder.Entity<Product>()
                .ToTable(nameof(Product)).HasKey(k => k.Id);
            modelBuilder.Entity<Product>().Property(e => e.DateCreated).HasDefaultValueSql("GETUTCDATE()");

        }

        public DbContext GetContext()
        {
            return this;
        }

        public DbSet<TEntity> GetSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await ex.Entries.Single().ReloadAsync().ConfigureAwait(false);
            }
        }

        public void DetachLocal<TEntity>(TEntity entity, EntityState state) where TEntity : class
        {

            if (entity is null)
            {
                return;
            }

            var local = Set<TEntity>().Local.ToList();

            if (local?.Any() ?? false)
            {
                local.ForEach(item =>
                {
                    Entry(item).State = EntityState.Detached;
                });
            }

            Entry(entity).State = state;
        }
    }
}
