using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;
using System.Linq;

namespace AutomationExercise.Tests.Pages
{
    public class CartPage : BasePage
    {
        private readonly By CartContainerLocator =
            By.CssSelector("section#cart_items");

        private readonly By CartRowsLocator =
            By.CssSelector("div.cart_info table tbody tr");
        public CartPage(IWebDriver driver)
            : base(driver)
        {
        }
        public override bool IsOpened()
        {
            return IsElementDisplayed(CartContainerLocator);
        }
        public bool HasAnyProducts()
        {
            try
            {
                IReadOnlyCollection<IWebElement> rows = WaitAndFindElements(CartRowsLocator);

                foreach (var row in rows)
                {
                    if (row.Displayed)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        public int GetProductsCount()
        {
            try
            {
                IReadOnlyCollection<IWebElement> rows = WaitAndFindElements(CartRowsLocator);
                return rows.Count(row => row.Displayed);
            }
            catch
            {
                return 0;
            }
        }
        public int GetFirstProductQuantity()
        {
            var row = GetFirstDisplayedRow();

            try
            {
                var quantityButton =
                    row.FindElement(By.CssSelector("td.cart_quantity button"));

                var text = quantityButton.Text;

                if (int.TryParse(text, out var quantity))
                {
                    return quantity;
                }
            }
            catch (NoSuchElementException)
            {
                var quantityCell =
                    row.FindElement(By.CssSelector("td.cart_quantity"));

                return ParseNumber(quantityCell.Text);
            }

            return 0;
        }
        public int GetFirstProductPrice()
        {
            var row = GetFirstDisplayedRow();

            var priceElement =
                row.FindElement(By.CssSelector("td.cart_price p"));

            return ParseNumber(priceElement.Text);
        }
        public int GetFirstProductTotal()
        {
            var row = GetFirstDisplayedRow();

            var totalElement =
                row.FindElement(By.CssSelector("td.cart_total p.cart_total_price"));

            return ParseNumber(totalElement.Text);
        }
        public void ClearCart()
        {
            try
            {
                var rows = WaitAndFindElements(CartRowsLocator).ToList();

                foreach (var row in rows)
                {
                    if (!row.Displayed)
                    {
                        continue;
                    }

                    var deleteButton = row.FindElement(By.CssSelector("a.cart_quantity_delete"));

                    deleteButton.Click();

                    Wait.Until(ExpectedConditions.StalenessOf(row));
                }
            }
            catch
            {
            }
        }
        private IWebElement GetFirstDisplayedRow()
        {
            IReadOnlyCollection<IWebElement> rows = WaitAndFindElements(CartRowsLocator);

            foreach (var row in rows)
            {
                if (row.Displayed)
                {
                    return row;
                }
            }

            throw new NoSuchElementException("В корзине нет видимых строк с товарами.");
        }
        private static int ParseNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            var digits = new string(text.Where(char.IsDigit).ToArray());

            return int.TryParse(digits, out var result)
                ? result
                : 0;
        }
    }
}
