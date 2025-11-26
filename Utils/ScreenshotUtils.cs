using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AutomationExercise.Tests.Utils
{
    public static class ScreenshotUtils
    {
        public static string TakeScreenshot(IWebDriver driver, string testName)
        {
            if (driver == null)
            {
                return null;
            }

            try
            {
                var takesScreenshot = driver as ITakesScreenshot;
                if (takesScreenshot == null)
                {
                    return null;
                }

                var screenshot = takesScreenshot.GetScreenshot();
                var safeTestName = MakeSafeFileName(testName);
                var fileName = $"{safeTestName}_{Guid.NewGuid():N}.png";

                var allureResultsDir = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    "allure-results");

                if (!Directory.Exists(allureResultsDir))
                {
                    Directory.CreateDirectory(allureResultsDir);
                }

                var fullPath = Path.Combine(allureResultsDir, fileName);
                screenshot.SaveAsFile(fullPath);

                return fullPath;
            }
            catch
            {
                return null;
            }
        }
        private static string MakeSafeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "screenshot";
            }

            foreach (var c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return name;
        }
    }
}