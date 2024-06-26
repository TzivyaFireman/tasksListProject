using System.Text.Json;
using taskList.Interfaces;
using taskList.Models;
using System;

namespace taskList.Services;

public class TaskService : IMyTaskService
{
    List<MyTask> tasks { get; set; }
    private string FileName = "data/tasks.json";
    public TaskService()
    {
        this.FileName = Path.Combine("data/tasks.json");
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

    public List<MyTask> GetAll(int userId)
    {
        return new List<MyTask>(tasks.Where(t => t.Owner == userId));
    }

    public MyTask GetById(int id)
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public void Add(MyTask newTask)
    {
        newTask.Id = tasks.Max(t=> t.Id) + 1;
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
        saveToFile();

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
        saveToFile();

        return true;
    }

    public void DeleteByUserID(int id)
    {
        tasks = new List<MyTask>(tasks.Where(t => t.Owner != id));
        saveToFile();
    }
    public int Count => tasks.Count();

}

