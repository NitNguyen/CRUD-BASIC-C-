using FirstAPI.Data;
using FirstAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TaskController> _logger;
        public TaskController(ApplicationDbContext dbContext, ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetAllTasks(int pageNumber = 1, int pageSize = 300)
        {
            int totalTasks = _dbContext.tasks.ToList().Count;
            int totalPages = (int)Math.Ceiling((double)totalTasks / pageSize);

            List<TaskItem> pagedTasks = _dbContext.tasks.ToList()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new
            {
                TotalTasks = totalTasks,
                TotalPages = totalPages,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Tasks = pagedTasks
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _dbContext.tasks.Find(id);
            return Ok(task);
        }


        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskItem task)
        {
            try
            {
                _dbContext.tasks.Add(task); // Add the task to the DbSet
                _dbContext.SaveChanges();   // Save changes to the database
                return Ok(new { Message = "Task created successfully", Task = task });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the task.");
                return StatusCode(500, new { Message = "An error occurred while creating the task." });
            }
        }

        [HttpPut]
        public IActionResult UpdateTask([FromBody] TaskItem updatedTask)
        {
            var tasks = _dbContext.tasks.ToList();
            var task = tasks.FirstOrDefault(t => t.id == updatedTask.id);
            if (task == null)
                return NotFound();

            task.title = updatedTask.title;
            task.is_deleted = updatedTask.is_deleted;
            _dbContext.SaveChanges();      // Save changes to the database

            return Ok(new { Message = "Task Updated successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _dbContext.tasks.Find(id); // Find the task by its ID
            if (task == null)
                return NotFound();

            _dbContext.tasks.Remove(task); // Remove the task from the DbSet
            _dbContext.SaveChanges();      // Save changes to the database

            return Ok(new { Message = "Task Deleted successfully" });
        }
    }
}
