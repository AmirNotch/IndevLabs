using AutoMapper;
using IndevLabs.Models.db;
using IndevLabs.Models.Wines;
using IndevLabs.Repository.Interface;
using IndevLabs.Util;
using IndevLabs.Validation;

namespace IndevLabs.Service;

public class WineService
{
    private readonly ILogger<WineService> _logger;
    private readonly IValidationStorage _validationStorage;
    private readonly IWineRepository _wineRepository;
    private readonly IMapper _mapper;
    
    public WineService(ILogger<WineService> logger, IValidationStorage validationStorage,
        IWineRepository wineRepository, IMapper mapper)
    {
        _logger = logger;
        _validationStorage = validationStorage;
        _mapper = mapper;
        _wineRepository = wineRepository;
    }

    #region Action

    public async Task<IEnumerable<WineDto>> GetWines(CancellationToken ct)
    {
        IEnumerable<Wine> wines = await _wineRepository.GetWines(ct);
        var winesDto = _mapper.Map<List<WineDto>>(wines);
        return winesDto;
    }
    
    public async Task<WineDto> GetWineById(int id, CancellationToken ct)
    {
        bool isValid = await ValidateGetWineById(id, ct);
        if (!isValid)
        {
            return null!;
        }
        Wine wine = await _wineRepository.GetWineById(id, ct);
        var wineDto = _mapper.Map<WineDto>(wine);
        return wineDto;
    }
    
    public async Task<bool> CreateWine(WineDto wineDto, CancellationToken ct)
    {
        var wine = new Wine()
        {
            Title = wineDto.Title,
            Year = wineDto.Year,
            Brand = wineDto.Brand,
            Type = wineDto.Type
        };
        await _wineRepository.CreateWine(wine, ct);
        return true;
    }
    
    public async Task<bool> UpdateWine(Wine wine, CancellationToken ct)
    {
        bool isValid = await ValidateUpdateWine(wine, ct);
        if (!isValid)
        {
            return false;
        }
        
        await _wineRepository.UpdateWine(wine, ct);
        return true;
    }
    
    public async Task<bool> DeleteWine(int wineId, CancellationToken ct)
    {
        bool isValid = await ValidateDeleteWine(wineId, ct);
        if (!isValid)
        {
            return false;
        }
        
        await _wineRepository.DeleteWine(wineId, ct);
        return true;
    }
    
    #endregion
    
    #region Validation
    
    public async Task<bool> ValidateGetWineById(int id, CancellationToken ct)
    {
        if (!await _wineRepository.WineExists(id, ct))
        {
            ValidationUtils.AddUnknownWineError(_validationStorage, id);
        }
        return _validationStorage.IsValid;
    }
    
    public async Task<bool> ValidateUpdateWine(Wine wine, CancellationToken ct)
    {
        var item = await _wineRepository.GetByIdOptional(wine.Id, ct);
        if (item == null)
        {
            ValidationUtils.AddUnknownWineError(_validationStorage, wine.Id);
            return false;
        }
        return _validationStorage.IsValid;
    }
    
    public async Task<bool> ValidateDeleteWine(int id, CancellationToken ct)
    {
        if (!await _wineRepository.WineExists(id, ct))
        {
            ValidationUtils.AddUnknownWineError(_validationStorage, id);
        }
        return _validationStorage.IsValid;
    }
    
    #endregion
}