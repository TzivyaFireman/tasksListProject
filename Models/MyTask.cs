namespace taskList.Models;
public class MyTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDone { get; set; }
    public int Owner { get; set; }
    public MyTask(int id,string name, bool isDone,int Owner)
    {
        Id=id;
        Name = name;
        IsDone = isDone;
        this.Owner = Owner;
    }
}