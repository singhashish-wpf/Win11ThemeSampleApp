using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using NUnit.Framework.Internal;
using System.Configuration;

namespace Win11ThemeTest
{
    public class ButtonTest
    {
        private Application? app;
        private Window? window;
        public Window? btnWindow;
        Button? testButton;
        Button? button;
        Button? disabledButton;

        public ButtonTest()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                    btnWindow = window.FindFirstDescendant(cf => cf.ByName("ButtonWindow")).AsWindow();
                    button = btnWindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
                    disabledButton = btnWindow.FindFirstDescendant(cf => cf.ByAutomationId("disbtn")).AsButton();
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

        //test if button is available in window
        [Test]
        public void button1_isButtonAvailable()
        {
            Assert.IsNotNull(btnWindow);
            Assert.IsNotNull(button);
        }

        //test if button is clicked
        [Test]
        public void button2_isClicked()
        {
            Assert.IsNotNull(btnWindow);
            Assert.IsNotNull(button);
            button.Click();
            Wait.UntilInputIsProcessed();
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnWindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test if button clicked with enter key
        [Test]
        public void button3_isClickableWithEnterKey()
        {
            Assert.IsNotNull(btnWindow);
            Assert.IsNotNull(button);
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnWindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test if button clicked with space key
        [Test]
        public void button4_isClickableWithSpaceKey()
        {
            Assert.IsNotNull(btnWindow);
            Assert.IsNotNull(button);
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Wait.UntilInputIsProcessed();
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnWindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test no action on mouse right click on button
        [Test]
        public void button5_onMouseRightClick()
        {
            Assert.IsNotNull(btnWindow);
            Assert.IsNotNull(button);
            button.RightClick();
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //Test disabled button
        [Test]
        public void button6_isDisabled()
        {
            Assert.IsNotNull(disabledButton);
            Assert.That(disabledButton.IsEnabled, Is.False);
        }

        //Test disabled button
        [Test]
        public void button7_isDisabledClick()
        {
            Assert.IsNotNull(disabledButton);
            Assert.That(disabledButton.IsEnabled, Is.False);
            disabledButton.Click();
            var popup = disabledButton.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //close windows
        [Test]
        public void button8_closeWindows()
        {
            Assert.IsNotNull(btnWindow);
            Assert.IsNotNull(window);
            btnWindow.Close();
            Assert.IsTrue(btnWindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }

    }
}
