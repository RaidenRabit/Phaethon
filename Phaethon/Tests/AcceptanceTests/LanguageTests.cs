using NUnit.Framework;
using OpenQA.Selenium;

namespace Tests.AcceptanceTests
{
    public class LanguageTests: AcceptanceTestBase
    {
        [Test]
        public void As_A_User_I_Can_Change_The_Website_Language_to_Latvian()
        {
            _firefoxDriver.FindElement(By.Id("ChangeLanguageLv")).Click();
        }
    }
}
