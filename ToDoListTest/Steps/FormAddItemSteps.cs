using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace ToDoListTest.Steps
{
    public class FormAddItemSteps
    {
        private IWebDriver _webDriver;

        public FormAddItemSteps(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void FormSubmit(string buttonText)
        {
            _webDriver.FindElement(
                By.XPath($"//button[@type='submit' and span='{buttonText}']"))
                .Click();
        }

        public void InputNameText(string placeholder, string taskName)
        {
            _webDriver.FindElement(
                By.XPath($"//input[@placeholder='{placeholder}']"))
                .SendKeys($"{taskName}");
        }
    }
}
