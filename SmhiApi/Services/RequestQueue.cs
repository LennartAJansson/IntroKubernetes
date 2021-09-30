
using System.Collections.Concurrent;

namespace SmhiApi.Services
{
    public class RequestQueue
    {
        private ConcurrentQueue<Request> queue;

        public ConcurrentQueue<Request> Queue
        {
            get
            {
                if (queue == null)
                {
                    queue = new ConcurrentQueue<Request>();
                }

                return queue;
            }
        }
    }
}
