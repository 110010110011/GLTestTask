using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Linq;

namespace TestTaskGL1
{
    class ChromeTest
    {
        IWebDriver driver;
        Actions actions;

        [SetUp]
        public void Initialize()
        {
            driver = new ChromeDriver();
            actions = new Actions(driver);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test()
        {
            driver.Url = "https://duckduckgo.com/";
            var element = driver.FindElement(By.Id("search_form_input_homepage"));
            element.SendKeys("Duck");
            driver.FindElement(By.Id("search_button_homepage")).Click();

            var firstResultsList = driver.FindElements(
                By.ClassName("results_links_deep"));
            Assert.IsTrue(firstResultsList.All(x => x.Displayed));

            var firstResultsCount = firstResultsList.Count;
            var showMoreBtn = driver.FindElement(By.Id("rld-1"));
            actions.MoveToElement(showMoreBtn);

            showMoreBtn.Click();

            var secondResultsList = driver.FindElements(
                By.ClassName("results_links_deep"));

            Assert.IsTrue(secondResultsList.Count > firstResultsCount);
            Assert.IsTrue(secondResultsList.All(x => x.Displayed));
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }
    }
}
