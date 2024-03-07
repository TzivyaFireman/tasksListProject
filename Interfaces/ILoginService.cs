using System.Reflection.Metadata.Ecma335;
using taskList.Models;
using taskList.Services;

namespace taskList.Interfaces;

public interface ILoginService
{
    List<User> GetAll();
    User GetById(int id);
    void Add(User newUser);
    bool Update(int id, User newUser);
    bool Delete(int id);

}