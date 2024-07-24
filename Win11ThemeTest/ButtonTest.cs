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
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = LaunchApplication(appPath);
                using var automation = new UIA3Automation();
                window = app?.GetMainWindow(automation);
                testButton = window?.FindFirstDescendant(cf => cf.ByAutomationId("testbtn")).AsButton();
                ClickButton(testButton);
                
                btnWindow = window?.FindFirstDescendant(cf => cf.ByName("ButtonWindow")).AsWindow();
                button = btnWindow?.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
                disabledButton = btnWindow?.FindFirstDescendant(cf => cf.ByAutomationId("disbtn")).AsButton();

        }


        private static Application? LaunchApplication(string? appPath)
        {
            try
            {
                return Application.Launch(appPath);
            }
            catch (Exception ex)
            {
                LogException(ex);
                throw;
            }
        }

        private static void ClickButton(Button? button)
        {
            if (button == null) throw new ArgumentNullException(nameof(button));

            Mouse.Click(button.GetClickablePoint());
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
        }

        private static void LogException(Exception ex)
        {
            var filePath = ConfigurationManager.AppSettings["logpath"];
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var logFilePath = Path.Combine(filePath, $"log_{DateTime.Now:yyyyMMddHHmmss}.txt");
            using StreamWriter sw = new(logFilePath, append: true);
            sw.WriteLine("-----------Exception Details on " + DateTime.Now + "-----------------");
            sw.WriteLine("-------------------------------------------------------------------------------------");
            sw.WriteLine($"Log Written Date: {DateTime.Now}\nError Message: {ex.Message}");
        }
        //test if button is available in window
        [Test]
        public void Button1_IsButtonAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(btnWindow, Is.Not.Null);

                Assert.That(button, Is.Not.Null);
            });
        }

        //test if button is clicked
        [Test]
        public void Button2_IsClicked()
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
        public void Button3_IsClickableWithEnterKey()
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
        public void Button4_IsClickableWithSpaceKey()
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
        public void Button5_OnMouseRightClick()
        {
            Assert.That(button, Is.Not.Null);
            button.RightClick();
            Assert.That(btnWindow, Is.Not.Null);
            var popup = btnWindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //Test disabled button
        [Test]
        public void Button6_IsDisabled()
        {
            Assert.That(disabledButton, Is.Not.Null);
            Assert.That(disabledButton.IsEnabled, Is.False);
        }

        //Test disabled button
        [Test]
        public void Button7_IsDisabledClick()
        {
            Assert.That(disabledButton, Is.Not.Null);
            Assert.That(disabledButton.IsEnabled, Is.False);
            disabledButton.Click();
            var popup = disabledButton.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //close windows
        [Test]
        public void Button8_CloseWindows()
        {
            if (app != null)
            {
                app.Close();
                Assert.That(app.Close(), Is.True);
                Console.WriteLine("Application closed successfully.");
            }
            else
            {
                Console.WriteLine("Application not found.");
                Assert.Fail("Application not found.");
            }
        }

    }
}
