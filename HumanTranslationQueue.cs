using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LocalTravelInfo
{
    public class HumanTranslationQueue
    {
        public static HumanTranslationQueue Instance = new HumanTranslationQueue();

        private BlockingCollection<PublishingTask> queue = new BlockingCollection<PublishingTask>(new ConcurrentQueue<PublishingTask>(), 1000);

        private HumanTranslationQueue()
        {

        }

        public void Start()
        {
            Task.Run(() =>
                {
                    while (!queue.IsCompleted)
                    {
                        try
                        {
                            PublishingTask task = queue.Take();
                            ExecuteTask(task);
                        }
                        catch (InvalidOperationException) { }
                    }
                });
        }

        public void Add(PublishingTask task)
        {
            queue.Add(task);
        }

        public void Complete()
        {
            queue.CompleteAdding();
        }

        private void ExecuteTask(PublishingTask task)
        {
            PublishingService service = new PublishingService();
            service.SendHumanTranslation(task);
        }
    }
}