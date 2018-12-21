using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Tests.AcceptanceTests
{
    public class LanguageTests: AcceptanceTestBase
    {
        [Test]
        public void As_A_User_I_Can_Change_The_Website_Language_to_Latvian()
        {
            //setup
            const string latvianAbout = "Par mums";
            IWebElement originalhWebsiteText = _chromeDriver.FindElements(By.ClassName("nav-link"))[0]; //get initial 'About' element

            //act
            _chromeDriver.FindElement(By.ClassName("float-lg-right")).Click(); //click on the language selector
            _chromeDriver.FindElement(By.Id("ChangeLanguageLv")).Click(); //click on lv button
            WaitForStaleness(originalhWebsiteText); //wait for element to become stale (this forces driver to wait for page to fully refresh
            string websiteText = _chromeDriver.FindElements(By.ClassName("nav-link"))[0].Text; //get new text from the About element
            //assert
            Assert.IsTrue(latvianAbout.Equals(websiteText), websiteText); //check if its in latvian
        }

        [Test]
        public void As_A_User_I_Can_Change_The_Website_Language_to_English()
        {
            //setup
            const string englishAbout = "About";
            IWebElement originalhWebsiteText = _chromeDriver.FindElements(By.ClassName("nav-link"))[0]; 

            //act
            _chromeDriver.FindElement(By.ClassName("float-lg-right")).Click(); 
            _chromeDriver.FindElement(By.Id("ChangeLanguageEn")).Click();
            WaitForStaleness(originalhWebsiteText); 
            string websiteText = _chromeDriver.FindElements(By.ClassName("nav-link"))[0].Text; 
            //assert
            Assert.IsTrue(englishAbout.Equals(websiteText), websiteText);
        }
    }
}
