using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace ToDoListTest.PageElements
{
    public class HomePage
    {
        private FormAddItem _formAddItem;
        private ToDoList _toDoList;

        private IWebDriver _webDriver;

        public HomePage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _formAddItem = new FormAddItem(webDriver);
            _toDoList = new ToDoList(webDriver);
        }

        public void CreateItem(string itemName, string itemDescription)
        {
            _formAddItem.AddItem(itemName,itemDescription);
        }

        public int GetItemsCount()
        {
            return _toDoList.GetItemsCount();
        }
    }
}
