namespace LoadsService;

public class NewLoad
{
    public int FirmId { get; set; }
    
    public string Name { get; set; }
    
    public float Weight { get; set; }
    
    public float Volume { get; set; }
    
    public string LoadingCity { get; set; }
    public int? LoadingCityId { get; set; }
    
    public string UnloadingCity { get; set; }
    public int? UnloadingCityId { get; set; }
    
    public DateTime LoadingTime { get; set; }
    
    public DateTime UnloadingTime { get; set; }
    
    public double Price { get; set; }
}