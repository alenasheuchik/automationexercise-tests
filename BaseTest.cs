using Allure.Net.Commons;
using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Utils;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace AutomationExercise.Tests
{
    public class BaseTest
    {
        protected IWebDriver Driver;
        protected LoginPage LoginPage;
        protected MainPage MainPage;

        [SetUp]
        public void SetUp()
        {
            Driver = DriverFactory.CreateChromeDriver();

            LoginPage = new LoginPage(Driver);
            MainPage = new MainPage(Driver);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                var outcome = TestContext.CurrentContext.Result.Outcome.Status;

                if (outcome == TestStatus.Failed)
                {
                    var testName = TestContext.CurrentContext.Test.Name;

                    var screenshotPath = ScreenshotUtils.TakeScreenshot(Driver, testName);

                    if (!string.IsNullOrEmpty(screenshotPath))
                    {
                        AllureApi.AddAttachment(
                            "Screenshot",
                            "image/png",
                            screenshotPath);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Driver?.Quit();
            }
        }
    }
}
