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
        private readonly Application? app;
        private readonly Window? window;
        public Window? btnWindow;
        readonly Button? testButton;
        readonly Button? button;
        readonly Button? disabledButton;

        public ButtonTest()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using var automation = new UIA3Automation();
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testbtn")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                btnWindow = window.FindFirstDescendant(cf => cf.ByName("ButtonWindow")).AsWindow();
                button = btnWindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
                disabledButton = btnWindow.FindFirstDescendant(cf => cf.ByAutomationId("disbtn")).AsButton();
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

        //test if button is available in window
        [Test]
        public void Button1_isButtonAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(btnWindow, Is.Not.Null);

                Assert.That(button, Is.Not.Null);
            });
        }

        //test if button is clicked
        [Test]
        public void Button2_isClicked()
        {
            Assert.That(button, Is.Not.Null);
            button.Click();
            Wait.UntilInputIsProcessed();
            Assert.That(btnWindow, Is.Not.Null);
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnWindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test if button clicked with enter key
        [Test]
        public void Button3_isClickableWithEnterKey()
        {
            Assert.That(button, Is.Not.Null);
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Assert.That(btnWindow, Is.Not.Null);
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnWindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test if button clicked with space key
        [Test]
        public void Button4_isClickableWithSpaceKey()
        {
            Assert.That(button, Is.Not.Null);
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Wait.UntilInputIsProcessed();
            Assert.That(btnWindow, Is.Not.Null);
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnWindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test no action on mouse right click on button
        [Test]
        public void Button5_onMouseRightClick()
        {
            Assert.That(button, Is.Not.Null);
            button.RightClick();
            Assert.That(btnWindow, Is.Not.Null);
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //Test disabled button
        [Test]
        public void Button6_isDisabled()
        {
            Assert.That(disabledButton, Is.Not.Null);
            Assert.That(disabledButton.IsEnabled, Is.False);
        }

        //Test disabled button
        [Test]
        public void Button7_isDisabledClick()
        {
            Assert.That(disabledButton, Is.Not.Null);
            Assert.That(disabledButton.IsEnabled, Is.False);
            disabledButton.Click();
            var popup = disabledButton.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //close windows
        [Test]
        public void Button8_closeWindows()
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
