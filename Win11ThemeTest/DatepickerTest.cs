using System;
using System.Configuration;
using System.Globalization;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;


namespace Win11ThemeTest
{
    public class DatePickerTest
    {
        private readonly Application? app;
        private readonly Window? window;
        public Window? dtPickerWindow;
        readonly Button? testButton;
        readonly DateTimePicker? datePicker;
        readonly AutomationElement? calBtn;
        AutomationElement? calWidget;
        AutomationElement? headerBtn;
        AutomationElement? prevBtn;
        AutomationElement? nextBtn;
        readonly TextBox? dtTextBox;
        MenuItem? dtMenuCopy;
        MenuItem? dtMenuCut;
        MenuItem? dtMenuPaste;

        public DatePickerTest()
        {
            try
            {
                //Check if the previous windows are closed
                Win11ThemeTest.Tests tests = new Win11ThemeTest.Tests();
                tests.IfExists();
                //Launch Application
                var appPath = ConfigurationManager.AppSettings["Testpath"];                
                app = Application.Launch(appPath);
                using var automation = new UIA3Automation();
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("datepickertestbtn")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed();
                dtPickerWindow = window.FindFirstDescendant(cf => cf.ByName("DatepickerWindow")).AsWindow();
                datePicker = dtPickerWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstDatepicker")).AsDateTimePicker();
                dtTextBox = datePicker.FindFirstChild(cf => cf.ByAutomationId("PART_TextBox")).AsTextBox();
                calBtn = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
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
        
        //test if date picker is available in window
        [Test]
        public void DatePicker1_isAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(dtPickerWindow, Is.Not.Null);
                Assert.That(datePicker, Is.Not.Null);
            });
        }

        //test if date input is the selected date and the date format is dd MMMM yyyy
        [Test]
        public void DatePicker2_SelectDate()
        {
            Assert.That(datePicker, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            datePicker.SelectedDate = thisDay;
            DateTime dt_selectedDate = DateTime.Parse(datePicker.Patterns.Value.Pattern.Value);
            Assert.That(dt_selectedDate, Is.EqualTo(thisDay));
            Wait.UntilInputIsProcessed();
            Assert.That(dtTextBox, Is.Not.Null);
            DateTime txtDate = DateTime.Parse(dtTextBox.Text);
            Assert.That(txtDate.ToString("dd MMMM yyyy"), Is.EqualTo(thisDay.ToString("dd MMMM yyyy")));
        }

        //test if calendar widget appears on click of calendar icon
        [Test]
        public void DatePicker3_isCalendarClickable()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            Assert.That(calWidget, Is.Not.Null);
            calBtn.Click();
        }

