using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace ToDoListTest.PageElements
{
    public class HomePageNotComposite
    {
        private IWebDriver _webDriver;

        public HomePageNotComposite(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            PageFactory.InitElements(webDriver, this);
        }

        [FindsBy(How = How.XPath, Using = "//input[@placeholder='Todo task']")]
        [CacheLookup]
        private IWebElement inputNameField;

        [FindsBy(How = How.XPath, Using = "//textarea[@placeholder='Todo description']")]
        [CacheLookup]
        private IWebElement inputDecsriptionField;

        [FindsBy(How = How.XPath, Using = "//button[@type='submit' and span='Create']")]
        [CacheLookup]
        private IWebElement buttonSubmit;

        [FindsBy(How = How.XPath,
            Using = "//div[contains(@class, 'TaskItem')]")]
        [CacheLookup]
        private IList<IWebElement> itemCards;

        public int GetItemsCount()
        {
            return itemCards.Count();
        }

        public void CreateItem(string itemName, string itemDescription)
        {
            inputNameField.SendKeys(itemName);
            inputDecsriptionField.SendKeys(itemDescription);
            buttonSubmit.Click();
        }

    }
}
