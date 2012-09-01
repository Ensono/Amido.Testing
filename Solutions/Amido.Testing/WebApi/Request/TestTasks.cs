using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amido.Testing.Dbc;

namespace Amido.Testing.WebApi.Request
{
    /// <summary>
    /// A collection of tasks to be run in parrallel. 
    /// </summary>
    public class TestTasks
    {
        internal List<Task> Tasks { get; set; }

        /// <summary>
        /// Constructs a new TestTasks list.
        /// </summary>
        public TestTasks()
        {
            Tasks = new List<Task>();
        }

        /// <summary>
        /// Adds a new task to the collection.
        /// </summary>
        /// <param name="task">The action to run.</param>
        /// <returns>Returns an instance of the current <see cref="TestTasks"/>.</returns>
        public TestTasks Add(Action task)
        {
            Contract.Requires(task != null, "The task cannot be null.");

            Tasks.Add(Task.Factory.StartNew(task));
            return this;
        }

        /// <summary>
        /// Forces the main thread to wait until all tasks in the collection have completed.
        /// </summary>
        public void Wait()
        {
            Task.WaitAll(Tasks.ToArray());
        }
    }
}
