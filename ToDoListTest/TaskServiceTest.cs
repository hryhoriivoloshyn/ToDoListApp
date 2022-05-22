using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using ToDoListApp.Controllers;
using ToDoListApp.Data;
using ToDoListApp.Dtos;
using ToDoListApp.Models;
using ToDoListApp.Service;

namespace ToDoListTest
{
    public class TaskServiceTest
    {
        private ITaskService service;
        private Mock<ITaskRepository> repositoryMock;
        private Mock<IMapper> mapperMock;

        private Mock<ToDoTask> taskMock;
        private List<ToDoTask> tasks;

        private Mock<TaskDto> taskDtoMock;
        private List<TaskDto> taskDtoList;
        private Mock<CreateTaskDto> createTaskDtoMock;
        private Mock<UpdateTaskDto> updateTaskDtoMock;

        private ToDoTask task;
        private List<string> ErrorMessages;
        private Exception exception;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<ITaskRepository>();
            mapperMock = new Mock<IMapper>();
            taskMock = new Mock<ToDoTask>();
            taskDtoMock = new Mock<TaskDto>();
            createTaskDtoMock = new Mock<CreateTaskDto>();
            updateTaskDtoMock = new Mock<UpdateTaskDto>();
            service = new TaskService(repositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public void GetTasks_AllTasks_ReturnsSuccessfulResponse()
        {
            //Arrange
            tasks = new List<ToDoTask>
            {
                taskMock.Object
            };

            repositoryMock.Setup(r => r.GetAllTasks()).Returns(tasks.AsEnumerable());
            
            taskDtoList = new List<TaskDto>
            {
                taskDtoMock.Object
            };

            mapperMock.Setup(m => m.Map<List<ToDoTask>, List<TaskDto>>(tasks)).Returns(taskDtoList);

            //Act
            var result = service.GetTasks();

            //Assert
            Assert.AreEqual(taskDtoList, result.Data);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(StatusEnum.OK, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void GetTasks_Exception_ReturnsError()
        {
            //Arrange
            exception = new Exception();

            ErrorMessages = new List<string> { Convert.ToString(exception.Message) };

            repositoryMock.Setup(r => r.GetAllTasks()).Throws(exception);

            //Act
            var result = service.GetTasks();

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.Error, result.Message);
            Assert.AreEqual(ErrorMessages, result.ErrorMessages);
        }

        [Test]
        public void GetById_ExistingTask_ReturnsSuccessfulResponse()
        {
            const int taskId = 1;
            //Arrange
            repositoryMock.Setup(r => r.GetTaskById(taskId)).Returns(taskMock.Object);
            mapperMock.Setup(m => m.Map<TaskDto>(taskMock.Object)).Returns(taskDtoMock.Object);
            

            //Act
            var result = service.GetById(taskId);

            //Assert
            Assert.AreEqual(taskDtoMock.Object, result.Data);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(StatusEnum.OK, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);

        }

        [Test]
        public void GetById_NotExistingTask_ReturnsNotFound()
        {

            //Arrange
            const int taskId = 1;
            task = null;

            repositoryMock.Setup(r => r.GetTaskById(taskId)).Returns(task);
            

            //Act
            var result = service.GetById(taskId);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.NotFound, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void GetById_Exception_ReturnsError()
        {
            //Arrange
            const int taskId = -1;
            Exception exception = new IndexOutOfRangeException();
            ErrorMessages = new List<string> { Convert.ToString(exception.Message) };

            repositoryMock.Setup(r => r.GetTaskById(taskId)).Throws(exception);
            

            //Act
            var result = service.GetById(taskId);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.Error, result.Message);
            Assert.AreEqual(ErrorMessages, result.ErrorMessages);
        }

        [Test]
        public void AddTask_NewTask_ReturnsSuccessfulResponse()
        {
            //Arrange
            mapperMock.Setup(m => m.Map<ToDoTask>(createTaskDtoMock.Object)).Returns(taskMock.Object);

            repositoryMock.Setup(r => r.InsertTask(taskMock.Object)).Returns(true);

            mapperMock.Setup(m => m.Map<TaskDto>(taskMock.Object)).Returns(taskDtoMock.Object);

            

            //Act
            var result = service.AddTask(createTaskDtoMock.Object);

            //Assert
            Assert.AreEqual(taskDtoMock.Object, result.Data);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(StatusEnum.OK, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void AddTask_Exception_ReturnsError()
        {
            //Arrange
            exception = new Exception();
            ErrorMessages = new List<string> { Convert.ToString(exception.Message) };

            mapperMock.Setup(m => m.Map<ToDoTask>(createTaskDtoMock.Object)).Throws(exception);

            repositoryMock.Setup(r => r.InsertTask(taskMock.Object)).Returns(true);

            //Act
            var result = service.AddTask(createTaskDtoMock.Object);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.Error, result.Message);
            Assert.AreEqual(ErrorMessages, result.ErrorMessages);
        }

        [Test]
        public void AddTask_RepositoryError_ReturnsError()
        {
            //Arrange
            mapperMock.Setup(m => m.Map<ToDoTask>(createTaskDtoMock.Object)).Returns(taskMock.Object);

            repositoryMock.Setup(r => r.InsertTask(taskMock.Object)).Returns(false);

            //Act
            var result = service.AddTask(createTaskDtoMock.Object);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.RepositoryError, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void UpdateTask_ExistingTask_ReturnsSuccessfulResponse()
        {
            //Arrange
            repositoryMock.Setup(r => r.GetTaskById(updateTaskDtoMock.Object.Id)).Returns(taskMock.Object);

            mapperMock.Setup(m => m.Map(updateTaskDtoMock.Object, taskMock.Object)).Returns(taskMock.Object);

            repositoryMock.Setup(r => r.UpdateTask(taskMock.Object)).Returns(true);

            mapperMock.Setup(m => m.Map<TaskDto>(taskMock.Object)).Returns(taskDtoMock.Object);

            

            //Act
            var result = service.UpdateTask(updateTaskDtoMock.Object);

            //Assert
            Assert.AreEqual(taskDtoMock.Object, result.Data);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(StatusEnum.OK, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void UpdateTask_NotExistingTask_ReturnsNotFound()
        {
            //Arrange
            task = null;
            repositoryMock.Setup(r => r.GetTaskById(updateTaskDtoMock.Object.Id)).Returns(task);

            

            //Act
            var result = service.UpdateTask(updateTaskDtoMock.Object);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.NotFound, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void UpdateTask_RepositoryError_ReturnsRepositoryError()
        {
            //Arrange
            repositoryMock.Setup(r => r.GetTaskById(updateTaskDtoMock.Object.Id)).Returns(taskMock.Object);

            mapperMock.Setup(m => m.Map<ToDoTask>(updateTaskDtoMock.Object)).Returns(taskMock.Object);

            repositoryMock.Setup(r => r.UpdateTask(taskMock.Object)).Returns(false);

            //Act
            var result = service.UpdateTask(updateTaskDtoMock.Object);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.RepositoryError, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void UpdateTask_Exception_ReturnsError()
        {
            //Arrange
            exception = new Exception();
            ErrorMessages = new List<string> { Convert.ToString(exception.Message) };

            repositoryMock.Setup(r => r.GetTaskById(updateTaskDtoMock.Object.Id)).Returns(taskMock.Object);

            mapperMock.Setup(m => m.Map(updateTaskDtoMock.Object, taskMock.Object)).Throws(exception);

            //Act
            var result = service.UpdateTask(updateTaskDtoMock.Object);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.Error, result.Message);
            Assert.AreEqual(ErrorMessages, result.ErrorMessages);
        }

        [Test]
        public void DeleteTask_ExistingTask_ReturnsSuccessfulResponse()
        {

            //Arrange
            const int taskId = 1;
            repositoryMock.Setup(r => r.IsTaskExists(taskId)).Returns(true);
            repositoryMock.Setup(r => r.DeleteTask(taskId)).Returns(true);

            

            //Act
            var result = service.DeleteTask(taskId);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(StatusEnum.TaskDeleted, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);

        }

        [Test]
        public void DeleteTask_NotExistingTask_ReturnsNotFound()
        {

            //Arrange
            const int taskId = 1;
            repositoryMock.Setup(r => r.IsTaskExists(taskId)).Returns(false);

            

            //Act
            var result = service.DeleteTask(taskId);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.NotFound, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void DeleteTask_RepositoryError_ReturnsRepositoryError()
        {
            //Arrange
            const int taskId = 1;
            repositoryMock.Setup(r => r.IsTaskExists(taskId)).Returns(true);
            repositoryMock.Setup(r => r.DeleteTask(taskId)).Returns(false);

            

            //Act
            var result = service.DeleteTask(taskId);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.RepositoryError, result.Message);
            Assert.AreEqual(null, result.ErrorMessages);
        }

        [Test]
        public void DeleteTask_Exception_ReturnsError()
        {
            //Arrange
            exception = new Exception();
            ErrorMessages = new List<string> { Convert.ToString(exception.Message) };
            const int taskId = 1;
            repositoryMock.Setup(r => r.IsTaskExists(taskId)).Throws(exception);

            

            //Act
            var result = service.DeleteTask(taskId);

            //Assert
            Assert.AreEqual(null, result.Data);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(StatusEnum.Error, result.Message);
            Assert.AreEqual(ErrorMessages, result.ErrorMessages);
        }

    }
}
