using taskList.Models;
namespace taskList.Services;

public static class TaskService
{
    private static List<MyTask> tasks;

    static TaskService()
    {
        tasks = new List<MyTask>
        {
            new MyTask (1, "make H.W",false),
            new MyTask (2,  "go to shopping", false),
            new MyTask (3, "sleeeeeeeep",  false)
        };
    }

    public static List<MyTask> GetAll() => tasks;

    public static MyTask GetById(int id)
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public static int Add(MyTask newTask)
    {
        if (tasks.Count == 0)
        {
            newTask.Id = 1;
        }
        else
        {
            newTask.Id = tasks.Max(t => t.Id) + 1;
        }
        tasks.Add(newTask);
        return newTask.Id;
    }

    public static bool Update(int id, MyTask newTask)
    {
        if (id != newTask.Id)
            return false;
        var existingTask = GetById(id);
        if (existingTask == null)
            return false;
        var index = tasks.IndexOf(existingTask);
        if (index == -1)
            return false;
        tasks[index] = newTask;
        return true;
    }


    public static bool Delete(int id)
    {
        var existingTask = GetById(id);
        if (existingTask == null)
            return false;

        var index = tasks.IndexOf(existingTask);
        if (index == -1)
            return false;

        tasks.RemoveAt(index);
        return true;
    }



}