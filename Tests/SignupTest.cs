using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhptravelTest.Tests
{
    public class SignupTest : IDisposable
    {
        public IWebDriver Driver { get; set; } = new ChromeDriver("D:\\3rdparty\\chrome");

        public const string URL = "https://phptravels.net/";
        public const string PAGE = "signup";

        [Fact]
        public void Signup_Button_Works()
        {
            LoadPage();
            Driver.Navigate().GoToUrl($"{URL}");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
            wait.Until(d => d.Url == URL);

            var accountButton = Driver.FindElement(By.Id("ACCOUNT"));
            accountButton.Click();

            wait.Timeout = TimeSpan.FromSeconds(5);
            wait.Until(d => d.FindElement(By.XPath("//*[@id=\"fadein\"]/header/div/div/div/div/div/div[2]/div/div[2]/div[3]/div/ul")).Displayed);

            //Click in signup button
            Driver.FindElement(By.XPath("//*[@id=\"fadein\"]/header/div/div/div/div/div/div[2]/div/div[2]/div[3]/div/ul/li[2]/a")).Click();
            wait.Timeout = TimeSpan.FromSeconds(60);
            wait.Until(d => d.Url == $"{URL}{PAGE}");

            Assert.True(Driver.Url == $"{URL}{PAGE}");
        }


        [Fact]
        public void Empty_Form_Validation()
        {
            LoadPage();
            Assert.False(IsValidForm() ?? true);
        }

        [Fact]
        public void FirstName_Shouldnt_Have_Numbers()
        {
            LoadPage();
            int firstNameIndex = 1;

            var form = GetForm();

            ((IJavaScriptExecutor)Driver).ExecuteScript($"arguments[0][{firstNameIndex}].value = 'Daniel1'", form);

            var executionStart = DateTime.Now;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            wait.Until(d => DateTime.Now - executionStart - TimeSpan.FromMilliseconds(3000) > TimeSpan.Zero);


            Assert.False(IsValidForm(firstNameIndex) ?? true);
        }

        [Fact]
        public void LastName_Shouldnt_Have_Numbers()
        {
            LoadPage();
            int lastNameIndex = 2;

            var form = GetForm();
            ((IJavaScriptExecutor)Driver).ExecuteScript($"arguments[0][{lastNameIndex}].value = 'Campos0'", form);
            var executionStart = DateTime.Now;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            wait.Until(d => DateTime.Now - executionStart - TimeSpan.FromMilliseconds(3000) > TimeSpan.Zero);


            Assert.False(IsValidForm(lastNameIndex) ?? true);
        }

        [Fact]
        public void Phone_Number_Shouldnt_Have_Letters()
        {
            LoadPage();
            int phoneIndex = 3;


            var form = GetForm();
            ((IJavaScriptExecutor)Driver).ExecuteScript($"arguments[0][{phoneIndex}].value = '+55279999A999'", form);
            var executionStart = DateTime.Now;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            wait.Until(d => DateTime.Now - executionStart - TimeSpan.FromMilliseconds(3000) > TimeSpan.Zero);


            Assert.False(IsValidForm(phoneIndex) ?? true);
        }


        [Fact]
        public void Email_Validation()
        {
            LoadPage();
            int emailIndex = 4;
            var text = "emailTeste@@";

            var form = GetForm();
            ((IJavaScriptExecutor)Driver).ExecuteScript($"arguments[0][{emailIndex}].value = '{text}'", form);
            var executionStart = DateTime.Now;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            wait.Until(d => DateTime.Now - executionStart - TimeSpan.FromMilliseconds(3000) > TimeSpan.Zero);


            Assert.False(IsValidForm(emailIndex) ?? true);
        }

        [Fact]
        public void Password_Must_Have_At_Least_8_characters()
        {
            LoadPage();
            int passwordIndex = 5;

            var form = GetForm();
            ((IJavaScriptExecutor)Driver).ExecuteScript($"arguments[0][{passwordIndex}].value = 'senha12'", form);
            var executionStart = DateTime.Now;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            wait.Until(d => DateTime.Now - executionStart - TimeSpan.FromMilliseconds(3000) > TimeSpan.Zero);

            Assert.False(IsValidForm(passwordIndex) ?? true, "Password need to have more than 7 characters");
        }

        private bool? IsValidForm(int? index = null)
        {
            var form = GetForm();
            string? indexBuilder = index is null ? string.Empty : $"[{index}]";

            bool? isValidForm = (bool)((IJavaScriptExecutor)Driver).ExecuteScript($"return arguments[0]{indexBuilder}.checkValidity();", form);

            var executionStart = DateTime.Now;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            wait.Until(d => DateTime.Now - executionStart - TimeSpan.FromMilliseconds(3000) > TimeSpan.Zero);

            return isValidForm;
        }

        private IWebElement? GetForm()
        {
            var xpath = "//*[@id=\"fadein\"]/div[4]/div/div[2]/div[2]/div/form";
            return Driver.FindElement(By.XPath(xpath));
        }

        private void LoadPage()
        {
            Driver.Navigate().GoToUrl($"{URL}{PAGE}");
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
            wait.Until(d => d.Url == $"{URL}{PAGE}");
        }

        public void Dispose()
        {
            Driver.Close();
        }
    }
}
