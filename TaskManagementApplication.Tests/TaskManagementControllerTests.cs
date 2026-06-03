using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementApplication.Controllers;
using TaskManagementApplication.Models;
using TaskManagementApplication.Repository;

namespace TaskManagementApplication.Tests
{
    public class TaskManagementControllerTests
    {
        private readonly Mock<ITaskDataRepository> _mockRepo;
        private readonly TaskManagementController _controller;

        public TaskManagementControllerTests()
        {
            _mockRepo = new Mock<ITaskDataRepository>();
            _controller = new TaskManagementController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllTasks_ShouldReturnOkWithListOfTasks()
        {
            // Arrange
            var taskList = new List<TaskData> { new TaskData { taskId = 1, taskTitle = "Clearing and Layout" } };
            _mockRepo.Setup(repo => repo.GetAllTasks()).ReturnsAsync(taskList);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsType<List<TaskData>>(okResult.Value);
            Assert.Single(returnedTasks);
        }

        [Fact]
        public async Task AddTask_ShouldReturnOkWithCreatedTask()
        {
            // Arrange
            var inputTask = new TaskData { taskTitle = "Clearing and Layout" };
            var outputTask = new TaskData { taskId = 1, taskTitle = "Clearing and Layout" };
            _mockRepo.Setup(repo => repo.SaveTask(inputTask)).ReturnsAsync(outputTask);

            // Act
            var result = await _controller.AddTask(inputTask);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTask = Assert.IsType<TaskData>(okResult.Value);
            Assert.Equal(1, returnedTask.taskId);
        }

        [Fact]
        public async Task UpdateTask_ShouldReturnOkWithUpdatedTaskObject()
        {
            // Arrange
            var inputTask = new TaskData { taskTitle = "Updated Task" };
            _mockRepo.Setup(repo => repo.UpdateTask(1, inputTask)).ReturnsAsync(inputTask);

            // Act
            var result = await _controller.updateTask(1, inputTask);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(inputTask, okResult.Value);
        }

        [Fact]
        public async Task DeleteTask_ShouldReturnOkResultOnSuccess()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteTask(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.deleteTask(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}