using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Common;
using bike_selling_app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace bike_selling_app.Infrastructure.Persistence
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext, IApplicationDbContext
    {
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(
            DbContextOptions options,
            IDomainEventService domainEventService,
            IDateTime dateTime) : base(options)
        {
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<Bike> Bikes { get; set; }
        public DbSet<CapitalItem> CapitalItems { get; set; }
        public DbSet<NonCapitalItem> NonCapitalItems { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<RevenueItem> RevenueItems { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents();

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .Where(domainEvent => !domainEvent.IsPublished)
                    .FirstOrDefault();
                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }

        public void AddBike(Bike bike)
        {
            this.Bikes.Add(bike);
        }

        public void RemoveBike(Bike bike)
        {
            this.Bikes.Remove(bike);
        }

        public void AddProject(Project project)
        {
            this.Projects.Add(project);
        }

        public void RemoveProject(Project project)
        {
            this.Projects.Remove(project);
        }

        public async Task<IList<Bike>> GetAllBikes()
        {
            return await this.Bikes.ToListAsync();
        }

        public async Task<IList<Project>> GetAllProjects()
        {
            return await this.Projects.ToListAsync();
        }

        public Task<Project> GetProjectById(int id)
        {
            return Task.FromResult(this.Projects.Include(p => p.Bikes).SingleOrDefault(p => p.Id == id));
        }
    }

    // Reference: https://stackoverflow.com/questions/15220411/entity-framework-delete-all-rows-in-table
    public static class EntityExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
