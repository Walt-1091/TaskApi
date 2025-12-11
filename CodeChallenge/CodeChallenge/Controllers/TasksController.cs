using Microsoft.AspNetCore.Mvc;
using CodeChallenge.Data;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskRepository repository, ILogger<TasksController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks()
        {
            var tasks = _repository.GetAll();
            return Ok(tasks);
        }

        [HttpPost]
        public ActionResult<TaskItem> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest(new { message = "Title is required." });
            }

            var created = _repository.Add(request.Title.Trim());

            return CreatedAtAction(nameof(GetTaskById), new { id = created.Id }, created);
        }

        [HttpGet("{id:int}")]
        public ActionResult<TaskItem> GetTaskById(int id)
        {
            var task = _repository.GetById(id);
            if (task == null)
            {
                return NotFound(new { message = $"Task with id {id} was not found." });
            }

            return Ok(task);
        }

        [HttpPut("{id:int}")]
        public ActionResult<TaskItem> UpdateTaskStatus(
            int id,
            [FromBody] UpdateTaskStatusRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body is required." });
            }

            var existing = _repository.GetById(id);
            if (existing == null)
            {
                return NotFound(new { message = $"Task with id {id} was not found." });
            }

            var updated = _repository.UpdateStatus(id, request.Completed);

            return Ok(updated);
        }
    }
}
