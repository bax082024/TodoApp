using System.Collections.ObjectModel;

namespace ToDoListApp;


public partial class MainPage : ContentPage
{
    // ObservableCollection to store tasks (update UI automatic)
    public ObservableCollection<TaskItem> Tasks { get; set; } = new();

    private readonly DatabaseService _database;

    public MainPage()
    {
        InitializeComponent();
        TaskList.ItemsSource = Tasks;

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tasks.db");
        _database = new DatabaseService(dbPath);       

        LoadTasksAsync();
    }

    private async void LoadTasksAsync()
    {
        var tasks = await _database.GetTasksAsync();
        Console.WriteLine($"Tasks loaded: {tasks.Count}");

        foreach (var task in tasks)
        {
            Tasks.Add(task);
            Console.WriteLine($"Loaded task: {task.Title}");
        }
    }

    // Add new task
    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
        {
            var newTask = new TaskItem { Title = TaskEntry.Text };
            Tasks.Add(newTask);
            TaskEntry.Text = string.Empty;

            var result = await _database.SaveTaskAsync(newTask);
            Console.WriteLine($"Task saved to database with result: {result}");

            // Animate scrolling to the last task
            await Task.Delay(100); // Optional delay for a smoother experience
            TaskList.ScrollTo(Tasks.Last(), position: ScrollToPosition.End, animate: true);

        }
    }


    // Delete task
    private async void OnDeleteTaskSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is TaskItem task)
        {
            Tasks.Remove(task);
            await _database.DeleteTaskAsync(task);
        }
    }


    // Mark task complete
    private void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is TaskItem task)
        {
            task.IsCompleted = e.Value; 
        }
    }
}


