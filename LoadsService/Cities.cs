using Microsoft.OpenApi.Models;

namespace LoadsService;

public static class Cities
{
    public static Dictionary<int, string> CitiesDictionary = new()
    {
        { 0, "Москва" },
        { 1, "Санкт-Петербург" },
        { 2, "Тверь" },
        { 3, "Казань" },
        { 4, "Ростов" },
        { 5, "Воронеж" },
        { 6, "Уфа" },
        { 7, "Красноярск" },
        { 8, "Калининград" },
        { 9, "Ярославль" },
        { 10, "Владивосток" },
    };

    public static int GetCityId(string cityName)
    {
        foreach (var city in CitiesDictionary)
        {
            if (city.Value == cityName) return city.Key;
        }

        return 0;
    }
}