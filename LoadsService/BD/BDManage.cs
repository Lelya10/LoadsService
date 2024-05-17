using Microsoft.EntityFrameworkCore;

namespace LoadsService.BD;

public class BDManage : DbContext
{
    public DbSet<LoadView> Loads { get; set; } = null!;
    
    public BDManage(DbContextOptions<BDManage> options) : base(options)
    {
        Database.EnsureCreated(); 
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var firmId = 11110;
        modelBuilder.Entity<LoadView>().HasData(
            new LoadView
            {
                Id = Guid.NewGuid(),
                UpdateTime = DateTime.UtcNow,
                FirmId = firmId,
                Name = "Игрушки",
                Weight = 4,
                Volume = 3,
                LoadingCityId = 1,
                LoadingTime = DateTime.Parse("2024-06-16 23:54:34Z").ToUniversalTime(),
                UnloadingCityId = 3,
                UnloadingTime = DateTime.Parse("2024-07-16 23:54:34Z").ToUniversalTime(),
                Price = 15
            });
    }

    public IResult GetLoadById(Guid id, int firmId)
    {
        LoadView? load = Loads.FirstOrDefault(u => u.Id == id);
        if (load == null)  return Results.NotFound(new { message = "Груз не найден" });
        if (load.FirmId != firmId) return Results.NotFound(new { message = "Данный груз принадлежит другому пользователю" });
 
        return Results.Json(load);
    }
    
    public async Task<IResult> DeleteLoad(Guid id, int firmId)
    {
        LoadView? load = Loads.FirstOrDefault(u => u.Id == id);
 
        if (load == null) return Results.NotFound(new { message = "Груз не найден" });
        if (load.FirmId != firmId) return Results.Problem(statusCode: 500, detail: "У вас нет прав для работы с этим грузом");
 
        Loads.Remove(load);
        await SaveChangesAsync();
        return Results.Json(load);
    }

    public async Task<IResult> AddLoad(NewLoad load, int firmId)
    {
        if (load.FirmId != firmId) return Results.Problem(statusCode: 500, detail: "Вы не можете добавлять грузы под чужой фирмой");
        var id = Guid.NewGuid();
        var newLoad = LoadView.AddLoad(load, id);
        try
        {
            await Loads.AddAsync(newLoad);
            await SaveChangesAsync();
            return Results.Json("Груз добавлен с id=" + id);
        }
        catch (Exception e)
        {
            return Results.Problem(e.ToString());
        }
    }

    public async Task<IResult> UpdateLoad(Load load, int firmId)
    {
        var oldLoad = Loads.FirstOrDefault(u => u.Id == load.LoadId);
        if (oldLoad == null) return Results.NotFound(new { message = "Груз не найден" });
        if (oldLoad.FirmId != firmId) return Results.Problem(statusCode: 500, detail: "У вас нет прав для работы с этим грузом");

        LoadView.UpdateLoad(load, oldLoad);
        await SaveChangesAsync();
        return Results.Json(oldLoad);
    }
    
    public async Task<IResult> GetLoadsByFirmId(int firmId)
    {
        var loads = Loads.Where(l => l.FirmId == firmId).ToList();
        if (loads == null)  return Results.NotFound(new { message = "У данной фирмы нет грузов" });

        return Results.Json(loads);
    }
    
    public async Task<IResult> SearchLoads(FilterForLoad filter)
    {
        var result = new List<LoadView>();
        foreach (var load in Loads)
        {
            if (filter.Name != null && filter.Name != load.Name) continue;
            if (filter.Weight != null && 
                (filter.Weight.WeightTo != null && filter.Weight.WeightTo <= load.Weight || 
                 filter.Weight.WeightFrom != null && filter.Weight.WeightFrom >= load.Weight)) continue;
            if (filter.Volume != null && 
                (filter.Volume.VolumeTo != null && filter.Volume.VolumeTo < load.Volume || 
                 filter.Volume.VolumeFrom != null && filter.Volume.VolumeFrom > load.Volume)) continue;
            if (filter.LoadingCityId != null && filter.LoadingCityId != load.LoadingCityId) continue;
            if (filter.UnloadingCityId != null && filter.UnloadingCityId != load.UnloadingCityId) continue;
            if (filter.LoadingTime != null && 
                (filter.LoadingTime.TimeTo != null && filter.LoadingTime.TimeTo < load.LoadingTime || 
                 filter.LoadingTime.TimeFrom  != null && filter.LoadingTime.TimeFrom > load.LoadingTime)) continue;
            if (filter.UnloadingTime != null && 
                (filter.UnloadingTime.TimeTo != null && filter.UnloadingTime.TimeTo < load.UnloadingTime || 
                 filter.UnloadingTime.TimeFrom  != null && filter.UnloadingTime.TimeFrom > load.UnloadingTime)) continue;
            if (filter.Price!= null && 
                (filter.Price.PriceTo != null && filter.Price.PriceTo < load.Price || 
                 filter.Price.PriceFrom != null && filter.Price.PriceFrom > load.Price)) continue;
            
            result.Add(load);
        }
        
        if (result.Count == 0)  return Results.NotFound(new { message = "Не нашлось грузов по фильтру" });

        return Results.Json(result);
    }
}