using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaApplication2.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AvaloniaApplication2.Views
{
    public partial class EditTaskWindow : ReactiveWindow<EditTaskViewModel>
    {
        public EditTaskWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public EditTaskWindow(EditTaskViewModel viewModel) : this()
        {
            DataContext = viewModel;
            this.WhenActivated(disposables =>
            {
                viewModel.Close.RegisterHandler(interaction =>
                {
                    interaction.SetOutput(Unit.Default);
                    Close();
                    return Task.CompletedTask;



                }).DisposeWith(disposables);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CloseButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close();
        }
    }
}
