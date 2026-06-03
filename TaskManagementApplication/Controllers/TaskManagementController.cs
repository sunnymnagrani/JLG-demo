using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskManagementApplication.Models;
using TaskManagementApplication.Repository;

namespace TaskManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {
        private readonly ITaskDataRepository taskDataRepository;
        public TaskManagementController(ITaskDataRepository taskDataRepository)
        {
            this.taskDataRepository = taskDataRepository;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await taskDataRepository.GetAllTasks();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskData task)
        {
            var createdTask = await taskDataRepository.SaveTask(task);
            return Ok(createdTask);
        }

        [HttpPut("{id}")]
        [DebuggerStepThrough]
        public async Task<ActionResult> updateTask(int id, [FromBody] TaskData td)
        {
            var updatedTask = await taskDataRepository.UpdateTask(id, td);

            // Return the fresh database state back to frontend
            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> deleteTask(int id)
        {
            await taskDataRepository.DeleteTask(id);
            return Ok();
        }
    }
}
