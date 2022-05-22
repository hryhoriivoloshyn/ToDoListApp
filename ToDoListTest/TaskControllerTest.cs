using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ToDoListApp.Controllers;
using ToDoListApp.Dtos;
using ToDoListApp.Models;
using ToDoListApp.Service;

namespace ToDoListTest
{

    public class TaskControllerTest
    {
        private TasksController _controller;
        private Mock<ITaskService> _taskServiceMock;

        private Mock<ServiceResponse<List<TaskDto>>> _taskListServiceResponseMock;
        private Mock<ServiceResponse<TaskDto>> _taskServiceResponseMock;
        private ServiceResponse<TaskDto> _taskServiceResponse;

        private Mock<TaskDto> _taskDtoMock;
        private Mock<CreateTaskDto> _createTaskDtoMock;
        private Mock<UpdateTaskDto> _updateTaskDtoMock;


        [SetUp]
        public void Setup()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _taskListServiceResponseMock = new Mock<ServiceResponse<List<TaskDto>>>();
            _taskDtoMock = new Mock<TaskDto>();
            _createTaskDtoMock = new Mock<CreateTaskDto>();
            _taskServiceResponseMock = new Mock<ServiceResponse<TaskDto>>();
            _updateTaskDtoMock = new Mock<UpdateTaskDto>();

            _controller = new TasksController(_taskServiceMock.Object);
        }

