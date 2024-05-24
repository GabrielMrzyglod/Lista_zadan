using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using AvaloniaApplication2.Models;

namespace AvaloniaApplication2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _newTaskDescription;
        private TaskItem _selectedTask;

        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

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

        public MainViewModel()
        {
            AddTaskCommand = ReactiveCommand.Create(AddTask);
            RemoveTaskCommand = ReactiveCommand.Create(RemoveTask);
        }

        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(NewTaskDescription))
            {
                Tasks.Add(new TaskItem { Description = NewTaskDescription });
                NewTaskDescription = string.Empty;
            }
        }

        private void RemoveTask()
        {
            if (SelectedTask != null)
            {
                Tasks.Remove(SelectedTask);
            }
        }
    }
}
