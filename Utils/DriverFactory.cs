using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomationExercise.Tests.Utils
{
    public static class DriverFactory
    {
        public static IWebDriver CreateChromeDriver()
        {
            var options = new ChromeOptions();

            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--remote-allow-origins=*");

            var driver = new ChromeDriver(options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;

            return driver;
        }
    }
}
