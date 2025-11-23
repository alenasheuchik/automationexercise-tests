using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AutomationExercise.Tests.Utils
{
    public static class ScreenshotUtils
    {
        public static string TakeScreenshot(IWebDriver driver, string fileNamePrefix)
        {
            var screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver == null)
            {
                return null;
            }
            var screenshot = screenshotDriver.GetScreenshot();
            var screenshotsDirectory = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "screenshots");

            Directory.CreateDirectory(screenshotsDirectory);

            var fileName = $"{fileNamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var filePath = Path.Combine(screenshotsDirectory, fileName);

            screenshot.SaveAsFile(filePath);

            return filePath;
        }
    }
}
