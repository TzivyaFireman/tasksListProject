namespace taskList.Utilities;
using taskList.Interfaces; 
using taskList.Services; 


public static class Utilities
{
    public static void AddMyTask(this IServiceCollection services)
    {
        services.AddSingleton<IMyTaskService, TaskService>();
    }

    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
    }
    public static void AddToken(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
    }
}