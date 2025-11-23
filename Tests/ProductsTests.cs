using Allure.NUnit;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using AutomationExercise.Tests.Utils;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Products")]
    public class ProductsTests : BaseTest
    {
        [Test]
        [AllureName("Категория Women → Tops содержит женские товары")]
        [AllureSeverity(SeverityLevel.normal)]
        public void WomenTopsCategory_ShouldShowWomenProductsInTitle()
        {
            LoginPage.Open();

            var mainPage = LoginPage.LoginAs(
                TestSettings.ValidUserName,
                TestSettings.ValidPassword);

            Assert.That(mainPage.IsOpened(), Is.True,
                "Главная страница не открылась после логина.");

            mainPage.Open();
            mainPage.OpenWomenTopsCategory();

            var title = mainPage.GetCurrentProductsTitle();

            Assert.Multiple(() =>
            {
                Assert.That(title, Does.Contain("Women").IgnoreCase,
                    "Заголовок товаров не содержит 'Women', как ожидалось для женской категории.");

                Assert.That(title, Does.Contain("Tops").IgnoreCase,
                    "Заголовок товаров не содержит 'Tops' для подкатегории Women → Tops.");
            });
        }
    }
}
