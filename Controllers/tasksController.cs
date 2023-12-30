using Microsoft.AspNetCore.Mvc;
using taskList.Services;
using taskList.Models;
using taskList.Interfaces;


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
    public ActionResult<List<MyTask>> Get()
    {
        return MyTaskService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<MyTask> Get(int id)
    {
        var task = MyTaskService.GetById(id);
        if (task == null)
            return NotFound();
        return task;
    }

            [HttpPost] 
        public IActionResult Create(MyTask myTask)
        {
            MyTaskService.Add(myTask);
            return CreatedAtAction(nameof(Create), new {id=myTask.Id}, myTask);

        }


    [HttpPut("{id}")]
    public ActionResult Put(int id, MyTask newTask)
    {
        var result = MyTaskService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var task = MyTaskService.GetById(id);
        if (task==null)
            return NotFound();

        MyTaskService.Delete(id);

        return NoContent();

    }
}