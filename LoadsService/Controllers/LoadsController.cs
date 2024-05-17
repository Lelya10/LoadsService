using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LoadsService.BD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace LoadsService.Controllers;

[ApiController]
[Authorize]
[Route("own_loads")]
public class LoadsController : ControllerBase
{
    private readonly ILogger<LoadsController> _logger;
    private readonly BDManage _bd;

    public LoadsController(ILogger<LoadsController> logger, BDManage bd)
    {
        _logger = logger;
        _bd = bd;
    }
    
    [HttpGet("loads/get")]
    public async Task<IResult> GetMyLoads()
    {
        try
        {
            var firmId = GetFirmId();
            if (firmId == 0)
            {
                _logger.LogError("Ошибка при получении firmId");
                return Results.StatusCode(500);
            }
            var res = await _bd.GetLoadsByFirmId(firmId);
            return  res;
        }
        catch (Exception e)
        {
            _logger.LogError("Ошибка при выполнении запроса: " + e);
            return Results.StatusCode(500);
        }
    }
    
    [HttpGet("load/get/{loadId}")]
    public IResult GetLoadById(Guid loadId)
    {
        try
        {
            var firmId = GetFirmId();
            if (firmId == 0)
            {
                _logger.LogError("Ошибка при получении firmId");
                return Results.StatusCode(500);
            }
            var res = _bd.GetLoadById(loadId, firmId);
            return  res;
        }
        catch (Exception e)
        {
            _logger.LogError("Ошибка при выполнении запроса: " + e);
            return Results.StatusCode(500);
        }
    }
    
    [HttpDelete("load/delete/{loadId}")]
    public async Task<IResult> DeleteLoad(Guid loadId)
    {
        try
        {
            var firmId = GetFirmId();
            if (firmId == 0)
            {
                _logger.LogError("Ошибка при получении firmId");
                return Results.StatusCode(500);
            }
            var res = await _bd.DeleteLoad(loadId, firmId);
            return  res;
        }
        catch (Exception e)
        {
            _logger.LogError("Ошибка при выполнении запроса: " + e);
            return Results.StatusCode(500);
        }
    }
    
    [HttpPost("load/add")]
    public async Task<IResult> AddLoad(NewLoad load)
    {
        try
        {
            var firmId = GetFirmId();
            if (firmId == 0)
            {
                _logger.LogError("Ошибка при получении firmId");
                return Results.StatusCode(500);
            }
            var res = await _bd.AddLoad(load, firmId);
            return  res;
        }
        catch (Exception e)
        {
            _logger.LogError("Ошибка при выполнении запроса: " + e);
            return Results.StatusCode(500);
        }
    }
    
    [HttpPut("load/update")]
    public async Task<IResult> UpdateLoad(Load load)
    {
        try
        {
            var firmId = GetFirmId();
            if (firmId == 0)
            {
                _logger.LogError("Ошибка при получении firmId");
                return Results.StatusCode(500);
            }
            var res = await _bd.UpdateLoad(load, firmId);
            return  res;
        }
        catch (Exception e)
        {
            _logger.LogError("Ошибка при выполнении запроса: " + e);
            return Results.StatusCode(500);
        }
    }

    private int GetFirmId()
    {
        Request.Headers.TryGetValue("Authorization", out StringValues headerValues);
        string jsonWebToken = headerValues.FirstOrDefault();
        string[] arr = jsonWebToken.Split(new char[] { ' ' });

        if (arr.Length == 2)
        {
            var token = new JwtSecurityToken(jwtEncodedString: arr[1]);
            string firmId = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            return Convert.ToInt32(firmId);
        }
        
        return 0;
    }
}