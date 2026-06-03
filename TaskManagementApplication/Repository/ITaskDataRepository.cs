using TaskManagementApplication.Models;

namespace TaskManagementApplication.Repository
{
    public interface ITaskDataRepository
    {
        Task<List<TaskData>> GetAllTasks();
        Task<TaskData> SaveTask(TaskData task);
        Task<TaskData> UpdateTask(int id, TaskData obj);
        Task DeleteTask(int id);
    }
}
