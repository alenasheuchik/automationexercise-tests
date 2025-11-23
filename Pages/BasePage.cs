using System.Collections.Generic;
using AutomationExercise.Tests.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutomationExercise.Tests.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;
        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TestSettings.DefaultTimeout);
        }
        protected IWebElement WaitAndFindElement(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
        protected IReadOnlyCollection<IWebElement> WaitAndFindElements(By locator)
        {
            Wait.Until(ExpectedConditions.ElementExists(locator));
            return Driver.FindElements(locator);
        }
        protected void ClickElement(By locator)
        {
            Wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
        }
        protected void ClickElement(IWebElement element)
        {
            Wait.Until(_ => element.Displayed && element.Enabled);
            element.Click();
        }
        protected void ScrollToElement(By locator)
        {
            var element = WaitAndFindElement(locator);
            ScrollToElement(element);
        }
        protected void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver)
                .ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
        protected bool IsElementDisplayed(By locator)
        {
            try
            {
                return WaitAndFindElement(locator).Displayed;
            }
            catch
            {
                return false;
            }
        }
        public abstract bool IsOpened();
    }
}
