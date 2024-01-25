using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.UIA3.Identifiers;
using FlaUI.UIA3.Patterns;
using System.Drawing;
using System.Linq;
using NuGet.Frameworks;


namespace Win11ThemeTest
{
    public class ButtonTest
    {
        private Application app;
        private Window window;
        //  private Window window2;
        public Window btnwindow;
        Button testButton;
        Button button;
        Button disabledbutton;
        // Button iconbutton;

        public ButtonTest()
        {

            app = Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");

            using (var automation = new UIA3Automation())
            {
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testbtn")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                btnwindow = window.FindFirstDescendant(cf => cf.ByName("ButtonWindow")).AsWindow();

            }

        }

        //test if button is available in window
        [Test]
        public void btn1_isButtonAvailable()
        {
            Assert.IsNotNull(btnwindow);
            button = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
            Assert.IsNotNull(button);
        }

        //test if button is clicked
        [Test]
        public void btn2_isClicked()
        {
            // button = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
            button.Click();
            Wait.UntilInputIsProcessed();
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnwindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //test if button clicked with enter key
        [Test]
        public void btn3_isClickableWithEnterKey()
        {

            // button = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.ENTER);
            // Wait.UntilInputIsProcessed();
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnwindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();

        }

        //test if button clicked with space key
        [Test]
        public void btn4_isClickableWithSpaceKey()
        {

            //  button = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
            button.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Wait.UntilInputIsProcessed();
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Not.Null);
            Button pBtn = btnwindow.FindFirstDescendant(cf => cf.ByName("OK")).AsButton();
            pBtn.Click();
        }

        //[Test]
        //public void btn_backgroundColor()
        //{

        //}

        //[Test]
        //public void btn_foregroundColor()
        //{

        //}

        //[Test]
        //public void btn_fontFamily()
        //{


        //}

        //[Test]
        //public void btn_onMouseHover()
        //{
        //    button = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
        //    Mouse.MoveTo(button.GetClickablePoint());


        //}

        //test no action on mouse right click on button
        [Test]
        public void btn5_onMouseRightclick()
        {
            // button = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("btn")).AsButton();
            button.RightClick();
            var popup = btnwindow.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }

        //Test disabled button
        [Test]
        public void btn6_isDisabled()
        {
            disabledbutton = btnwindow.FindFirstDescendant(cf => cf.ByAutomationId("disbtn")).AsButton();
            Assert.IsNotNull(disabledbutton);
            Assert.That(disabledbutton.IsEnabled,Is.False);
        }

        //Test disabled button
        [Test]
        public void btn7_isDisabledClick()
        {         
            Assert.That(disabledbutton.IsEnabled, Is.False);
            disabledbutton.Click();
            var popup = disabledbutton.FindFirstDescendant(cf => cf.ByName("Button Clicked")).AsWindow();
            Assert.That(popup, Is.Null);
        }
    }
}
