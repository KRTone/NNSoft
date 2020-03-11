using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NNSoft.PL.Async
{
    public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<CancellationToken, object, Task<TResult>> command;
        private readonly CancelAsyncCommand cancelCommand;
        private NotifyTaskCompletion<TResult> execution;
        private readonly Predicate<object> canExecute;

        public AsyncCommand(Func<CancellationToken, object, Task<TResult>> command) : this(command, null) { }

        public AsyncCommand(Func<CancellationToken, object, Task<TResult>> command, Predicate<object> canExecute)
        {
            this.command = command;
            this.canExecute = canExecute;
            cancelCommand = new CancelAsyncCommand();
        }

        public override bool CanExecute(object parameter)
        {
            return (Execution == null || Execution.IsCompleted) && (canExecute == null ? true : canExecute(parameter));
        }

        public override async Task ExecuteAsync(object parameter)
        {
            cancelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<TResult>(command(cancelCommand.Token, parameter));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            cancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
        }

        public NotifyTaskCompletion<TResult> Execution
        {
            get { return execution; }
            private set
            {
                execution = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource cts = new CancellationTokenSource();
            private bool commandExecuting;

            public CancellationToken Token { get { return cts.Token; } }

            public void NotifyCommandStarting()
            {
                commandExecuting = true;
                if (!cts.IsCancellationRequested)
                    return;
                cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter)
            {
                return commandExecuting && !cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                cts.Cancel();
                RaiseCanExecuteChanged();
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            private void RaiseCanExecuteChanged()
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
