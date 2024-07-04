using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Views;

namespace AvaloniaApplication2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _newTaskTitle;
        private string _newTaskDescription;
        private TaskItem _selectedTask;
        private readonly AppDbContext _dbContext;

        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set => this.RaiseAndSetIfChanged(ref _newTaskTitle, value);
        }

        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set => this.RaiseAndSetIfChanged(ref _newTaskDescription, value);
        }

        public TaskItem SelectedTask
        {
            get => _selectedTask;
            set => this.RaiseAndSetIfChanged(ref _selectedTask, value);
        }

        public ICommand AddTaskCommand { get; }
        public ICommand RemoveTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand ToggleTaskCompletionCommand { get; }

        public MainViewModel()
        {
            _dbContext = new AppDbContext();
            _dbContext.Database.EnsureCreated();
            LoadTasks();

            AddTaskCommand = ReactiveCommand.Create(AddTask);
            RemoveTaskCommand = ReactiveCommand.Create(RemoveTask);
            EditTaskCommand = ReactiveCommand.Create(EditTask);
            ToggleTaskCompletionCommand = ReactiveCommand.Create<TaskItem>(ToggleTaskCompletion);
        }

        private void LoadTasks()
        {
            var tasks = _dbContext.Tasks.ToList();
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }

        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(NewTaskTitle) && !string.IsNullOrWhiteSpace(NewTaskDescription))
            {
                var task = new TaskItem { Title = NewTaskTitle, Description = NewTaskDescription, IsCompleted = false };
                _dbContext.Tasks.Add(task);
                _dbContext.SaveChanges();

                Tasks.Add(task);
                NewTaskTitle = string.Empty;
                NewTaskDescription = string.Empty;
            }
        }

        private void RemoveTask()
        {
            if (SelectedTask != null)
            {
                _dbContext.Tasks.Remove(SelectedTask);
                _dbContext.SaveChanges();

                Tasks.Remove(SelectedTask);
            }
        }

        private async void EditTask()
        {
            if (SelectedTask != null)
            {
                var editWindow = new EditTaskWindow(new EditTaskViewModel(SelectedTask));
                var mainWindow = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                if (mainWindow != null)
                {
                    await editWindow.ShowDialog(mainWindow);
                    _dbContext.SaveChanges();
                    LoadTasks();
                }
            }
        }

        private void ToggleTaskCompletion(TaskItem task)
        {
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                _dbContext.Tasks.Update(task);
                _dbContext.SaveChanges();
            }
        }
    }
}
