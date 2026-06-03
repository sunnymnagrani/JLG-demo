using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApplication.Models;
using TaskManagementApplication.Repository;

namespace TaskManagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagementController : ControllerBase
    {
        private readonly TaskDataRepository taskDataRepository;
        public TaskManagementController(TaskDataRepository taskDataRepository)
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
        public async Task<ActionResult> updateTask(int id, [FromBody] TaskData td)
        {
            var updatedTask = await taskDataRepository.UpdateTask(id, td);

            if (updatedTask == null)
            {
                return NotFound();
            }

            // Return the fresh database state back to Angular
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
