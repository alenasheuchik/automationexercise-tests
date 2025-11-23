using AutomationExercise.Tests.Utils;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;
using System.Linq;

namespace AutomationExercise.Tests.Pages
{
    public class MainPage : BasePage
    {
        private readonly By LoggedInAsLocator =
            By.XPath("//*[contains(text(),'Logged in as')]");

        private readonly By LogoutButtonLocator =
            By.CssSelector("a[href='/logout']");

        private readonly By FeaturesItemsLocator =
            By.CssSelector("div.features_items");

        private readonly By AddToCartButtonsLocator =
            By.CssSelector("div.features_items div.productinfo a.btn.add-to-cart");

        private readonly By CartLinkLocator =
            By.CssSelector("a[href='/view_cart']");

        private readonly By WomenCategoryToggleLocator =
            By.XPath("//div[contains(@class,'category-products')]//a[@data-toggle='collapse' and contains(normalize-space(),'Women')]");

        private readonly By WomenTopsSubcategoryLocator =
            By.XPath("//div[@id='Women']//a[contains(normalize-space(),'Tops')]");

        private readonly By ProductsTitleLocator =
            By.CssSelector("div.features_items h2.title.text-center");

        private readonly By ProductInfoBlocksLocator =
            By.CssSelector("div.features_items div.productinfo");

        private readonly By ProductAddedModalLocator =
            By.Id("cartModal");

        private readonly By ContinueShoppingButtonLocator =
            By.CssSelector("button.close-modal");
        private IWebElement LogoutButton =>
            WaitAndFindElement(LogoutButtonLocator);
        public MainPage(IWebDriver driver)
            : base(driver)
        {
        }
        public override bool IsOpened()
        {
            return IsElementDisplayed(LoggedInAsLocator);
        }
        public void Open()
        {
            Driver.Navigate().GoToUrl(TestSettings.BaseUrl);
        }
        public void AddFirstVisibleProductToCart()
        {
            WaitAndFindElement(FeaturesItemsLocator);

            IReadOnlyCollection<IWebElement> buttons =
                WaitAndFindElements(AddToCartButtonsLocator);

            foreach (var button in buttons)
            {
                if (!button.Displayed)
                {
                    continue;
                }

                ScrollToElement(button);
                ClickElement(button);

                CloseProductAddedModalIfPresent();
                break;
            }
        }
        public void AddFirstNVisibleProductsToCart(int count)
        {
            if (count <= 0)
            {
                return;
            }

            WaitAndFindElement(FeaturesItemsLocator);

            IReadOnlyCollection<IWebElement> buttons =
                WaitAndFindElements(AddToCartButtonsLocator);

            int added = 0;

            foreach (var button in buttons)
            {
                if (!button.Displayed)
                {
                    continue;
                }

                ScrollToElement(button);
                ClickElement(button);

                CloseProductAddedModalIfPresent();
                added++;

                if (added >= count)
                {
                    break;
                }
            }
        }
        public CartPage OpenCart()
        {
            ClickElement(CartLinkLocator);
            return new CartPage(Driver);
        }
        public CartPage AddFirstProductToCartAndOpenCart()
        {
            Open();
            AddFirstVisibleProductToCart();
            return OpenCart();
        }
        public LoginPage Logout()
        {
            LogoutButton.Click();
            return new LoginPage(Driver);
        }
        private void HideAdsIfPresent()
        {
            try
            {
                var js = (IJavaScriptExecutor)Driver;

                js.ExecuteScript(@"
                    var iframes = document.querySelectorAll(
                        'iframe[title=""Advertisement""], iframe[id^=""aswift_""]'
                    );
                    for (var i = 0; i < iframes.length; i++) {
                        iframes[i].style.display = 'none';
                    }");
            }
            catch
            {
            }
        }

        public void OpenWomenTopsCategory()
        {
            HideAdsIfPresent();

            var womenToggle = WaitAndFindElement(WomenCategoryToggleLocator);
            ScrollToElement(womenToggle);

            try
            {
                womenToggle.Click();
            }
            catch (ElementClickInterceptedException)
            {
                var js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript("arguments[0].click();", womenToggle);
            }

            var topsLink = WaitAndFindElement(WomenTopsSubcategoryLocator);
            ScrollToElement(topsLink);

            try
            {
                topsLink.Click();
            }
            catch (ElementClickInterceptedException)
            {
                var js = (IJavaScriptExecutor)Driver;
                js.ExecuteScript("arguments[0].click();", topsLink);
            }
        }
        public string GetCurrentProductsTitle()
        {
            var titleElement = WaitAndFindElement(ProductsTitleLocator);
            return titleElement.Text;
        }
        public int GetFirstVisibleProductPriceFromList()
        {
            WaitAndFindElement(FeaturesItemsLocator);

            var products = WaitAndFindElements(ProductInfoBlocksLocator);

            foreach (var product in products)
            {
                if (!product.Displayed)
                {
                    continue;
                }

                var priceElement = product.FindElement(By.TagName("h2"));
                var priceText = priceElement.Text;

                return ParsePrice(priceText);
            }

            return 0;
        }
        private void CloseProductAddedModalIfPresent()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementIsVisible(ProductAddedModalLocator));

                ClickElement(ContinueShoppingButtonLocator);

                Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(ProductAddedModalLocator));
            }
            catch
            {
            }
        }
        private static int ParsePrice(string priceText)
        {
            if (string.IsNullOrEmpty(priceText))
            {
                return 0;
            }

            var digits = new string(priceText.Where(char.IsDigit).ToArray());

            return int.TryParse(digits, out var result)
                ? result
                : 0;
        }
    }
}
