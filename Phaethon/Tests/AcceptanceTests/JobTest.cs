using NUnit.Framework;
using OpenQA.Selenium;

namespace Tests.AcceptanceTests
{
    public class JobTest: AcceptanceTestBase
    {
        [Test]
        public void ReadAllJobs()
        {
            IWebElement btn = _chromeDriver.FindElement(By.Id("ChangeLanguageEn"));
            Assert.AreEqual(btn.TagName, "div", btn.TagName);
        }
    }
}
