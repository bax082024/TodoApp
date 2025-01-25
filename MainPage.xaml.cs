using System.Collections.ObjectModel;

namespace ToDoListApp;

public partial class MainPage : ContentPage
{
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
        foreach (var task in tasks)
        {
            Tasks.Add(task);
        }
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
        {
            var newTask = new TaskItem { Title = TaskEntry.Text };
            Tasks.Add(newTask);
            TaskEntry.Text = string.Empty;
            await _database.SaveTaskAsync(newTask);
        }
    }

    private async void OnDeleteSelectedClicked(object sender, EventArgs e)
    {
        var selectedTasks = Tasks.Where(t => t.IsChecked).ToList();
        foreach (var task in selectedTasks)
        {
            Tasks.Remove(task);
            await _database.DeleteTaskAsync(task);
        }
    }

    private void OnMoveUpClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is TaskItem task)
        {
            var index = Tasks.IndexOf(task);
            if (index > 0) // Ensure it's not the first item
            {
                Tasks.Move(index, index - 1); // Move item up
            }
        }
    }

    private void OnMoveDownClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is TaskItem task)
        {
            var index = Tasks.IndexOf(task);
            if (index < Tasks.Count - 1) // Ensure it's not the last item
            {
                Tasks.Move(index, index + 1); // Move item down
            }
        }
    }

}
















