using System.Threading.Tasks;

namespace BufferBatchFlushSample
{
    public interface IBufferBatchFlushService
    {
        Task Add(string message);
    }
}