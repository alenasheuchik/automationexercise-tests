using Allure.NUnit;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using AutomationExercise.Tests.Utils;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Login")]
    public class LoginTests : BaseTest
    {
        [Test]
        [AllureName("Успешный логин с валидными кредами")]
        [AllureSeverity(SeverityLevel.normal)]
        public void LoginWithValidCredentials_ShouldOpenMainPage()
        {
            LoginPage.Open();

            Assert.That(LoginPage.IsOpened(), Is.True,
                "Страница логина не открылась.");

            var mainPage = LoginPage.LoginAs(
                TestSettings.ValidUserName,
                TestSettings.ValidPassword);

            Assert.That(mainPage.IsOpened(), Is.True,
                "Главная страница не открылась после логина.");
        }

        [Test]
        [AllureName("Логин с неверным паролем показывает ошибку")]
        [AllureSeverity(SeverityLevel.normal)]
        public void LoginWithInvalidPassword_ShouldShowErrorMessage()
        {
            LoginPage.Open();

            Assert.That(LoginPage.IsOpened(), Is.True,
                "Страница логина не открылась.");

            LoginPage.EnterUserName(TestSettings.ValidUserName);
            LoginPage.EnterPassword("wrong_password");
            LoginPage.ClickLoginButton();

            Assert.That(LoginPage.IsErrorDisplayed(), Is.True,
                "Ожидалось сообщение об ошибке логина, но его нет.");

            var errorText = LoginPage.GetErrorText();

            Assert.That(string.IsNullOrEmpty(errorText), Is.False,
                "Текст ошибки логина пустой.");
        }

        [Test]
        [AllureName("Логин с пустыми полями не должен пускать на главную")]
        [AllureSeverity(SeverityLevel.minor)]
        public void LoginWithEmptyEmailAndPassword_ShouldNotLogin()
        {
            LoginPage.Open();

            Assert.That(LoginPage.IsOpened(), Is.True,
                "Страница логина не открылась.");

            LoginPage.EnterUserName(string.Empty);
            LoginPage.EnterPassword(string.Empty);
            LoginPage.ClickLoginButton();

            Assert.Multiple(() =>
            {
                Assert.That(LoginPage.IsOpened(), Is.True,
                    "Ожидалось, что при пустых полях пользователь останется на странице логина.");

                var mainPage = new Pages.MainPage(Driver);

                Assert.That(mainPage.IsOpened(), Is.False,
                    "Главная страница не должна открываться при пустых полях логина/пароля.");
            });
        }
    }
}