        #region calendarScenarios
        //test if calendar widget has current month
        [Test]
        public void DatePicker4_calendarMonth()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string headerName = headerBtn.Name;
            string Month = thisDay.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            calBtn.Click();
        }

        //test for click previous month button
        [Test]
        public void DatePicker5_CalendarClickPrevMonth()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string oldHeaderName = headerBtn.Name;
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            string headerName = headerBtn.Name;
            string[] oldYearMonth = oldHeaderName.Split(' ');
            int currentMonth = DateTime.ParseExact(oldYearMonth[0], "MMMM", CultureInfo.CurrentCulture).Month;
            DateTime currentDate = new DateTime(Convert.ToInt32(oldYearMonth[1]), currentMonth, 1).AddMonths(-1);
            int year = currentDate.Year;
            string Month = currentDate.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.Multiple(() =>
            {
                Assert.That(yearMonth[0], Is.EqualTo(Month));
                Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));
            });
            calBtn.Click();
        }

        [Test]
        //test for click next month button
        public void DatePicker6_CalendarClickNextMonth()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string oldHeaderName = headerBtn.Name;
            nextBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
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

            //reset to current month
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            calBtn.Click();
        }

        //test for click Month-year button
        [Test]
        public void DatePicker7_CalendarClickMonthYear()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Wait.UntilInputIsProcessed();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                Wait.UntilInputIsProcessed();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            Wait.UntilInputIsProcessed();
            string headerName = headerBtn.Name;
            DateTime currentMonth = DateTime.Today;
            int year = currentMonth.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            calBtn.Click();
        }

        //test for click prev year button
        [Test]
        public void DatePicker8_CalendarClickPrevYear()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            headerBtn.Click();
            prevBtn.Click();
            string headerName = headerBtn.Name;
            DateTime prevYear = DateTime.Today.AddYears(-1);
            int year = prevYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            nextBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            calBtn.Click();
        }

        //test for click next year button
        [Test]
        public void DatePicker9_isCalendarClickNextYear()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Wait.UntilInputIsProcessed();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                Wait.UntilInputIsProcessed();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            nextBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            headerBtn.Click();
            Wait.UntilInputIsProcessed();
            nextBtn.Click();
            Wait.UntilInputIsProcessed();
            string headerName = headerBtn.Name;
            DateTime nextYear = DateTime.Today.AddYears(1);
            int year = nextYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            Wait.UntilInputIsProcessed();
            calBtn.Click();
            Wait.UntilInputIsProcessed();
        }

        //test for click year button
        [Test]
        public void DatePickers1_CalendarClickYear()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            DateTime currentMonth = DateTime.Today;
            int year = currentMonth.Year;
            string[] parts = headerName.Split('-');
            int yearLow = Convert.ToInt32(parts[0]);
            int yearHigh = Convert.ToInt32(parts[1]);
            Assert.That(year, Is.GreaterThan(yearLow));
            Assert.That(year, Is.LessThan(yearHigh));
            calBtn.Click();
        }

        //test for click previous year range button
        [Test]
        public void DatePickers2_CalendarClickPrevYearRange()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Wait.UntilInputIsProcessed();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                Wait.UntilInputIsProcessed();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            Wait.UntilInputIsProcessed();
            headerBtn.Click();
            Wait.UntilInputIsProcessed();
            string headerName = headerBtn.Name;
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            Wait.UntilInputIsProcessed();
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
            Assert.Multiple(() =>
            {
                Assert.That(lowDiff, Is.EqualTo(10));
                Assert.That(highDiff, Is.EqualTo(10));
            });
            calBtn.Click();
        }

        //test for click next year range button
        [Test]
        public void DatePickers3_CalendarClickNextYearRange()
        {
            Assert.That(calBtn, Is.Not.Null);
            calBtn.Click();
            Assert.That(datePicker, Is.Not.Null);
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.That(calWidget, Is.Not.Null);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            nextBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
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
            Assert.Multiple(() =>
            {
                Assert.That(lowDiff, Is.EqualTo(10));
                Assert.That(highDiff, Is.EqualTo(10));
            });
            calBtn.Click();
        }

        //test click on other month dates loads that month
        [Test]
        public void DatePickers4_OnClickOfOtherMonthDate()
        {
            Assert.That(calBtn, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            Assert.That(datePicker, Is.Not.Null);
            datePicker.SelectedDate = thisDay;
            Wait.UntilInputIsProcessed();
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Wait.UntilInputIsProcessed();
            AutomationElement[] dayButtons = calWidget.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement dayBtn = dayButtons[3];
            string dayBtnString = dayButtons[3].Name;
            string[] parts = dayBtnString.Split(' ');
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
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
                dayBtn.Click();
                calBtn.Click();
                Wait.UntilInputIsProcessed();
            }
            DateTime dateTime = DateTime.Parse(dayBtnString);
            dayBtnString = dateTime.ToString("MMMM yyyy");
            AutomationElement headerBtnNew = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtnNew.Name, Is.EqualTo(dayBtnString));
            calBtn.Click();
        }
        #endregion

        //test keyboard input for date- valid date
        [Test]
        public void DatePickers5_ValidKeyboardInput()
        {
            Assert.That(datePicker, Is.Not.Null);
            datePicker.Focus();
            DateTime sDate = new DateTime(2024, 09, 21);
            Keyboard.Type("2024/09/21");
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(dtTextBox, Is.Not.Null);
            DateTime txtDate = DateTime.Parse(dtTextBox.Text);
            Assert.That(txtDate.ToString("dd MMMM yyyy"), Is.EqualTo(sDate.ToString("dd MMMM yyyy")));
        }

        //test keyboard input for date- invalid date
        [Test]
        public void DatePickers6_InvalidKeyboardInput()
        {
            Assert.That(datePicker, Is.Not.Null);
            string beforeDate = datePicker.Patterns.Value.Pattern.Value;
            datePicker.Focus();
            Wait.UntilInputIsProcessed();
            Keyboard.Type("2024/20/21");
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed();
            string afterDate = datePicker.Patterns.Value.Pattern.Value;
            Assert.That(beforeDate, Is.EqualTo(afterDate));
        }

        //right click on date picker textbox-menu items copy, cut and paste
        [Test]
        public void DatePickers7_RightClick()
        {
            Assert.That(dtTextBox, Is.Not.Null);
            dtTextBox.Focus();
            dtTextBox.RightClick();
            Assert.That(dtPickerWindow, Is.Not.Null);
            dtMenuCopy = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            dtMenuPaste = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            dtMenuCut = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Cut")).AsMenuItem();
            Assert.Multiple(() =>
            {
                Assert.That(dtMenuCopy, Is.Not.Null);
                Assert.That(dtMenuPaste, Is.Not.Null);
                Assert.That(dtMenuCut, Is.Not.Null);
            });
            Mouse.Click();
        }

        //test for copy and paste menu action items
        [Test]
        public void DatePickers8_datePickerCopyPaste()
        {
            Assert.That(datePicker, Is.Not.Null);
            DateTime thisDay = DateTime.Today;
            datePicker.SelectedDate = thisDay;
            Assert.That(dtTextBox, Is.Not.Null);
            string beforeCopy = dtTextBox.Text;
            Console.WriteLine(beforeCopy);
            dtTextBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(dtPickerWindow, Is.Not.Null);
            var copyText = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            Assert.That(copyText, Is.Not.Null);
            var pattern = dtTextBox.Patterns.Text.Pattern.DocumentRange;
            pattern.Select();
            Mouse.MoveTo(copyText.GetClickablePoint());
            copyText.Click();
            copyText.Click();
            Wait.UntilInputIsProcessed();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            dtTextBox.Text = null;
            dtTextBox.RightClick();
            var pasteText = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(pasteText, Is.Not.Null);
            pasteText.Click();
            Wait.UntilInputIsProcessed();
            Assert.IsNotNull(dtTextBox.Text);
            string afterCopy = dtTextBox.Text;
            Assert.That(beforeCopy, Is.EqualTo(afterCopy));
        }

        [Test]
        public void DatePickers9_closeWindows()
        {
            Assert.That(dtPickerWindow, Is.Not.Null);
            dtPickerWindow.Focus();
            dtPickerWindow.Close();
            Assert.That(window, Is.Not.Null);
            Wait.UntilInputIsProcessed();
            Assert.That(dtPickerWindow.IsOffscreen, Is.True);
            window.Close();
            Assert.That(window.IsOffscreen, Is.True);
        }
    }
}
