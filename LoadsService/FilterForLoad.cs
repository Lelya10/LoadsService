namespace LoadsService;

public class FilterForLoad
{
    public string? Name { get; set; }
    
    public Weight? Weight { get; set; }
    
    public Volume? Volume { get; set; }
    
    public int? LoadingCityId { get; set; }
    
    public int? UnloadingCityId { get; set; }
    
    public Time? LoadingTime { get; set; }
    
    public Time? UnloadingTime { get; set; }
    
    public Price? Price { get; set; }
}


public class Weight
{
    public float? WeightFrom { get; set; }
    public float? WeightTo { get; set; }
}

public class Volume
{
    public float? VolumeFrom { get; set; }
    public float? VolumeTo { get; set; }
}

public class Time
{
    public DateTime? TimeFrom { get; set; }
    public DateTime? TimeTo { get; set; }
}

public class Price
{
    public double? PriceFrom { get; set; }
    public double? PriceTo { get; set; }
}