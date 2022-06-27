using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace ToDoListTest.PageElements
{
    public class ToDoList
    {
        private IWebDriver _webDriver;

        public ToDoList(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            PageFactory.InitElements(webDriver, this);
        }

        [FindsBy(How = How.XPath,
            Using = "//div[contains(@class, 'TaskItem')]")]
        [CacheLookup]
        private IList<IWebElement> itemCards;

        public int GetItemsCount()
        {
            return itemCards.Count();
        }
    }
}
