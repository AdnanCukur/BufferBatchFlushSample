using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BufferBatchFlushSample
{
    public class BufferBatchFlushService : IBufferBatchFlushService
    {
        private List<string> messages;
        private ILogger<BufferBatchFlushService> logger;
        private Timer timer;

        /// <summary> Maximum messages to be contained before flushing </summary>
        private int MaxMessages = 100;

        /// <summary> Flush after a period of inactivity </summary>
        private TimeSpan FlushAfter = TimeSpan.FromSeconds(5);

        /// <summary>Not needed just used for testing some stuff</summary>
        private int total;

        public BufferBatchFlushService(IApplicationLifetime applicationLifetime, ILogger<BufferBatchFlushService> logger)
        {
            this.messages = new List<string>();
            // Flush on application shutdown
            applicationLifetime.ApplicationStopped.Register(Flush);
            this.logger = logger;
            // Timer for inactivity flush
            timer = new Timer((e) =>
            {
                lock (this.messages)
                {
                    this.logger.LogWarning("Flushing because of inactivity");
                    this.Flush();
                    
                }
            });
        }

        public Task Add(string message)
        {
            lock (this.messages)
            {
                if (this.messages.Count >= this.MaxMessages)
                {
                    this.logger.LogInformation("Over 100 messages in, flushing list: {0}", this.messages.Count);
                    this.Flush();
                }
                this.messages.Add(message);
                this.logger.LogInformation("Adding new message: {0}", this.messages.Count);
                // Change timer to expire in 5 seconds, dont repeat.
                this.timer.Change(this.FlushAfter, TimeSpan.Zero); 
                total++;
            }

            return null;
        }

        private void Flush()
        {
            this.logger.LogInformation("Flushing list {0}", this.messages.Count);
            this.messages.Clear();
            this.logger.LogInformation("total {0}", this.total);
        }
    }
}
