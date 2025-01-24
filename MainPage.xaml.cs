using System.Collections.ObjectModel;

namespace ToDoListApp;


public partial class MainPage : ContentPage
{
    // ObservableCollection to store tasks (update UI automatic)
    public ObservableCollection<TaskItem> Tasks { get; set; } = new();

    private readonly DatabaseService _database;

    private TaskItem _draggedTask;
    private int _draggedIndex;

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
        if (!string.IsNullOrWhiteSpace(TaskEntry.Text) && PriorityPicker.SelectedItem != null)
        {
            var newTask = new TaskItem
            {
                Title = TaskEntry.Text,
                Priority = PriorityPicker.SelectedItem.ToString()
            };
            Tasks.Add(newTask);
            TaskEntry.Text = string.Empty;

            await _database.SaveTaskAsync(newTask);

            SortTasks();
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

    private void SortTasks()
    {
        var sortedTasks = Tasks.OrderBy(task => task.Priority).ToList();
        Tasks.Clear();
        foreach (var task in sortedTasks)
        {
            Tasks.Add(task);
        }
    }

    private void OnDragStarting(object sender, DragStartingEventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is TaskItem task)
        {
            _draggedTask = task;
            _draggedIndex = Tasks.IndexOf(task);
        }
    }


    private void OnDropCompleted(object sender, DropCompletedEventArgs e)
    {
        if (_draggedTask != null)
        {
            // Ensure valid indices
            var targetTask = (sender as BindableObject)?.BindingContext as TaskItem;
            if (targetTask != null)
            {
                var targetIndex = Tasks.IndexOf(targetTask);
                if (targetIndex >= 0 && _draggedIndex != targetIndex)
                {
                    Tasks.Remove(_draggedTask);
                    Tasks.Insert(targetIndex, _draggedTask);
                }
            }
            // Reset drag state
            _draggedTask = null;
            _draggedIndex = -1;
        }
    }


    private void OnItemPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started && sender is BindableObject bindable && bindable.BindingContext is TaskItem task)
        {
            _draggedTask = task;
            _draggedIndex = Tasks.IndexOf(task);
        }
        else if (e.StatusType == GestureStatus.Completed)
        {
            // Handle drop
            var targetTask = (sender as BindableObject)?.BindingContext as TaskItem;
            if (_draggedTask != null && targetTask != null)
            {
                var targetIndex = Tasks.IndexOf(targetTask);
                if (_draggedIndex >= 0 && targetIndex >= 0 && _draggedIndex != targetIndex)
                {
                    Tasks.Remove(_draggedTask);
                    Tasks.Insert(targetIndex, _draggedTask);
                }
            }
            // Reset state
            _draggedTask = null;
            _draggedIndex = -1;
        }
    }





}


