using Microsoft.EntityFrameworkCore;
using TaskManagementApplication.Data;
using TaskManagementApplication.Models;

namespace TaskManagementApplication.Repository
{
    public class TaskDataRepository
    {
        private readonly AppDbContext db;
        public TaskDataRepository(AppDbContext dbContext)
        {
            this.db = dbContext;
        }

        public  async Task<List<TaskData>> GetAllTasks()
        {
            return await db.TaskList.ToListAsync();
        }

        public async Task<TaskData> SaveTask(TaskData task)
        {
            await db.TaskList.AddAsync(task);
            await db.SaveChangesAsync();
            // Returns the task with the generated CreatedDate
            return task;
        }

        public async Task<TaskData> UpdateTask(int id, TaskData obj)
        {
            var taskData = await db.TaskList.FindAsync(id);
            if (taskData == null)
            {
                throw new Exception("Task Not Found");
            }
            taskData.taskTitle = obj.taskTitle;
            taskData.taskDesc = obj.taskDesc;
            taskData.taskStatus = obj.taskStatus;

            await db.SaveChangesAsync();
            // Returns the task with the newly generated UpdatedDate
            return taskData;

        }

        public async Task DeleteTask(int id)
        {
            var taskData = await db.TaskList.FindAsync(id);
            if (taskData == null)
            {
                throw new Exception("Task Not Found");
            }
            db.TaskList.Remove(taskData);
            await db.SaveChangesAsync();

        }

    }
}
