using hw_reflect_attribute.Domain;
using Microsoft.AspNetCore.Mvc;

namespace hw_reflect_attribute.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class apiController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public apiController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public User GetUser(int id)
    {
        
        DateReader dateReader = new DateReader(_configuration);
        return dateReader.Read<User>(id);
    }
    
    [HttpGet]
    public Company GetCompany(int id)
    {
        DateReader dateReader = new DateReader(_configuration);
        return dateReader.Read<Company>(id);
    }
    [HttpGet]
    public List<Company> GetCompanyAll()
    {
        DateReader dateReader = new DateReader(_configuration);
        return dateReader.ReadAll<Company>();
    }
    
    [HttpGet]
    public List<User> GetUserAll()
    {
        DateReader dateReader = new DateReader(_configuration);
        return dateReader.ReadAll<User>();
    }
    
}