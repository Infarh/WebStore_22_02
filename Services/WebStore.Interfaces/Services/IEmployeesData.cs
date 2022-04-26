using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IEmployeesData
{
    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken Cancel = default);

    Task<Employee?> GetByIdAsync(int id, CancellationToken Cancel = default);

    Task<int> AddAsync(Employee employee, CancellationToken Cancel = default);

    Task<bool> EditAsync(Employee employee, CancellationToken Cancel = default);

    Task<bool> DeleteAsync(int id, CancellationToken Cancel = default);
}