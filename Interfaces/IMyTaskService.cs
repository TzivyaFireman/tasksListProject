using System.Reflection.Metadata.Ecma335;
using taskList.Models;
using taskList.Services;

namespace taskList.Interfaces;

public interface IMyTaskService
{
    List<MyTask> GetAll();
    MyTask GetById(int id);
    void Add(MyTask newTask);
    bool Update(int id, MyTask newTask);

    bool Delete(int id);

}