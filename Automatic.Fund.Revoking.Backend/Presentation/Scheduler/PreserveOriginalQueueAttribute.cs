﻿using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace Presentation.Scheduler
{
    public class PreserveOriginalQueueAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            var enqueuedState = context.NewState as EnqueuedState;

            // Activating only when enqueueing a background job
            if (enqueuedState != null)
            {
                // Checking if an original queue is already set
                var originalQueue = JobHelper.FromJson<string>(context.Connection.GetJobParameter(
                    context.BackgroundJob.Id,
                    "OriginalQueue"));

                if (originalQueue != null)
                {
                    // Override any other queue value that is currently set (by other filters, for example)
                    enqueuedState.Queue = originalQueue;
                }
                else
                {
                    // Queueing for the first time, we should set the original queue
                    context.Connection.SetJobParameter(
                        context.BackgroundJob.Id,
                        "OriginalQueue",
                        JobHelper.ToJson(enqueuedState.Queue));
                }
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}