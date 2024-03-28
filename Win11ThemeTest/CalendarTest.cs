using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.Core.Definitions;
using System.Globalization;
using Calendar = FlaUI.Core.AutomationElements.Calendar;
using System.Configuration;

namespace Win11ThemeTest
{
    public class CalendarTest
    {
        private Application app;
        private Window window;
        public Window calWindow;
        Button testButton;
        Calendar calendar;
        Calendar multiSelectCalendar;
        AutomationElement headerBtn;
        AutomationElement prevBtn;
        AutomationElement nextBtn;

        public CalendarTest()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("calendartestbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                    calWindow = window.FindFirstDescendant(cf => cf.ByName("CalendarWindow")).AsWindow();
                    calendar = calWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCal")).AsCalendar();
                    multiSelectCalendar = calWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCal_multiSelect")).AsCalendar();
                }
            }
            catch (Exception ex)
            {
                var filePath = ConfigurationManager.AppSettings["logpath"];
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + "log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";   //Text File Name
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + "\nError Message:" + " " + ex.Message.ToString();
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(error);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        //test if calendar is available in window
        [Test]
        public void calendar1_isCalendarAvailable()
        {
            Assert.IsNotNull(calWindow);
            Assert.IsNotNull(calendar);
        }

        //test if selected date is today's date
        [Test]
        public void calendar2_isCalendarTodayDate()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            DateTime[] sDate = calendar.SelectedDates;
            Assert.That(sDate.Length, Is.EqualTo(1));
            Assert.That(sDate[0], Is.EqualTo(thisDay));
        }

