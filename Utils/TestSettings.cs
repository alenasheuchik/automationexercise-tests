using System;
using System.Configuration;

namespace AutomationExercise.Tests.Utils
{
    public static class TestSettings
    {
        public static string BaseUrl =>
            ConfigurationManager.AppSettings["BaseUrl"];
        public static string LoginUrl =>
            ConfigurationManager.AppSettings["LoginUrl"];
        public static string ValidUserName =>
            ConfigurationManager.AppSettings["ValidUserName"];
        public static string ValidPassword =>
            ConfigurationManager.AppSettings["ValidPassword"];
        public static TimeSpan DefaultTimeout =>
            TimeSpan.FromSeconds(20);
    }
}
