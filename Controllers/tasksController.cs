using Microsoft.AspNetCore.Mvc;
using taskList.Services;
using taskList.Models;
using taskList.Interfaces;
using Microsoft.AspNetCore.Authorization;
using taskList.Controllers;



namespace taskList.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{

    IMyTaskService MyTaskService;

    public TaskController(IMyTaskService MyTaskService)
    {
        this.MyTaskService = MyTaskService;
    }

    [HttpGet]
    [Authorize(Policy = "user")]
    public ActionResult<List<MyTask>> Get()
    {
        return MyTaskService.GetAll(int.Parse(User.FindFirst("id")?.Value));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "user")]
    public ActionResult<MyTask> Get(int id)
    {
        var task = MyTaskService.GetById(id);
        if (task == null)
            return NotFound();
        if (!chekAuthorization(id))
            return Unauthorized();

        return task;
    }

    [HttpPost]
    [Authorize(Policy = "user")]
    public IActionResult Create(MyTask myTask)
    {
        myTask.Owner = int.Parse(User.FindFirst("Id").Value);
        MyTaskService.Add(myTask);
        return CreatedAtAction(nameof(Create), new { id = myTask.Id }, myTask);

    }


    [HttpPut("{id}")]
    [Authorize(Policy = "user")]
    public ActionResult Put(int id, MyTask newTask)
    {
        if(!chekAuthorization(id))
            return Unauthorized();
        newTask.Owner = int.Parse(User.FindFirst("Id").Value);
        var result = MyTaskService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "user")]
    public ActionResult Delete(int id)
    {
        var task = MyTaskService.GetById(id);
        if (task == null)
            return NotFound();
        if (!chekAuthorization(id))
            return Unauthorized();
        MyTaskService.Delete(id);

        return NoContent();

    }

    private bool chekAuthorization(int taskId)
    {
        return MyTaskService.GetById(taskId).Owner == int.Parse(User.FindFirst("id").Value);

    }
}
