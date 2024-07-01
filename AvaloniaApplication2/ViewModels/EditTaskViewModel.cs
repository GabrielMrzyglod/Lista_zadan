using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using AvaloniaApplication2.Models;

namespace AvaloniaApplication2.ViewModels
{
    public class EditTaskViewModel : ViewModelBase
    {
        private string _taskTitle;
        private string _taskDescription;

        public EditTaskViewModel(TaskItem task)
        {
            Task = task;
            _taskTitle = task.Title;
            _taskDescription = task.Description;
            SaveCommand = ReactiveCommand.Create(Save);
            Close = new Interaction<Unit, Unit>();
        }

        public TaskItem Task { get; }

        public string TaskTitle
        {
            get => _taskTitle;
            set => this.RaiseAndSetIfChanged(ref _taskTitle, value);
        }

        public string TaskDescription
        {
            get => _taskDescription;
            set => this.RaiseAndSetIfChanged(ref _taskDescription, value);
        }

        public ICommand SaveCommand { get; }
        public Interaction<Unit, Unit> Close { get; }

        private void Save()
        {
            Task.Title = TaskTitle;
            Task.Description = TaskDescription;
            Close.Handle(Unit.Default);
        }
    }
}