        //test for click previous month button
        [Test]
        public void calendar3_isCalendarClickPrevMonth()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string oldHeaderName = headerBtn.Name;
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            string headerName = headerBtn.Name;
            string[] oldYearMonth = oldHeaderName.Split(' ');
            int currentMonth = DateTime.ParseExact(oldYearMonth[0], "MMMM", CultureInfo.CurrentCulture).Month;
            DateTime currentDate = new DateTime(Convert.ToInt32(oldYearMonth[1]), currentMonth, 1).AddMonths(-1);
            int year = currentDate.Year;
            string Month = currentDate.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));
        }

        //test for click next month button
        [Test]
        public void calendar4_isCalendarClickNextMonth()
        {
            Assert.IsNotNull(calendar);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string oldHeaderName = headerBtn.Name;
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            string headerName = headerBtn.Name;
            string[] oldYearMonth = oldHeaderName.Split(' ');
            int currentMonth = DateTime.ParseExact(oldYearMonth[0], "MMMM", CultureInfo.CurrentCulture).Month;
            DateTime currentDate = new DateTime(Convert.ToInt32(oldYearMonth[1]), currentMonth, 1).AddMonths(1);
            int year = currentDate.Year;
            string Month = currentDate.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));
        }

        //test for click Month year button
        [Test]
        public void calendar5_isCalendarClickMonthYear()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            string headerName = headerBtn.Name;
            int year = calendar.SelectedDates[0].Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset calendar
            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn = null;
            string[] monthName;
            for (int i = 3; i < monthButtons.Length; i++) // iterate through all the month buttons
            {
                monthName = monthButtons[i].Name.Split(' ');
                if (monthName[0] == calendar.SelectedDates[0].ToString("MMMM"))
                {
                    monthBtn = monthButtons[i];
                    if (monthBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }
        }

        //test for click prev year button
        [Test]
        public void calendar6_isCalendarClickPrevYear()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            string headerName = headerBtn.Name;
            DateTime prevYear = calendar.SelectedDates[0].AddYears(-1);
            int year = prevYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn = null;
            string[] monthName;
            for (int i = 3; i < monthButtons.Length; i++) // iterate through all the month buttons
            {
                monthName = monthButtons[i].Name.Split(' ');
                if (monthName[0] == calendar.SelectedDates[0].ToString("MMMM"))
                {
                    monthBtn = monthButtons[i];
                    if (monthBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }
        }

        //test for click next year button
        [Test]
        public void calendar7_isCalendarClickNextYear()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            string headerName = headerBtn.Name;
            DateTime nextYear = calendar.SelectedDates[0].AddYears(1);
            int year = nextYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn = null;
            string[] monthName;
            for (int i = 3; i < monthButtons.Length; i++) // iterate through all the month buttons
            {
                monthName = monthButtons[i].Name.Split(' ');
                if (monthName[0] == calendar.SelectedDates[0].ToString("MMMM"))
                {
                    monthBtn = monthButtons[i];
                    if (monthBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }
        }

        //test for click year button
        [Test]
        public void calendar8_isCalendarClickYear()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            DateTime currentMonth = calendar.SelectedDates[0];
            int year = currentMonth.Year;
            string[] parts = headerName.Split('-');
            int yearLow = Convert.ToInt32(parts[0]);
            int yearHigh = Convert.ToInt32(parts[1]);
            Assert.That(year, Is.GreaterThan(yearLow));
            Assert.That(year, Is.LessThan(yearHigh));
            //reset
            AutomationElement[] yearButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement yearBtn = null;
            string[] yearName;
            for (int i = 3; i < yearButtons.Length; i++) // iterate through all the month buttons
            {
                yearName = yearButtons[i].Name.Split(' ');
                if (Convert.ToInt32(yearName[0]) == calendar.SelectedDates[0].Year)
                {
                    yearBtn = yearButtons[i];
                    if (yearBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }

            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn = null;
            string[] monthName;
            for (int i = 3; i < monthButtons.Length; i++) // iterate through all the month buttons
            {
                monthName = monthButtons[i].Name.Split(' ');
                if (monthName[0] == calendar.SelectedDates[0].ToString("MMMM"))
                {
                    monthBtn = monthButtons[i];
                    if (monthBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }
        }

        //test for click previous year range button
        [Test]
        public void calendar9_isCalendarClickPrevYearRange()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            string prevHeaderName = headerBtn.Name;
            DateTime currentMonth = calendar.SelectedDates[0];
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
            AutomationElement[] yearButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement yearBtn = null;
            string[] yearName;
            for (int i = 3; i < yearButtons.Length; i++) // iterate through all the month buttons
            {
                yearName = yearButtons[i].Name.Split(' ');
                if (Convert.ToInt32(yearName[0]) == calendar.SelectedDates[0].Year)
                {
                    yearBtn = yearButtons[i];
                    if (yearBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }

            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn = null;
            string[] monthName;
            for (int i = 3; i < monthButtons.Length; i++) // iterate through all the month buttons
            {
                monthName = monthButtons[i].Name.Split(' ');
                if (monthName[0] == calendar.SelectedDates[0].ToString("MMMM"))
                {
                    monthBtn = monthButtons[i];
                    if (monthBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }
        }

        //test for click next year range button
        [Test]
        public void calendars1_isCalendarClickPrevYearRange()
        {
            Assert.IsNotNull(calendar);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            string nextHeaderName = headerBtn.Name;
            DateTime currentMonth = calendar.SelectedDates[0];
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
            AutomationElement[] yearButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement yearBtn = null;
            string[] yearName;
            for (int i = 3; i < yearButtons.Length; i++) // iterate through all the month buttons
            {
                yearName = yearButtons[i].Name.Split(' ');
                if (Convert.ToInt32(yearName[0]) == calendar.SelectedDates[0].Year)
                {
                    yearBtn = yearButtons[i];
                    if (yearBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }

            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn = null;
            string[] monthName;
            for (int i = 3; i < monthButtons.Length; i++) // iterate through all the month buttons
            {
                monthName = monthButtons[i].Name.Split(' ');
                if (monthName[0] == calendar.SelectedDates[0].ToString("MMMM"))
                {
                    monthBtn = monthButtons[i];
                    if (monthBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                    {
                        invokePattern.Invoke();
                    }
                    break;
                }
            }

        }

        //   test click on other month dates loads that month
        [Test]
        public void calendars2_OnClickOfOtherMonthDate()
        {
            AutomationElement[] dayButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement dayBtn = dayButtons[3];
            string dayBtnString = dayButtons[3].Name;
            string[] parts = dayBtnString.Split(' ');
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string headerName = headerBtn.Name;
            string[] hParts = headerName.Split(' ');
            if (parts[1] == hParts[0])
            {
                dayBtn = dayButtons[44];
                dayBtnString = dayButtons[44].Name;
                parts = dayBtnString.Split(' ');
            }
            if (parts[1] != hParts[0])
            {
                if (dayBtn.Patterns.Invoke.TryGetPattern(out var invokePattern))
                {
                    invokePattern.Invoke();
                }
            }
            dayBtnString = dayBtnString.Remove(0, 3);
            AutomationElement headerBtnNew = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtnNew.Name, Is.EqualTo(dayBtnString));
        }

        //test multiselect of dates for calendar with selection mode - MultipleRange
        [Test]
        public void calendars3_AddRangeToSelectionTest()
        {
            DateTime date1 = new DateTime(2024, 3, 10);
            multiSelectCalendar.SelectDate(date1);
            DateTime date2 = new DateTime(2024, 3, 15);
            DateTime date3 = new DateTime(2024, 3, 17);
            DateTime[] dates = [date2, date3];
            multiSelectCalendar.AddRangeToSelection(dates);
            DateTime[] selectedDates = multiSelectCalendar.SelectedDates;
            Assert.That(selectedDates, Has.Length.EqualTo(3));
            Assert.That(selectedDates[0], Is.EqualTo(date1));
            Assert.That(selectedDates[1], Is.EqualTo(date2));
            Assert.That(selectedDates[2], Is.EqualTo(date3));
        }

        [Test]
        public void calendars4_closeWindows()
        {
            calWindow.Focus();
            calWindow.Close();
            Assert.IsTrue(calWindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }
    }
}
