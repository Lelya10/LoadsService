namespace LoadsService;

public class LoadView
{
    public Guid Id { get; set; }
    
    public int FirmId { get; set; }

    public DateTime UpdateTime { get; set; }

    public string Name { get; set; }
    
    public float Weight { get; set; }
    
    public float Volume { get; set; }
    
    public int LoadingCityId { get; set; }
    
    public int UnloadingCityId { get; set; }
    
    public DateTime LoadingTime { get; set; }
    
    public DateTime UnloadingTime { get; set; }
    
    public double Price { get; set; }


    public static LoadView AddLoad(NewLoad load, Guid id)
    {
        return new LoadView
        {
            Id = id,
            FirmId = load.FirmId,
            Name = load.Name,
            Weight = load.Weight,
            Volume = load.Volume,
            LoadingCityId = Cities.GetCityId(load.LoadingCity),
            UnloadingCityId = Cities.GetCityId(load.UnloadingCity),
            LoadingTime = load.LoadingTime,
            UnloadingTime = load.UnloadingTime,
            Price = load.Price,
            UpdateTime = DateTime.Now
        };
    }

    public static void UpdateLoad(Load load, LoadView oldLoad)
    {
        oldLoad.Name = load.Name ?? oldLoad.Name;
        oldLoad.Weight = load.Weight ?? oldLoad.Weight;
        oldLoad.Volume = load.Volume ?? oldLoad.Volume;
        oldLoad.LoadingCityId = load.LoadingCityId ?? (load.LoadingCity != null ? Cities.GetCityId(load.LoadingCity) : oldLoad.LoadingCityId);
        oldLoad.UnloadingCityId = load.UnloadingCityId ?? (load.UnloadingCity != null ? Cities.GetCityId(load.UnloadingCity) : oldLoad.UnloadingCityId);
        oldLoad.LoadingTime = load.LoadingTime ?? oldLoad.LoadingTime;
        oldLoad.UnloadingTime = load.UnloadingTime ?? oldLoad.UnloadingTime;
        oldLoad.Price = load.Price ?? oldLoad.Price;
        oldLoad.UpdateTime = DateTime.Now;
    }
}