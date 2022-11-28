using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhptravelTest.Tests
{
    public class HotelSearchTest : IDisposable
    {
        public IWebDriver Driver { get; set; } = new ChromeDriver("D:\\3rdparty\\chrome");
        public const string URL = "https://phptravels.net/";

        [Fact]
        public void Travel_Search_Fields_Exists()
        {
            LoadPage();

            string travelersXPath = "//*[@id='hotels']/form[@id='hotels-search']/div[@class='main_search contact-form-action']" +
                "/div[@class='row g-1']/div[@class='col-md-3']/div[@class='input-box']/div[@class='form-group']/div[@class='dropdown dropdown-contain']" +
                "/a[@class='dropdown-toggle dropdown-btn travellers waves-effect']";

            var cities = Driver.FindElement(By.Id("hotels_city"));
            var checkin = Driver.FindElement(By.Id("checkin"));
            var checkout = Driver.FindElement(By.Id("checkout"));
            var travelers = Driver.FindElement(By.XPath(travelersXPath));

            Assert.True(cities is not null && cities.Displayed && cities.Enabled, "Cities field not working as expected.");
            Assert.True(checkin is not null && checkin.Displayed && checkin.Enabled, "Checkin field not working as expected.");
            Assert.True(checkout is not null && checkout.Displayed && checkout.Enabled, "Checkout field not working as expected.");
            Assert.True(travelers is not null && travelers.Displayed && travelers.Enabled, "travelers field not working as expected.");
        }

        [Fact]
        public void Checkin_Field_Show_DatePicker()
        {
            LoadPage();
            var checkin = Driver.FindElement(By.Id("checkin"));
            var checkinDatepicker = Driver.FindElements(By.ClassName("datepicker")).FirstOrDefault();

            Assert.False(checkinDatepicker?.Displayed);
            checkin.Click();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(d => checkinDatepicker.Displayed);
            Assert.True(checkinDatepicker?.Displayed, "Checkin datepicker do not displayed on click");
        }

        [Fact]
        public void Checkout_Field_Show_DatePicker()
        {
            LoadPage();
            var checkout = Driver.FindElement(By.Id("checkout"));
            var checkoutDatepicker = Driver.FindElement(By.XPath("//div[@class='datepicker dropdown-menu'][2]"));
            checkout.Click();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(d => checkoutDatepicker.Displayed);
            Assert.True(checkoutDatepicker?.Displayed, "Checkout datepicker do not displayed on click");
        }

        [Fact]
        public void Travelers_Field_Open_Dropdown()
        {
            LoadPage();
            var travelers = Driver.FindElement(By.XPath("//*[@id='hotels']/form[@id='hotels-search']/div[@class='main_search contact-form-action']/div[@class='row g-1']" +
                "/div[@class='col-md-3']/div[@class='input-box']/div[@class='form-group']/div[@class='dropdown dropdown-contain']/a[@class='dropdown-toggle dropdown-btn travellers waves-effect']"));

            var travelersPicker = Driver.FindElement(By.XPath("//form[@id='hotels-search']/div[@class='main_search contact-form-action']/div[@class='row g-1']/div[@class='col-md-3']" +
                "/div[@class='input-box']/div[@class='form-group']/div[@class='dropdown dropdown-contain']/div[@class='dropdown-menu dropdown-menu-wrap']"));

            Assert.False(travelersPicker?.Displayed);
            travelers.Click();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            Assert.True(travelersPicker?.Displayed, "Travelers options picker do not displayed on click");
        }

        private void LoadPage()
        {
            Driver.Navigate().GoToUrl(URL);

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));
            wait.Until(d => d.FindElement(By.Id("hotels-tab")).Displayed);
            Driver.FindElement(By.Id("hotels-tab")).Click();
        }

        public void Dispose()
        {
            Driver.Close();
        }
    }
}
