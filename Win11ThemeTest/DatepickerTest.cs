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
        private Application? app;
        private Window? window;
        public Window? dtPickerWindow;
        Button? testButton;
        DateTimePicker? datePicker;
        AutomationElement? calBtn;
        AutomationElement? calWidget;
        AutomationElement? headerBtn;
        AutomationElement? prevBtn;
        AutomationElement? nextBtn;
        TextBox? dtTextBox;
        MenuItem? dtMenuCopy;
        MenuItem? dtMenuCut;
        MenuItem? dtMenuPaste;

        public DatePickerTest()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("datepickertestbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed();
                    dtPickerWindow = window.FindFirstDescendant(cf => cf.ByName("DatepickerWindow")).AsWindow();
                    datePicker = dtPickerWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstDatepicker")).AsDateTimePicker();
                    dtTextBox = datePicker.FindFirstChild(cf => cf.ByAutomationId("PART_TextBox")).AsTextBox();
                    calBtn = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Button));

                }
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
                else
                {
                    throw new ArgumentNullException();
                }

            }

        }

        //test if date picker is available in window
        [Test]
        public void datePicker1_isAvailable()
        {
            Assert.IsNotNull(dtPickerWindow);
            Assert.IsNotNull(datePicker);
        }

        //test if date input is the selected date and the date format is dd MMMM yyyy
        [Test]
        public void datePicker2_SelectDate()
        {
            Assert.IsNotNull(datePicker);
            DateTime thisDay = DateTime.Today;
            datePicker.SelectedDate = thisDay;
            DateTime dt_selectedDate = DateTime.Parse(datePicker.Patterns.Value.Pattern.Value);
            Assert.That(dt_selectedDate, Is.EqualTo(thisDay));
            Assert.IsNotNull(dtTextBox);
            DateTime txtDate= DateTime.Parse(dtTextBox.Text);
            Assert.That(txtDate.ToString("dd MMMM yyyy"), Is.EqualTo(thisDay.ToString("dd MMMM yyyy")));

        }

        //test if calendar widget appears on click of calendar icon
        [Test]
        public void datePicker3_isCalendarClickable()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            Assert.IsNotNull(calWidget);
            calBtn.Click();
        }

        #region calendarScenarios
        //test if calendar widget has current month
        [Test]
        public void datePicker4_calendarMonth()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
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
        public void datePicker5_CalendarClickPrevMonth()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
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
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));
            calBtn.Click();
        }

        [Test]
        //test for click next month button
        public void datePicker6_CalendarClickNextMonth()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
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
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));

            //reset to current month
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            calBtn.Click();
        }

        //test for click Month-year button
        [Test]
        public void datePicker7_CalendarClickMonthYear()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            string headerName = headerBtn.Name;
            DateTime currentMonth = DateTime.Today;
            int year = currentMonth.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            calBtn.Click();
        }

        //test for click prev year button
        [Test]
        public void datePicker8_CalendarClickPrevYear()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
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
        public void datePicker9_isCalendarClickNextYear()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            nextBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            headerBtn.Click();
            nextBtn.Click();
            string headerName = headerBtn.Name;
            DateTime nextYear = DateTime.Today.AddYears(1);
            int year = nextYear.Year;
            Assert.That(Convert.ToInt32(headerName), Is.EqualTo(year));
            //reset
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            calBtn.Click();
        }

        //test for click year button
        [Test]
        public void datePickers1_CalendarClickYear()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
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
        public void datePickers2_CalendarClickPrevYearRange()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            headerBtn.Click();
            headerBtn.Click();
            string headerName = headerBtn.Name;
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
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
            calBtn.Click();
        }

        //test for click next year range button
        [Test]
        public void datePickers3_CalendarClickNextYearRange()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            calBtn.Click();
            calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datePicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
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
            Assert.That(lowDiff, Is.EqualTo(10));
            Assert.That(highDiff, Is.EqualTo(10));
            calBtn.Click();
        }

        //test click on other month dates loads that month
        [Test]
        public void datePickers4_OnClickOfOtherMonthDate()
        {
            Assert.IsNotNull(calBtn);
            Assert.IsNotNull(datePicker);
            DateTime thisDay = DateTime.Today;
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
        public void datePickers5_ValidKeyboardInput()
        {
            Assert.IsNotNull(datePicker);
            datePicker.Focus();
            DateTime sDate = new DateTime(2024, 09, 21);
            Keyboard.Type("2024/09/21");
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.IsNotNull(dtTextBox);
            DateTime txtDate = DateTime.Parse(dtTextBox.Text);
            Assert.That(txtDate.ToString("dd MMMM yyyy"), Is.EqualTo(sDate.ToString("dd MMMM yyyy")));
        }

        //test keyboard input for date- invalid date
        [Test]
        public void datePickers6_InvalidKeyboardInput()
        {
            Assert.IsNotNull(datePicker);
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
        public void datePickers7_RightClick()
        {
            Assert.IsNotNull(dtTextBox);
            Assert.IsNotNull(dtPickerWindow);
            dtTextBox.Focus();
            dtTextBox.RightClick();
            dtMenuCopy = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            dtMenuPaste = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            dtMenuCut = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Cut")).AsMenuItem();
            Assert.IsNotNull(dtMenuCopy);
            Assert.IsNotNull(dtMenuPaste);
            Assert.IsNotNull(dtMenuCut);
            Mouse.Click();
        }

        //test for copy and paste menu action items
        [Test]
        public void datePickers8_datePickerCopyPaste()
        {
            Assert.IsNotNull(datePicker);
            Assert.IsNotNull(dtPickerWindow);
            DateTime thisDay = DateTime.Today;
            datePicker.SelectedDate = thisDay;
            Assert.IsNotNull(dtTextBox);
            string beforeCopy = dtTextBox.Text;
            Console.WriteLine(beforeCopy);

            dtTextBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            var copyText = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            Assert.IsNotNull(copyText);
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
            Assert.IsNotNull(pasteText);
            pasteText.Click();
            Wait.UntilInputIsProcessed();
            Assert.IsNotNull(dtTextBox.Text);
            string afterCopy = dtTextBox.Text;
            Assert.That(beforeCopy, Is.EqualTo(afterCopy));
        }

        [Test]
        public void datePickers9_closeWindows()
        {
            Assert.IsNotNull(dtPickerWindow);
            Assert.IsNotNull(window);
            dtPickerWindow.Focus();
            dtPickerWindow.Close();
            Assert.IsTrue(dtPickerWindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }
    }
}
