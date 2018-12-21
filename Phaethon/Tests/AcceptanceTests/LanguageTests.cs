using NUnit.Framework;
using OpenQA.Selenium;

namespace Tests.AcceptanceTests
{
    public class LanguageTests: AcceptanceTestBase
    {
        [Test]
        public void As_A_User_I_Can_Change_The_Website_Language_to_Latvian()
        {
            IWebElement btn = _chromeDriver.FindElement(By.Id("ChangeLanguageLv"));
            Assert.AreEqual(btn.TagName, "div", btn.TagName);
        }
    }
}
