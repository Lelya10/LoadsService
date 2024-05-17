using System.Runtime.InteropServices.JavaScript;
using LoadsService.BD;
using Microsoft.AspNetCore.Mvc;

namespace LoadsService.Controllers;

[ApiController]
[Route("internal")]
public class InternalController : ControllerBase
{
    private readonly ILogger<InternalController> _logger;
    private readonly BDManage _bd;

    public InternalController(ILogger<InternalController> logger, BDManage bd)
    {
        _logger = logger;
        _bd = bd;
    }
    
    [HttpPost("loads/search")]
    public async Task<IResult> SearchLoads(FilterForLoad filter)
    {
        try
        {
            var res = await _bd.SearchLoads(filter);
            return  res;
        }
        catch (Exception e)
        {
            _logger.LogError("Ошибка при выполнении запроса: " + e);
            return Results.StatusCode(500);
        }
    }
}