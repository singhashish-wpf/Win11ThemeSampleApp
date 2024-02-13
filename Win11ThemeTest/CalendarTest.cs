using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win11ThemeTest
{
    public class CalendarTest
    {
        private Application app;
        private Window window;
        public Window calwindow;
        Button testButton;
        Calendar calendar;
        Calendar multiSelectCalendar;
        AutomationElement headerBtn;
        AutomationElement prevBtn;
        AutomationElement nextBtn;
        public CalendarTest()
        {
            app = Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");

            using (var automation = new UIA3Automation())
            {
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testbtn")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                calwindow = window.FindFirstDescendant(cf => cf.ByName("CalendarWindow")).AsWindow();
                calendar = calwindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCal")).AsCalendar();
                multiSelectCalendar = calwindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCal_multiSelect")).AsCalendar();
                
            }
        }

        //test if calendar is available in window
        [Test]
        public void cal1_isCalendarAvailable()
        {
            Assert.IsNotNull(calwindow);
            Assert.IsNotNull(calendar);
        }

        //test if selected date is today's date
        [Test]
        public void cal2_isCalendarTodayDate()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            DateTime[] sdate = calendar.SelectedDates;
            Assert.That(sdate.Length, Is.EqualTo(1));
            Assert.That(sdate[0], Is.EqualTo(thisDay));

        }


        //test for click previous month button
        [Test]
        public void cal3_isCalendarClickprevMonth()
        {
            Assert.IsNotNull(calendar);
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string headerName = headerBtn.Name;
            DateTime prevMonth = DateTime.Today.AddMonths(-1);
            int year = prevMonth.Year;
            string Month = prevMonth.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));
            //reset to current month
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
        }

        //test for click next month button
        [Test]
        public void cal4_isCalendarClickNextMonth()
        {
            Assert.IsNotNull(calendar);
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string headerName = headerBtn.Name;
            DateTime nextMonth = DateTime.Today.AddMonths(1);
            int year = nextMonth.Year;
            string Month = nextMonth.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));
            //reset to current month
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
        }

        //test for click Monthyear button
        [Test]
        public void cal5_isCalendarClickMonthYear()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            string headerName = headerBtn.Name;
            DateTime currentMonth = DateTime.Today;
            int year = currentMonth.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
        }

        //test for click prev year button
        [Test]
        public void cal6_isCalendarClickPrevYear()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            string headerName = headerBtn.Name;
            DateTime prevYear = DateTime.Today.AddYears(-1);
            int year = prevYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
        }

        //test for click next year button
        [Test]
        public void cal7_isCalendarClickNextYear()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            string headerName = headerBtn.Name;
            DateTime nextYear = DateTime.Today.AddYears(1);
            int year = nextYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
        }

        //test for click year button
        [Test]
        public void cal8_isCalendarClickYear()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            string headerName = headerBtn.Name;
            DateTime currentMonth = DateTime.Today;
            int year = currentMonth.Year;
            string[] parts = headerName.Split('-');
            int yearLow = Convert.ToInt32(parts[0]);
            int yearHigh = Convert.ToInt32(parts[1]);
            Assert.That(year, Is.GreaterThan(yearLow));
            Assert.That(year, Is.LessThan(yearHigh));
        }

        //test for click previous year range button
        [Test]
        public void cal9_isCalendarClickPrevYearRange()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string headerName = headerBtn.Name;
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            string prevHeaderName = headerBtn.Name;
            DateTime currentMonth = DateTime.Today;
            int year = currentMonth.Year;
            string[] parts = headerName.Split('-');
            int yearLow = Convert.ToInt32(parts[0]);
            int yearHigh = Convert.ToInt32(parts[1]);
            string[] prevParts = prevHeaderName.Split('-');
            int prevYearLow = Convert.ToInt32(prevParts[0]);
            int prevYearHigh = Convert.ToInt32(prevParts[1]);
            int lowDiff = yearLow - prevYearLow;
            int highDiff = yearHigh - prevYearHigh;
            Assert.That(lowDiff, Is.EqualTo(10));
            Assert.That(highDiff, Is.EqualTo(10));
            //reset
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
        }

        //test for click next year range button
        [Test]
        public void cald1_isCalendarClickPrevYearRange()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string headerName = headerBtn.Name;
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            string nextHeaderName = headerBtn.Name;
            DateTime currentMonth = DateTime.Today;
            int year = currentMonth.Year;
            string[] parts = headerName.Split('-');
            int yearLow = Convert.ToInt32(parts[0]);
            int yearHigh = Convert.ToInt32(parts[1]);
            string[] nextParts = nextHeaderName.Split('-');
            int nextYearLow = Convert.ToInt32(nextParts[0]);
            int nextYearHigh = Convert.ToInt32(nextParts[1]);
            int lowDiff = nextYearLow - yearLow;
            int highDiff = nextYearHigh - yearHigh;
            Assert.That(lowDiff, Is.EqualTo(10));
            Assert.That(highDiff, Is.EqualTo(10));
            //reset
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
        }

        //test click on other month dates loads that month
        [Test]
        public void cald2_OnClickofOtherMonthDate()
        {
            AutomationElement[] dayButtons = multiSelectCalendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement dayBtn = dayButtons[3];
            string dayBtnstring = dayButtons[3].Name;
            string[] parts = dayBtnstring.Split(' ');
            headerBtn = multiSelectCalendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string headerName = headerBtn.Name;
            string[] hparts = headerName.Split(' ');
            if (parts[1] == hparts[0])
            {
                dayBtn = dayButtons[44];
                dayBtnstring = dayButtons[44].Name;
                parts = dayBtnstring.Split(' ');
            }
            if (parts[1] != hparts[0])
            {
                dayBtn.Click();
            }
            dayBtnstring = dayBtnstring.Remove(0, 3);
            AutomationElement headerBtnNew = multiSelectCalendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtnNew.Name, Is.EqualTo(dayBtnstring));
        }

        //test multiselect of dates for calendar with selection mode - MultipleRange
        [Test]
        public void cald3_AddRangeToSelectionTest()
        {
            DateTime date1 = new DateTime(2024, 3, 10);
            multiSelectCalendar.SelectDate(date1);
            DateTime date2 = new DateTime(2024, 3, 15);
            DateTime date3 = new DateTime(2024, 3, 17);
            DateTime[] dates = new DateTime[] { date2, date3 };
            multiSelectCalendar.AddRangeToSelection(dates);
            DateTime[] selectedDates = multiSelectCalendar.SelectedDates;
            Assert.That(selectedDates, Has.Length.EqualTo(3));
            Assert.That(selectedDates[0], Is.EqualTo(date1));
            Assert.That(selectedDates[1], Is.EqualTo(date2));
            Assert.That(selectedDates[2], Is.EqualTo(date3));
            calwindow.Close();
            window.Close();
        }


    }
}
