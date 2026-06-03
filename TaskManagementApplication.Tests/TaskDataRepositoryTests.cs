using Microsoft.EntityFrameworkCore;
using TaskManagementApplication.Data;
using TaskManagementApplication.Models;
using TaskManagementApplication.Repository;

namespace TaskManagementApplication.Tests
{
    public class TaskDataRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task SaveTask_ShouldAddAndReturnTaskWithCorrectData()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new TaskDataRepository(context);
            var newTask = new TaskData { taskTitle = "Survey Site", taskDesc = "survey the site to finalize" };

            // Act
            var result = await repository.SaveTask(newTask);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Survey Site", result.taskTitle);
            Assert.Equal(1, await context.TaskList.CountAsync());
        }

        [Fact]
        public async Task UpdateTask_WithValidTaskId_ShouldModifyFieldsSuccessfully()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var existingTask = new TaskData { taskId = 1, taskTitle = "Old Title", taskDesc = "Old Desc" };
            await context.TaskList.AddAsync(existingTask);
            await context.SaveChangesAsync();

            var repository = new TaskDataRepository(context);
            var updatedInfo = new TaskData { taskTitle = "New Title", taskDesc = "New Desc", taskStatus = true };

            // Act
            var result = await repository.UpdateTask(1, updatedInfo);

            // Assert
            Assert.Equal("New Title", result.taskTitle);
            Assert.Equal("New Desc", result.taskDesc);
            Assert.True(result.taskStatus);
        }

        [Fact]
        public async Task UpdateTask_WithInvalidTaskId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new TaskDataRepository(context);
            var updatedInfo = new TaskData { taskTitle = "New Title", taskDesc = "New Desc" };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.UpdateTask(999, updatedInfo));
        }

        [Fact]
        public async Task DeleteTask_WithValidTaskId_ShouldRemoveTaskFromDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var taskToDelete = new TaskData { taskId = 1, taskTitle = "To Be Deleted", taskDesc = "Dummy Desc" };
            await context.TaskList.AddAsync(taskToDelete);
            await context.SaveChangesAsync();

            var repository = new TaskDataRepository(context);

            // Act
            await repository.DeleteTask(1);

            // Assert
            var taskInDb = await context.TaskList.FindAsync(1);
            Assert.Null(taskInDb);
        }
    }
}