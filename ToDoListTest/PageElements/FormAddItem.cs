using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace ToDoListTest.PageElements
{
    public class FormAddItem
    {
        private IWebDriver _webDriver;

        public FormAddItem(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            PageFactory.InitElements(webDriver,this);
        }

        [FindsBy(How= How.XPath, Using = "//input[@placeholder='Todo task']")]
        [CacheLookup]
        private IWebElement inputNameField;

        [FindsBy(How = How.XPath, Using = "//textarea[@placeholder='Todo description']")]
        [CacheLookup]
        private IWebElement inputDecsriptionField;

        [FindsBy(How = How.XPath, Using = "//button[@type='submit' and span='Create']")]
        [CacheLookup]
        private IWebElement buttonSubmit;

        public void AddItem(string itemName, string itemDescription)
        {
            inputNameField.SendKeys(itemName);
            inputDecsriptionField.SendKeys(itemDescription);
            buttonSubmit.Click();
        }
    }
}
