using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace ToDoListTest.Steps
{
    public class TaskListSteps
    {
        private IWebDriver _webDriver;

        public TaskListSteps(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public bool CheckTaskExistsByName(string taskName)
        {
            IWebElement itemName = _webDriver.FindElement(
                By.XPath($"//div[contains(@class, 'ant-card-head-title')" +
                         $" and text() = '{taskName}']"));
            return itemName != null;
        }
    }
}
