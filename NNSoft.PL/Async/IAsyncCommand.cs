using System.Threading.Tasks;
using System.Windows.Input;

namespace NNSoft.PL.Async
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
