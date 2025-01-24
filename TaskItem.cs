using SQLite;

namespace ToDoListApp
{
    public class TaskItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Unique identifier for database

        public string Title { get; set; } = string.Empty; // Task name

        public bool IsCompleted { get; set; } // Whether the task is completed
    }
}
