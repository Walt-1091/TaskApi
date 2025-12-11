using System.Collections.Concurrent;

using CodeChallenge.Models;

namespace CodeChallenge.Data
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> GetAll();
        TaskItem? GetById(int id);
        TaskItem Add(string title);
        TaskItem? UpdateStatus(int id, bool completed);
    }

    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly ConcurrentDictionary<int, TaskItem> _tasks = new();
        private int _nextId = 0;

        public InMemoryTaskRepository()
        {
            Add("Sample task 1");
            Add("Sample task 2");
        }

        public IEnumerable<TaskItem> GetAll() => _tasks.Values.OrderBy(t => t.Id);

        public TaskItem? GetById(int id)
        {
            _tasks.TryGetValue(id, out var task);
            return task;
        }

        public TaskItem Add(string title)
        {
            var id = Interlocked.Increment(ref _nextId);

            var task = new TaskItem
            {
                Id = id,
                Title = title,
                Completed = false
            };

            _tasks[id] = task;
            return task;
        }

        public TaskItem? UpdateStatus(int id, bool completed)
        {
            return _tasks.AddOrUpdate(
                id,
                _ => null!, 
                (_, existing) =>
                {
                    existing.Completed = completed;
                    return existing;
                }
            );
        }
    }
}


