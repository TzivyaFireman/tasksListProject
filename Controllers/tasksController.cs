using Microsoft.AspNetCore.Mvc;
using taskList.Models;
using taskList.Services;

namespace taskList.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<MyTask>> Get()
    {
        return TaskService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<MyTask> Get(int id)
    {
        var task = TaskService.GetById(id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public ActionResult Post(MyTask newTask)
    {
        var newId = TaskService.Add(newTask);

        return CreatedAtAction("Post",
            new { id = newId }, TaskService.GetById(newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, MyTask newTask)
    {
        var result = TaskService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var task = TaskService.GetById(id);
        if (task==null)
            return NotFound();

        TaskService.Delete(id);

        return NoContent();

    }
}
