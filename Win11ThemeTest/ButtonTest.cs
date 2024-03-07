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
        private Application app;
        private Window window;
        public Window btnwindow;
        Button testButton;
        Button button;
        Button disabledbutton;

        public ButtonTest()
        {

            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                // app = Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");

                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                    btnwindow = window.FindFirstDescendant(cf => cf.ByName("ButtonWindow")).AsWindow();
                    button = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
                    disabledbutton = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("disbtn")).AsButton();
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

        //test if button is available in window
        [Test]
        public void button1_isButtonAvailable()
        {
            Assert.IsNotNull(btnwindow);
            Assert.IsNotNull(button);
        }

        //test if button is clicked
        [Test]
        public void button2_isClicked()
        {
            button.Click();
            Wait.UntilInputIsProcessed();
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnwindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test if button clicked with enter key
        [Test]
        public void button3_isClickableWithEnterKey()
        {
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnwindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test if button clicked with space key
        [Test]
        public void button4_isClickableWithSpaceKey()
        {
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Wait.UntilInputIsProcessed();
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnwindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test no action on mouse right click on button
        [Test]
        public void button5_onMouseRightclick()
        {
            button.RightClick();
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //Test disabled button
        [Test]
        public void button6_isDisabled()
        {
            Assert.IsNotNull(disabledbutton);
            Assert.That(disabledbutton.IsEnabled, Is.False);
        }

        //Test disabled button
        [Test]
        public void button7_isDisabledClick()
        {
            Assert.That(disabledbutton.IsEnabled, Is.False);
            disabledbutton.Click();
            var popup = disabledbutton.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //close windows
        [Test]
        public void button8_closeWindows()
        {
            btnwindow.Close();
            Assert.IsTrue(btnwindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }

    }
}
