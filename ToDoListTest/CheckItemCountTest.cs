using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ToDoListTest.PageElements;
using ToDoListTest.Steps;

namespace ToDoListTest
{
    public class CheckItemCountTest
    {
        private IWebDriver _webDriver;
        private HomePage _homePage;

        [SetUp]
        public void StartBrowser()
        {
            _webDriver = new ChromeDriver();
            _homePage = new HomePage(_webDriver);

            _webDriver.Url = "http://localhost:3000";
            _webDriver.Manage().Window.Maximize();
        }

        [Test]
        public void CreateTask_CreatesOneTask()
        {
            _homePage.CreateItem("Go for a walk", "I have to walk 100km(");
            var result = _homePage.GetItemsCount();

            Assert.AreEqual(1, result);
        }

        [TearDown]
        public void closeBrowser()
        {
            _webDriver.Close();
        }
    }
}
