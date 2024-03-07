namespace taskList.Utilities;
using taskList.Interfaces; 
using taskList.Services; 


public static class Utilities
{
    public static void AddMyTask(this IServiceCollection services)
    {
        services.AddScoped<IMyTaskService, TaskService>();
    }

    public static void AddUser(this IServiceCollection services)
    {
        services.AddScoped<ILoginService, LoginService>();
    }
}