using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar;


namespace Win11ThemeTest
{
    public class DatepickerTest
    {
        private Application app;
        private Window window;
        public Window dtPickerWindow;
        Button testButton;
        DateTimePicker datepicker;
        AutomationElement calBtn;
        AutomationElement calWidget;
        AutomationElement headerBtn;
        AutomationElement prevBtn;
        AutomationElement nextBtn;
        TextBox dtTextBox;
        MenuItem dtmenuCopy;
        MenuItem dtmenuCut;
        MenuItem dtmenuPaste;

        public DatepickerTest()
        {
            try
            {
                // app = Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("datepickertestbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed();
                    dtPickerWindow = window.FindFirstDescendant(cf => cf.ByName("DatepickerWindow")).AsWindow();
                    datepicker = dtPickerWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstDatepicker")).AsDateTimePicker();
                    dtTextBox = datepicker.FindFirstChild(cf => cf.ByAutomationId("PART_TextBox")).AsTextBox();
                    // dtPickerWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstDatepicker")).
                    calBtn = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Button));

                }
            }
            catch (Exception ex)
            {
                var filepath = ConfigurationManager.AppSettings["logpath"];
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + "log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
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

        //test if datepicker is available in window
        [Test]
        public void datepicker1_isAvailable()
        {
            Assert.IsNotNull(dtPickerWindow);
            Assert.IsNotNull(datepicker);
        }

        //test if date input is the selected date and the date format is dd MMMM yyyy
        [Test]
        public void datepicker2_Selectdate()
        {
            DateTime thisDay = DateTime.Today;
            datepicker.SelectedDate = thisDay;
            DateTime dt_selectedDate = DateTime.Parse(datepicker.Patterns.Value.Pattern.Value);
            Assert.That(dt_selectedDate, Is.EqualTo(thisDay));
            Assert.That(dtTextBox.Text, Is.EqualTo(thisDay.ToString("dd MMMM yyyy")));

        }

        //test if calendar widget appears on click of calendar icon
        [Test]
        public void datepicker3_isCalendarClickable()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            Assert.IsNotNull(calWidget);
            calBtn.Click();
        }

        #region calendarScenarios
        //test if calendar widget has current month
        [Test]
        public void datepicker4_calendarMonth()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
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
        public void datepicker5_CalendarClickprevMonth()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string oldheaderName = headerBtn.Name;
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            string headerName = headerBtn.Name;
            string[] oldyearMonth = oldheaderName.Split(' ');
            int currentMonth = DateTime.ParseExact(oldyearMonth[0], "MMMM", CultureInfo.CurrentCulture).Month;
            DateTime currentdate = new DateTime(Convert.ToInt32(oldyearMonth[1]), currentMonth, 1).AddMonths(-1);
            int year = currentdate.Year;
            string Month = currentdate.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));

            //reset to current month
            //nextBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            //nextBtn.Click();
            calBtn.Click();
        }

        [Test]
        //test for click next month button
        public void datepicker6_CalendarClickNextMonth()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Assert.IsNotNull(calWidget);
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            string oldheaderName = headerBtn.Name;
            nextBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_NextButton"));
            nextBtn.Click();
            string headerName = headerBtn.Name;
            string[] oldyearMonth = oldheaderName.Split(' ');
            int currentMonth = DateTime.ParseExact(oldyearMonth[0], "MMMM", CultureInfo.CurrentCulture).Month;
            DateTime currentdate = new DateTime(Convert.ToInt32(oldyearMonth[1]), currentMonth, 1).AddMonths(1);
            int year = currentdate.Year;
            string Month = currentdate.ToString("MMMM");
            string[] yearMonth = headerName.Split(' ');
            Assert.That(yearMonth[0], Is.EqualTo(Month));
            Assert.That(Convert.ToInt32(yearMonth[1]), Is.EqualTo(year));

            //reset to current month
            prevBtn = calWidget.FindFirstChild(cf => cf.ByControlType(ControlType.Button));
            prevBtn.Click();
            calBtn.Click();
        }

        //test for click Monthyear button
        [Test]
        public void datepicker7_CalendarClickMonthYear()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
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
        public void datepicker8_CalendarClickPrevYear()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
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
        public void datepicker9_isCalendarClickNextYear()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
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
        public void datepickers1_CalendarClickYear()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
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
        public void datepickers2_CalendarClickPrevYearRange()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
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
        public void datepickers3_CalendarClickNextYearRange()
        {
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
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
        public void datepickers4_OnClickofOtherMonthDate()
        {
            DateTime thisDay = DateTime.Today;
            datepicker.SelectedDate = thisDay;
            Wait.UntilInputIsProcessed();
            calBtn.Click();
            calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            if (calWidget == null)
            {
                calBtn.Click();
                calWidget = datepicker.FindFirstChild(cf => cf.ByControlType(ControlType.Calendar));
            }
            Wait.UntilInputIsProcessed();
            AutomationElement[] dayButtons = calWidget.FindAllChildren(cf => cf.ByControlType(ControlType.Button));
            AutomationElement dayBtn = dayButtons[3];
            string dayBtnstring = dayButtons[3].Name;
            string[] parts = dayBtnstring.Split(' ');
            headerBtn = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
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
                calBtn.Click();
                Wait.UntilInputIsProcessed();
            }
            dayBtnstring = dayBtnstring.Remove(0, 3);
            AutomationElement headerBtnNew = calWidget.FindFirstChild(cf => cf.ByAutomationId("PART_HeaderButton"));
            Assert.That(headerBtnNew.Name, Is.EqualTo(dayBtnstring));
            calBtn.Click();
        }
        #endregion

        //test keyboard input for date- valid date
        [Test]
        public void datepickers5_datepickerValidKeyboardInput()
        {
            datepicker.Focus();
            DateTime sdate = new DateTime(2024, 09, 21);
            Keyboard.Type("2024/09/21");
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(dtTextBox.Text, Is.EqualTo(sdate.ToString("dd MMMM yyyy")));
        }

        //test keyboard input for date- invalid date
        [Test]
        public void datepickers6_datepickerInvalidKeyboardInput()
        {
            string beforeDate = datepicker.Patterns.Value.Pattern.Value;
            datepicker.Focus();
            Wait.UntilInputIsProcessed();
            Keyboard.Type("2024/20/21");
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed();
            string afterdate = datepicker.Patterns.Value.Pattern.Value;
            Assert.That(beforeDate, Is.EqualTo(afterdate));
        }

        //right click on datepicker textbox-menu items copy, cut and paste
        [Test]
        public void datepickers7_datepickerRightClick()
        {
            dtTextBox.Focus();
            dtTextBox.RightClick();
            dtmenuCopy = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            dtmenuPaste = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            dtmenuCut = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Cut")).AsMenuItem();
            Assert.IsNotNull(dtmenuCopy);
            Assert.IsNotNull(dtmenuPaste);
            Assert.IsNotNull(dtmenuCut);
            Mouse.Click();
        }

        //test for copy and paste menu action items
        [Test]
        public void datepickers8_datepickerCopyPaste()
        {
            DateTime thisDay = DateTime.Today;
            datepicker.SelectedDate = thisDay;
            string beforeCopy = dtTextBox.Text;
            Console.WriteLine(beforeCopy);

            dtTextBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            var copyText = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            Assert.IsNotNull(copyText);
            var pattern = dtTextBox.Patterns.Text.Pattern.DocumentRange;
            pattern.Select();
            //  Mouse.MoveBy(-3, 0);
            Wait.UntilInputIsProcessed();
            //copyText.GetClickablePoint();
            copyText.Click();
            Mouse.DoubleClick();
            Wait.UntilInputIsProcessed();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            dtTextBox.Text = null;
            dtTextBox.RightClick();
            var pasteText = dtPickerWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.IsNotNull(pasteText);
            pasteText.Click();
            Wait.UntilInputIsProcessed();
            string afterCopy = dtTextBox.Text;
            Assert.That(beforeCopy, Is.EqualTo(afterCopy));
        }

        [Test]
        public void datepickers9_closeWindows()
        {
            dtPickerWindow.Focus();
            dtPickerWindow.Close();
            Assert.IsTrue(dtPickerWindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }
    }
}
