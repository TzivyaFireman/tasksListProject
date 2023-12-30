using System.Text.Json;
using taskList.Interfaces;
using taskList.Models;

namespace taskList.Services;

public class TaskService : IMyTaskService
{
    List<MyTask> tasks { get; }
    private string FileName = "tasks.json";
    public TaskService()
    {
        this.FileName = Path.Combine( "tasks.json");
        using (var jsonFile = File.OpenText(FileName))
        {
            tasks = JsonSerializer.Deserialize<List<MyTask>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
    private void saveToFile()
    {
        File.WriteAllText(FileName, JsonSerializer.Serialize(tasks));
    }
    public List<MyTask> GetAll() => tasks;

    public MyTask GetById(int id)
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

     public void Add(MyTask newTask)
        {
            newTask.Id = tasks.Count()+1;
            tasks.Add(newTask);
            saveToFile();
        }

    public bool Update(int id, MyTask newTask)
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


    public bool Delete(int id)
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
        public int Count => tasks.Count();



}


public static class MyTaskUtils
{
    public static void AddMyTask(this IServiceCollection services)
    {
        services.AddScoped<IMyTaskService, TaskService>();
    }
}