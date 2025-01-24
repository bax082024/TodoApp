using SQLite;

namespace ToDoListApp
{
    public class TaskItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        public string Priority { get; set; } = "Medium";
    }

}
