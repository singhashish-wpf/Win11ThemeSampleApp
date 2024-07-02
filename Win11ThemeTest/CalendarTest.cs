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
        private readonly Application? app;
        private readonly Window? window;
        public Window? calWindow;
        readonly Button? testButton;
        readonly Calendar? calendar;
        readonly Calendar? multiSelectCalendar;
        AutomationElement? headerBtn;
        AutomationElement? prevBtn;
        AutomationElement? nextBtn;

        public CalendarTest()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using var automation = new UIA3Automation();
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("calendartestbtn")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                calWindow = window.FindFirstDescendant(cf => cf.ByName("CalendarWindow")).AsWindow();
                calendar = calWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCal")).AsCalendar();
                multiSelectCalendar = calWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCal_multiSelect")).AsCalendar();

            }
            catch (Exception ex)
            {
                var filePath = ConfigurationManager.AppSettings["logpath"];
                if (filePath != null)
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath = filePath + "log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";   //Text File Name
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Dispose();
                    }
                    using StreamWriter sw = File.AppendText(filePath);
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + "\nError Message:" + " " + ex.Message.ToString();
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(error);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
        }

        //test if calendar is available in window
        [Test]
        public void Calendar1_isCalendarAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(calWindow, Is.Not.Null);
                Assert.That(calendar, Is.Not.Null);
            });
        }

        //test if selected date is today's date
        [Test]
        public void Calendar2_isCalendarTodayDate()
        {
            Assert.That(calendar, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            DateTime[] sDate = calendar.SelectedDates;
            Assert.That(sDate.Length, Is.EqualTo(1));
            Assert.That(sDate[0], Is.EqualTo(thisDay));
        }

        //test for click previous month button
        [Test]
        public void Calendar3_isCalendarClickPrevMonth()
        {
            Assert.That(calendar, Is.Not.Null);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
            string oldHeaderName = headerBtn.Name;
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            Assert.That(prevBtn, Is.Not.Null);
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
        public void Calendar4_isCalendarClickNextMonth()
        {
            Assert.That(calendar, Is.Not.Null);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
            string oldHeaderName = headerBtn.Name;
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            Assert.That(nextBtn, Is.Not.Null);
            nextBtn.Click();
            string headerName = headerBtn.Name;
            string[] oldYearMonth = oldHeaderName.Split(' ');
            int currentMonth = DateTime.ParseExact(oldYearMonth[0], "MMMM", CultureInfo.CurrentCulture).Month;
            DateTime currentDate = new DateTime(Convert.ToInt32(oldYearMonth[1]), currentMonth, 1).AddMonths(1);
            int year = currentDate.Year;
            string Month = currentDate.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.Multiple(() =>
            {
                Assert.That(yearMonth[0], Is.EqualTo(Month));
                Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));
            });
        }

        //test for click Month year button
        [Test]
        public void Calendar5_isCalendarClickMonthYear()
        {
            Assert.That(calendar, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
            headerBtn.Click();
            string headerName = headerBtn.Name;
            int year = calendar.SelectedDates[0].Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset calendar
            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn;
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
        public void Calendar6_isCalendarClickPrevYear()
        {
            Assert.That(calendar, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
            headerBtn.Click();
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            Assert.That(prevBtn, Is.Not.Null);
            prevBtn.Click();
            string headerName = headerBtn.Name;
            DateTime prevYear = calendar.SelectedDates[0].AddYears(-1);
            int year = prevYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            Assert.That(nextBtn, Is.Not.Null);
            nextBtn.Click();
            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn;
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
        public void Calendar7_isCalendarClickNextYear()
        {
            Assert.That(calendar, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
            headerBtn.Click();
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            Assert.That(nextBtn, Is.Not.Null);
            nextBtn.Click();
            string headerName = headerBtn.Name;
            DateTime nextYear = calendar.SelectedDates[0].AddYears(1);
            int year = nextYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            Assert.That(prevBtn, Is.Not.Null);
            prevBtn.Click();
            AutomationElement[] monthButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement monthBtn;
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
        public void Calendar8_isCalendarClickYear()
        {
            Assert.That(calendar, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
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
            AutomationElement yearBtn;
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
            AutomationElement monthBtn;
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
        public void Calendar9_isCalendarClickPrevYearRange()
        {
            Assert.That(calendar, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            Assert.That(prevBtn, Is.Not.Null);
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
            Assert.Multiple(() =>
            {
                Assert.That(lowDiff, Is.EqualTo(10));
                Assert.That(highDiff, Is.EqualTo(10));
            });
            //reset
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            Assert.That(nextBtn, Is.Not.Null);
            nextBtn.Click();
            AutomationElement[] yearButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement yearBtn;
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
            AutomationElement monthBtn;
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
        public void Calendars1_isCalendarClickPrevYearRange()
        {
            Assert.That(calendar, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            calendar.SelectDate(thisDay);
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            nextBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            Assert.That(nextBtn, Is.Not.Null);
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
            Assert.Multiple(() =>
            {
                Assert.That(lowDiff, Is.EqualTo(10));
                Assert.That(highDiff, Is.EqualTo(10));
            });
            //reset
            prevBtn = calendar.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            Assert.That(prevBtn, Is.Not.Null);
            prevBtn.Click();
            AutomationElement[] yearButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement yearBtn;
            string[] yearName;
            for (int i = 3; i < yearButtons.Length; i++) // iterate through all the year buttons
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
            AutomationElement monthBtn;
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
        public void Calendars2_OnClickOfOtherMonthDate()
        {
            Assert.That(calendar, Is.Not.Null);
            AutomationElement[] dayButtons = calendar.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement dayBtn = dayButtons[3];
            string dayBtnString = dayButtons[3].Name;
            string[] parts = dayBtnString.Split(' ');
            headerBtn = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtn, Is.Not.Null);
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
            DateTime dateTime = DateTime.Parse(dayBtnString);
            dayBtnString = dateTime.ToString("MMMM yyyy");
            AutomationElement headerBtnNew = calendar.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtnNew.Name, Is.EqualTo(dayBtnString));
        }

        //test multiselect of dates for calendar with selection mode - MultipleRange
        [Test]
        public void Calendars3_AddRangeToSelectionTest()
        {
            Assert.That(multiSelectCalendar, Is.Not.Null);
            DateTime date1 = new DateTime(2024, 3, 10);
            multiSelectCalendar.SelectDate(date1);
            DateTime date2 = new DateTime(2024, 3, 15);
            DateTime date3 = new DateTime(2024, 3, 17);
            DateTime[] dates = [date2, date3];
            multiSelectCalendar.AddRangeToSelection(dates);
            DateTime[] selectedDates = multiSelectCalendar.SelectedDates;
            Assert.That(selectedDates, Has.Length.EqualTo(3));
            Assert.Multiple(() =>
            {
                Assert.That(selectedDates[0], Is.EqualTo(date1));
                Assert.That(selectedDates[1], Is.EqualTo(date2));
                Assert.That(selectedDates[2], Is.EqualTo(date3));
            });
        }

        [Test]
        public void Calendars4_closeWindows()
        {
            if (app != null)
            {
                app.Close();
                Console.WriteLine("Application closed successfully.");
                Assert.That(app.Close());
            }
            else
            {
                Console.WriteLine("Application not found.");
                Assert.That(app.Close());
            }
        }
    }
}
