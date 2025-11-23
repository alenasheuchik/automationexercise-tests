using AutomationExercise.Tests.Utils;
using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By emailInputLocator =
            By.CssSelector(".login-form form input[type='email'], input[data-qa='login-email']");
        private readonly By passwordInputLocator =
            By.CssSelector(".login-form form input[type='password'], input[data-qa='login-password']");
        private readonly By loginButtonLocator =
            By.XPath("//form//button[normalize-space()='Login'] | //button[@data-qa='login-button']");
        private readonly By errorMessageLocator =
            By.XPath("//p[contains(text(),'Your email or password is incorrect!')]");
        private IWebElement EmailInput =>
            WaitAndFindElement(emailInputLocator);
        private IWebElement PasswordInput =>
            WaitAndFindElement(passwordInputLocator);
        private IWebElement LoginButton =>
            WaitAndFindElement(loginButtonLocator);
        public LoginPage(IWebDriver driver)
            : base(driver)
        {
        }
        public LoginPage Open()
        {
            Driver.Navigate().GoToUrl(TestSettings.LoginUrl);
            return this;
        }
        public override bool IsOpened()
        {
            return IsElementDisplayed(emailInputLocator)
                   && IsElementDisplayed(loginButtonLocator);
        }
        public void EnterUserName(string email)
        {
            EmailInput.Clear();
            EmailInput.SendKeys(email);
        }
        public void EnterPassword(string password)
        {
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);
        }
        public void ClickLoginButton()
        {
            LoginButton.Click();
        }
        public MainPage LoginAs(string email, string password)
        {
            EnterUserName(email);
            EnterPassword(password);
            ClickLoginButton();

            return new MainPage(Driver);
        }
        public bool IsErrorDisplayed()
        {
            return IsElementDisplayed(errorMessageLocator);
        }
        public string GetErrorText()
        {
            try
            {
                var element = WaitAndFindElement(errorMessageLocator);
                return element.Text;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
