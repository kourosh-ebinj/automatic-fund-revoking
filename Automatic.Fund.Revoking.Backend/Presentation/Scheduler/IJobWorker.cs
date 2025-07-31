using System.Threading.Tasks;

namespace Presentation.Scheduler
{
    public interface IJobWorker
    {
        Task Run();
    }
}