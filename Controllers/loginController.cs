using Microsoft.AspNetCore.Mvc;
using taskList.Models;
using taskList.Services;
using taskList.Interfaces;

namespace taskList.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    ILoginService LoginService;

    public LoginController(ILoginService LoginService)
    {
        this.LoginService = LoginService;
    }

    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return LoginService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var User = LoginService.GetById(id);
        if (User == null)
            return NotFound();
        return User;
    }

    [HttpPost]
    public ActionResult Create(User newUser)
    {
        LoginService.Add(newUser);
        return CreatedAtAction(nameof(Create), new { id = newUser.Id }, LoginService.GetById(newUser.Id));

    }


    [HttpPut("{id}")]
    public ActionResult Put(int id, User newUser)
    {
        var result = LoginService.Update(id, newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var User = LoginService.GetById(id);
        if (User == null)
            return NotFound();

        LoginService.Delete(id);

        return NoContent();

    }
}