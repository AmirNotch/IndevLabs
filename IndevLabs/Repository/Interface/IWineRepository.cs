using IndevLabs.Models.db;

namespace IndevLabs.Repository.Interface;

public interface IWineRepository
{
    Task<IEnumerable<Wine>> GetWines(CancellationToken ct);
    Task<Wine> GetWineById(int wineId, CancellationToken ct);
    Task CreateWine(Wine wine, CancellationToken ct);
    Task UpdateWine(Wine wine, CancellationToken ct);
    Task DeleteWine(int wineId, CancellationToken ct);
}