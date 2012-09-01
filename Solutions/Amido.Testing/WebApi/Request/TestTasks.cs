using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.Request
{
    public class TestTasks
    {
        internal List<Task> Tasks { get; set; }

        public TestTasks()
        {
            Tasks = new List<Task>();
        }

        public TestTasks Add(Action task)
        {
            Contract.Requires(task != null, "The task cannot be null.");

            Tasks.Add(Task.Factory.StartNew(task));
            return this;
        }

        public void Wait()
        {
            Task.WaitAll(Tasks.ToArray());
        }
    }
}
