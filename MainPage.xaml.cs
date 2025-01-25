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
        Console.WriteLine($"TaskEntry.Text: {TaskEntry.Text}");
        if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
        {
            var newTask = new TaskItem
            {
                Title = TaskEntry.Text
            };
            Tasks.Add(newTask);
            TaskEntry.Text = string.Empty;

            Console.WriteLine($"Task Added: {newTask.Title}");
            await _database.SaveTaskAsync(newTask);
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
        if (sender is BindableObject bindable && bindable.BindingContext is TaskItem task)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    // Start drag
                    _draggedTask = task;
                    _draggedIndex = Tasks.IndexOf(task);
                    break;

                case GestureStatus.Completed:
                    if (_draggedTask != null)
                    {
                        // Calculate the new position
                        var targetIndex = _draggedIndex + (int)(e.TotalY / 50); // Adjust sensitivity
                        targetIndex = Math.Clamp(targetIndex, 0, Tasks.Count - 1);

                        if (targetIndex != _draggedIndex)
                        {
                            // Reorder the tasks
                            Tasks.Remove(_draggedTask);
                            Tasks.Insert(targetIndex, _draggedTask);
                        }
                    }

                    // Reset drag state
                    _draggedTask = null;
                    _draggedIndex = -1;
                    break;
            }
        }
    }

    private void OnMoveUpClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is TaskItem task)
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
        if (sender is Button button && button.CommandParameter is TaskItem task)
        {
            var index = Tasks.IndexOf(task);
            if (index < Tasks.Count - 1) // Ensure it's not the last item
            {
                Tasks.Move(index, index + 1); // Move item down
            }
        }
    }











}


