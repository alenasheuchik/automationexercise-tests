using Allure.NUnit;
using Allure.NUnit.Attributes;
using Allure.Net.Commons;
using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Utils;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Cart")]
    public class CartTests : BaseTest
    {
        [Test]
        [AllureName("Добавить товар в корзину")]
        [AllureSeverity(SeverityLevel.normal)]
        public void AddProductToCart_ShouldAppearInCart()
        {
            LoginPage.Open();

            var mainPage = LoginPage.LoginAs(
                TestSettings.ValidUserName,
                TestSettings.ValidPassword);

            Assert.That(mainPage.IsOpened(), Is.True,
                "Главная страница не открылась после логина.");

            var cartPage = mainPage.AddFirstProductToCartAndOpenCart();

            Assert.That(cartPage.IsOpened(), Is.True,
                "Страница корзины не открылась.");

            Assert.That(cartPage.HasAnyProducts(), Is.True,
                "Корзина пуста, хотя ожидалось, что в ней будет хотя бы один товар.");
        }

        [Test]
        [AllureName("Проверка количества и суммы товара в корзине")]
        [AllureSeverity(SeverityLevel.normal)]
        public void AddedProduct_ShouldHaveQuantityOneAndCorrectTotal()
        {
            LoginPage.Open();

            var mainPage = LoginPage.LoginAs(
                TestSettings.ValidUserName,
                TestSettings.ValidPassword);

            Assert.That(mainPage.IsOpened(), Is.True,
                "Главная страница не открылась после логина.");

            mainPage.Open();
            var cartPageBefore = mainPage.OpenCart();

            Assert.That(cartPageBefore.IsOpened(), Is.True,
                "Страница корзины не открылась.");

            cartPageBefore.ClearCart();

            mainPage.Open();
            var cartPage = mainPage.AddFirstProductToCartAndOpenCart();

            Assert.That(cartPage.IsOpened(), Is.True,
                "Страница корзины не открылась после добавления товара.");

            Assert.That(cartPage.HasAnyProducts(), Is.True,
                "Корзина пуста, хотя ожидалось, что в ней будет хотя бы один товар.");

            var quantity = cartPage.GetFirstProductQuantity();
            var price = cartPage.GetFirstProductPrice();
            var total = cartPage.GetFirstProductTotal();

            Assert.Multiple(() =>
            {
                Assert.That(quantity, Is.EqualTo(1),
                    "Ожидалось количество 1 для добавленного товара.");

                Assert.That(total, Is.EqualTo(price * quantity),
                    "Сумма в колонке 'Total' не равна Price * Quantity.");
            });
        }

        [Test]
        [AllureName("Добавление трех товаров в корзину")]
        [AllureSeverity(SeverityLevel.normal)]
        public void AddThreeProductsToCart_ShouldHaveExactlyThreeProducts()
        {
            LoginPage.Open();

            var mainPage = LoginPage.LoginAs(
                TestSettings.ValidUserName,
                TestSettings.ValidPassword);

            Assert.That(mainPage.IsOpened(), Is.True,
                "Главная страница не открылась после логина.");

            mainPage.Open();
            var cartPageBefore = mainPage.OpenCart();
            cartPageBefore.ClearCart();
            mainPage.Open();
            mainPage.AddFirstNVisibleProductsToCart(3);

            var cartPage = mainPage.OpenCart();

            Assert.That(cartPage.IsOpened(), Is.True,
                "Страница корзины не открылась.");

            var productsCount = cartPage.GetProductsCount();

            Assert.That(productsCount, Is.EqualTo(3),
                "Ожидалось ровно 3 товара в корзине.");
        }
    }
}
