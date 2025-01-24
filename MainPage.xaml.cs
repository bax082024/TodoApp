using System.Collections.ObjectModel;

namespace ToDoListApp;

public partial class MainPage : ContentPage
{
    // ObservableCollection to store tasks (update UI automatic)
    public ObservableCollection<TaskItem> Tasks { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        TaskList.ItemsSource = Tasks;
    }

    // Add new task
    private void OnAddTaskClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
        {
            Tasks.Add(new TaskItem { Title = TaskEntry.Text });
            TaskEntry.Text = string.Empty;
        }
    }

    // Delete task
    private void OnDeleteTaskSwiped(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is TaskItem task)
        {
            Tasks.Remove(task);
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

public class TaskItem
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