        [Test]
        public void GetAll_AllTasks_ReturnsOk()
        {
            //Arrange
            _taskListServiceResponseMock.SetupAllProperties();
            _taskListServiceResponseMock.Object.Data = new List<TaskDto>
            {
                _taskDtoMock.Object
            };
            _taskServiceMock.Setup(t => t.GetTasks()).Returns(_taskListServiceResponseMock.Object);

            //Act
            var result = _controller.GetAll();
            var tasks = (result as ObjectResult).Value as List<TaskDto>;


            //Assert
            Assert.NotNull(tasks);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetTaskById_ExistingTask_ReturnsOk()
        {
            //Arrange
            const int taskId = 1;
            _taskServiceResponse = new ServiceResponse<TaskDto>
            {
                Data = _taskDtoMock.Object,
                ErrorMessages = null,
                Message = StatusEnum.OK,
                Success = true
            };
            _taskServiceMock.Setup(t => t.GetById(taskId)).Returns(_taskServiceResponse);


            //Act
            var result = _controller.GetTaskById(taskId);
            var task = (result as ObjectResult).Value as TaskDto;

            //Assert
            Assert.NotNull(task);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetTaskById_TaskByNegativeId_ReturnsBadRequest()
        {
            //Arrange
            const int taskId = -1;

            //Act
            var result = _controller.GetTaskById(taskId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void GetTaskById_TaskByZeroId_ReturnsBadRequest()
        {
            //Arrange
            const int taskId = 0;


            //Act
            var result = _controller.GetTaskById(taskId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void GetTaskById_NotExistingTask_ReturnsNotFound()
        {
            //Arrange
            const int taskId = 1;
            ServiceResponse<TaskDto> _taskServiceResponse = new ServiceResponse<TaskDto>
            {
                Data = null,
                ErrorMessages = null,
                Message = StatusEnum.NotFound,
                Success = false
            };
            _taskServiceMock.Setup(t => t.GetById(taskId)).Returns(_taskServiceResponse);

            //Act
            var result = _controller.GetTaskById(taskId);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Post_Task_ReturnsOk()
        {
            //Arrange
            _taskServiceMock.Setup(s => s.AddTask(_createTaskDtoMock.Object)).Returns(_taskServiceResponseMock.Object);

            //Act
            var result = _controller.Post(_createTaskDtoMock.Object);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void Post_NullTask_ReturnsBadRequest()
        {
            //Arrange

            //Act
            var result = _controller.Post(null);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Post_RepositoryError_Returns500Response()
        {
            //Arrange
            ServiceResponse<TaskDto> erroResponse = new ServiceResponse<TaskDto>()
            {
                Data = null,
                Success = false,
                Message = StatusEnum.RepositoryError,
                ErrorMessages = null
            };

            _taskServiceMock.Setup(s => s.AddTask(_createTaskDtoMock.Object)).Returns(erroResponse);

            var expected = StatusCodes.Status500InternalServerError;

            //Act
            var result = _controller.Post(_createTaskDtoMock.Object);

            var actual = (result as ObjectResult).StatusCode;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Post_ServiceError_Returns500Response()
        {
            //Arrange
            _taskServiceResponseMock.SetupAllProperties();
            _taskServiceResponseMock.Object.Success = false;
            _taskServiceResponseMock.Object.Message = StatusEnum.Error;


            _taskServiceMock.Setup(s => s.AddTask(_createTaskDtoMock.Object)).Returns(_taskServiceResponseMock.Object);

            var expected = StatusCodes.Status500InternalServerError;

            //Act
            var result = _controller.Post(_createTaskDtoMock.Object);

            var actual = (result as ObjectResult).StatusCode;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpdateTask_ExistingTask_ReturnsOK()
        {
            //Arrange
            const int taskId = 1;
            _updateTaskDtoMock.SetupAllProperties();
            _updateTaskDtoMock.Object.Id = taskId;
            _taskServiceMock.Setup(s => s.UpdateTask(_updateTaskDtoMock.Object)).Returns(_taskServiceResponseMock.Object);

            //Act
            var result = _controller.UpdateTask(taskId, _updateTaskDtoMock.Object);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void UpdateTask_NotSameTaskIds_ReturnsBadRequest()
        {
            //Arrange
            const int taskId = 1;
            _updateTaskDtoMock.SetupAllProperties();
            _updateTaskDtoMock.Object.Id = 2;

            //Act
            var result = _controller.UpdateTask(taskId, _updateTaskDtoMock.Object);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void UpdateTask_TaskNotFound_ReturnsNotFound()
        {
            //Arrange
            const int taskId = 1;
            _updateTaskDtoMock.SetupAllProperties();
            _updateTaskDtoMock.Object.Id = taskId;
            _taskServiceResponseMock.SetupAllProperties();
            _taskServiceResponseMock.Object.Success = false;
            _taskServiceResponseMock.Object.Message = StatusEnum.NotFound;
            _taskServiceMock.Setup(s => s.UpdateTask(_updateTaskDtoMock.Object)).Returns(_taskServiceResponseMock.Object);


            //Act
            var result = _controller.UpdateTask(taskId, _updateTaskDtoMock.Object);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void UpdateTask_RepositoryError_Returns500Status()
        {
            //Arrange
            const int taskId = 1;
            _updateTaskDtoMock.SetupAllProperties();
            _updateTaskDtoMock.Object.Id = taskId;
            _taskServiceResponseMock.SetupAllProperties();
            _taskServiceResponseMock.Object.Success = false;
            _taskServiceResponseMock.Object.Message = StatusEnum.RepositoryError;
            _taskServiceMock.Setup(s => s.UpdateTask(_updateTaskDtoMock.Object)).Returns(_taskServiceResponseMock.Object);

            var expected = StatusCodes.Status500InternalServerError;
            //Act
            var result = _controller.UpdateTask(taskId, _updateTaskDtoMock.Object);
            var actual = (result as ObjectResult).StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpdateTask_ServiceError_Returns500Status()
        {
            //Arrange
            const int taskId = 1;
            _updateTaskDtoMock.SetupAllProperties();
            _updateTaskDtoMock.Object.Id = taskId;
            _taskServiceResponseMock.SetupAllProperties();
            _taskServiceResponseMock.Object.Success = false;
            _taskServiceResponseMock.Object.Message = StatusEnum.Error;
            _taskServiceMock.Setup(s => s.UpdateTask(_updateTaskDtoMock.Object)).Returns(_taskServiceResponseMock.Object);

            var expected = StatusCodes.Status500InternalServerError;
            //Act
            var result = _controller.UpdateTask(taskId, _updateTaskDtoMock.Object);
            var actual = (result as ObjectResult).StatusCode;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DeleteTask_ExistingTask_ReturnsOK()
        {
            //Arrange
            const int taskId = 1;
            
            _taskServiceMock.Setup(s => s.DeleteTask(taskId)).Returns(_taskServiceResponseMock.Object);

            //Act
            var result = _controller.DeleteTask(taskId);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void DeleteTask_TaskNotFound_ReturnsNotFound()
        {
            //Arrange
            const int taskId = 1;
            _taskServiceResponseMock.SetupAllProperties();
            _taskServiceResponseMock.Object.Success = false;
            _taskServiceResponseMock.Object.Message = StatusEnum.NotFound;
            _taskServiceMock.Setup(s => s.DeleteTask(taskId)).Returns(_taskServiceResponseMock.Object);


            //Act
            var result = _controller.DeleteTask(taskId);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void DeleteTask_RepositoryError_Returns500Status()
        {
            //Arrange
            const int taskId = 1;
            _taskServiceResponseMock.SetupAllProperties();
            _taskServiceResponseMock.Object.Success = false;
            _taskServiceResponseMock.Object.Message = StatusEnum.RepositoryError;
            _taskServiceMock.Setup(s => s.DeleteTask(taskId)).Returns(_taskServiceResponseMock.Object);

            var expected = StatusCodes.Status500InternalServerError;
            //Act
            var result = _controller.DeleteTask(taskId);
            var actual = (result as ObjectResult).StatusCode;
            //Assert
            Assert.AreEqual(expected,actual);
        }

        [Test]
        public void DeleteTask_ServiceError_Returns500Status()
        {
            //Arrange
            const int taskId = 1;
            _taskServiceResponseMock.SetupAllProperties();
            _taskServiceResponseMock.Object.Success = false;
            _taskServiceResponseMock.Object.Message = StatusEnum.Error;
            _taskServiceMock.Setup(s => s.DeleteTask(taskId)).Returns(_taskServiceResponseMock.Object);

            var expected = StatusCodes.Status500InternalServerError;
            //Act
            var result = _controller.DeleteTask(taskId);
            var actual = (result as ObjectResult).StatusCode;
            //Assert
            Assert.AreEqual(expected, actual);
        }

    }
}

