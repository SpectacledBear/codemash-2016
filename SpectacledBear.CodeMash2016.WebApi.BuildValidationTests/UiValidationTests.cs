using System;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace SpectacledBear.CodeMash2016.WebApi.BuildValidationTests
{
    [Trait("category", "ui")]
    public class UiValidationTests
    {
        private readonly Uri _websiteUrl = new Uri("http://localhost:11341");

        [Fact]
        public void IndexPage_IsPresent()
        {
            using (IWebDriver driver = new PhantomJSDriver())
            {
                driver.Navigate().GoToUrl(_websiteUrl);

                By tableSelector = By.CssSelector("table#hobbitsList tr td");
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(tableSelector));

                IWebElement element = driver.FindElement(tableSelector);

                Assert.True(element.Displayed);
            }
        }
    }
}
