using System.Reflection.Metadata.Ecma335;
using taskList.Models;
using taskList.Services;

namespace taskList.Interfaces;

public interface IMyTaskService
{
    public List<MyTask> GetAll(int userId);
    MyTask GetById(int id);
    void Add(MyTask newTask);
    bool Update(int id, MyTask newTask);

    bool Delete(int id);
    void DeleteByUserID(int id);
    //public bool chekAuthorization(int taskId);
}