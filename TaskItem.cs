using SQLite;

namespace ToDoListApp
{
    public class TaskItem
    {
        public string Title { get; set; } = String.Empty; 
        public string Description { get; set; } = String.Empty;
        public bool IsChecked { get; set; }
    }


}
