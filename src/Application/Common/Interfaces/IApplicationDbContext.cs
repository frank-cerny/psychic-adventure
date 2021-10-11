using bike_selling_app.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace bike_selling_app.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        void AddBike(Bike bike);
        void RemoveBike(Bike bike);
        void AddProject(Project project);
        void RemoveProject(Project project);
        void AddExpenseItem(ExpenseItem item);
        void RemoveExpenseItem(ExpenseItem item);
        void AddCapitalItem(CapitalItem item);
        void RemoveCapitalItem(CapitalItem item);
        void AddNonCapitalItem(NonCapitalItem item);
        void RemoveNonCapitalItem(NonCapitalItem item);
        Task<IList<Bike>> GetAllBikes();
        Task<Project> GetProjectById(int id);
        Task<IList<Project>> GetAllProjects();
        Task<IList<CapitalItem>> GetAllCapitalItems();
        Task<IList<NonCapitalItem>> GetAllNonCapitalItems();
        Task<IList<RevenueItem>> GetAllRevenueItems();
        Task<IList<ExpenseItem>> GetAllExpenseItems();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
