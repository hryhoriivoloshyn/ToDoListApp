using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ToDoListTest
{
    public class AddTaskTestNoSteps
    {
        private IWebDriver _webDriver;

        [SetUp]
        public void StartBrowser()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Url = "http://localhost:3000";
            _webDriver.Manage().Window.Maximize();
        }

        [Test]
        public void CreateTask_TaskCreated()
        {
            const string inputFieldPlaceholder = "Todo task";
            const string inputFieldValue = "To do the dishes";
            const string buttonText = "Create";

            IWebElement textInputTaskName = 
                _webDriver.FindElement(
                    By.XPath($"//input[@placeholder='{inputFieldPlaceholder}']"));
            textInputTaskName.SendKeys(inputFieldValue);

            IWebElement createButton = 
                _webDriver.FindElement(
                    By.XPath($"//button[@type='submit' and span='{buttonText}']"));
            createButton.Click();

            IWebElement itemName =
                _webDriver.FindElement(
                    By.XPath($"//div[contains(@class, 'ant-card-head-title')" +
                             $" and text() = '{inputFieldValue}']"));
            Assert.IsNotNull(itemName);
        }

        [TearDown]
        public void closeBrowser()
        {
            _webDriver.Close();
        }
    }
}
