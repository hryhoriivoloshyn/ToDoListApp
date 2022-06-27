using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ToDoListTest.Steps;

namespace ToDoListTest
{
    public class AddTaskWithStepsTest
    {
        private IWebDriver _webDriver;
        private FormAddItemSteps _formAddItemSteps;
        private TaskListSteps _taskListSteps;

        [SetUp]
        public void StartBrowser()
        {
            _webDriver = new ChromeDriver();
            _formAddItemSteps = new FormAddItemSteps(_webDriver);
            _taskListSteps = new TaskListSteps(_webDriver);

            _webDriver.Url = "http://localhost:3000";
            _webDriver.Manage().Window.Maximize();
        }

        [Test]
        public void CreateTask_TaskCreated()
        {
            const string inputFieldPlaceholder = "Todo task";
            const string inputFieldValue = "To do the dishes";
            const string buttonText = "Create";

            _formAddItemSteps.InputNameText(inputFieldPlaceholder, inputFieldValue);
            _formAddItemSteps.FormSubmit(buttonText);

            var result = _taskListSteps.CheckTaskExistsByName(inputFieldValue);
            Assert.IsTrue(result);
        }

        [TearDown]
        public void closeBrowser()
        {
            _webDriver.Close();
        }
    }
}
