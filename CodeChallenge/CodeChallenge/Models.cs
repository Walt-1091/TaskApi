namespace CodeChallenge.Models
{
    public class TaskItem
    {
        public int Id { get; set; } 
        public string Title { get; set; } = "";
        public bool Completed { get; set; }
    }

    public class CreateTaskRequest
    {
        public string Title { get; set; } = "";
    }

    public class UpdateTaskStatusRequest
    {
        public bool Completed { get; set; }
    }
}
