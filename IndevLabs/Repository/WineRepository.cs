using IndevLabs.Models;
using IndevLabs.Models.db;
using IndevLabs.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace IndevLabs.Repository;

public class WineRepository : IWineRepository
{
    private readonly ILogger<WineRepository> _logger;
    private readonly IndevLabsDbContext _dbContext;

    public WineRepository(IndevLabsDbContext dbContext, ILogger<WineRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<IEnumerable<Wine>> GetWines(CancellationToken ct)
    {
        return await _dbContext.Wines.OrderBy(x => x.Year).ToListAsync(ct);
    }

    public async Task<Wine> GetWineById(int wineId, CancellationToken ct)
    {
        return await _dbContext.Wines
            .Where(e => e.Id == wineId)
            .FirstOrDefaultAsync(ct);
    }

    public async Task CreateWine(Wine wine, CancellationToken ct)
    {
        _dbContext.Wines.Add(wine);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateWine(Wine wine, CancellationToken ct)
    {
        await _dbContext.Wines
            .Where(i => i.Id == wine.Id)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(i => i.Title, wine.Title) // Обновляем Title
                    .SetProperty(i => i.Year, wine.Year) // Обновляем Year
                    .SetProperty(i => i.Brand, wine.Brand) // Обновляем Brand
                    .SetProperty(i => i.Type, wine.Type), // Обновляем Type
                ct);
    }

    public async Task DeleteWine(int wineId, CancellationToken ct)
    {
        await _dbContext.Wines
            .Where(i => i.Id == wineId)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<bool> WineExists(int wineId, CancellationToken ct)
    {
        Wine? wine = await GetByIdOptional(wineId, ct);
        return wine != null;
    }

    public async Task<Wine?> GetByIdOptional(int wineId, CancellationToken ct)
    {
        return await _dbContext.Wines.FindAsync(wineId, ct);
    }
}