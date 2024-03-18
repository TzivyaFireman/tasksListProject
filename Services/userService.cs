using System.Text.Json;
using taskList.Interfaces;
using taskList.Models;

namespace taskList.Services;

public class UserService : IUserService
{
    private List<User> Users { get; }
    private string FileName = "data/users.json";
    public UserService()
    {
        this.FileName = Path.Combine("data/users.json");
        using (var jsonFile = File.OpenText(FileName))
        {
            Users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),  ////////
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
    private void saveToFile()
    {
        File.WriteAllText(FileName, JsonSerializer.Serialize(Users));
    }
    public List<User> GetAll() => Users;

    public User GetById(int id)
    {
        return Users.FirstOrDefault(u => u.Id == id);
    }

    public void Add(User newUser)
    {
        newUser.Id = Users.Count() + 1;
        Users.Add(newUser);
        saveToFile();
    }

    public bool Update(int id, User newUser)
    {
        if (id != newUser.Id)
            return false;
        var existingUser = GetById(id);
        if (existingUser == null)
            return false;
        var index = Users.IndexOf(existingUser);
        if (index == -1)
            return false;
        Users[index] = newUser;
        saveToFile();

        return true;
    }


    public bool Delete(int id)
    {
        var existingUser = GetById(id);
        if (existingUser == null)
            return false;

        var index = Users.IndexOf(existingUser);
        if (index == -1)
            return false;

        Users.RemoveAt(index);
        saveToFile();

        return true;
    }
    public int Count => Users.Count();

}


// public static class UserUtils
// {
//     public static void AddUser(this IServiceCollection services)
//     {
//         services.AddScoped<ILoginService, LoginService>();
//     }
// }/////////////////