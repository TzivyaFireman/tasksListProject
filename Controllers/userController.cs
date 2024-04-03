using Microsoft.AspNetCore.Mvc;
using taskList.Models;
using taskList.Services;
using taskList.Interfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace taskList.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    IUserService UserService;
    ITokenService TokenService;
    IMyTaskService MyTaskService;
    private int userId;
    public UserController(IUserService UserService, ITokenService TokenService, IMyTaskService MyTaskService)
    {
        this.UserService = UserService;
        this.TokenService = TokenService;
        this.MyTaskService = MyTaskService;
        // this.userId = int.Parse(User.FindFirst("id")?.Value);
    }


    [HttpPost("/Login")]
    public ActionResult<string> login([FromBody] User user)
    {
        var dt = DateTime.Now;
        User currentUser = this.UserService.GetAll().FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);

        if (currentUser == null)
            return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim("type", "user"),
            new Claim("id",(currentUser.Id).ToString()),
        };
        if (currentUser.UserType == UserType.ADMIN)
            claims.Add(new Claim("type", "admin"));

        var token = TokenService.GetToken(claims);
        return new OkObjectResult(TokenService.WriteToken(token));
    }

    [HttpPost]
    [Authorize(Policy = "admin")]
    public ActionResult Create(User newUser)
    {
        UserService.Add(newUser);
        return CreatedAtAction(nameof(Create), new { id = newUser.Id }, UserService.GetById(newUser.Id));

    }

    [HttpGet]
    [Authorize(Policy = "admin")]
    public ActionResult<List<User>> Get()
    {
        return UserService.GetAll();
    }

    [HttpGet("Current")]
    [Authorize(Policy = "user")]
    public ActionResult<User> GetCurrent()
    {
        this.userId = int.Parse(User.FindFirst("id")?.Value);
        var user = UserService.GetById(userId);
        if (user == null)
            return NotFound();
        return user;
        // if(!(int.Parse(User.FindFirst("id")?.Value!)) && User.FindFirst("type")?.Value!="admin")
        //     return Unauthorized();
        // if (user == null)
        // return user;
    }

    // [HttpGet("{id}")]
    // public ActionResult<User> Get(int id)
    // {
    //     var User = UserService.GetById(id);
    //     if (User == null)
    //         return NotFound();
    //     return User;
    // }


    // [HttpPut("{id}")]
    // [Authorize(Policy = "admin")]
    // public ActionResult Put(int id, User newUser)
    // {
    //     var result = UserService.Update(id, newUser);
    //     if (!result)
    //     {
    //         return BadRequest();
    //     }
    //     return NoContent();
    // }


    [HttpDelete("{id}")]
    [Authorize(Policy = "admin")]
    public ActionResult Delete(int id)
    {
        var User = UserService.GetById(id);
        if (User == null)
            return NotFound();

        UserService.Delete(id);
        MyTaskService.DeleteByUserID(id);
        return NoContent();

    }
}