using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using System.Text;

namespace Win11ThemeTest
{
    [TestFixture]
    public class rbTests
    {
        private Application app;
        private Window window;
        RadioButton radioButton1;
        RadioButton radioButton2;
        RadioButton radioButton3;
        RadioButton radioButton4;
        RadioButton radioButton5;
        RadioButton radioButton6;

        public rbTests()
        {
            app = Application.Launch(@"..\\..\\..\\..\\Win11ThemeSampleApp\\bin\\x64\\Debug\\net9.0-windows\\win-x64\\Win11ThemeSampleApp.exe");

            using (var automation = new UIA3Automation())
            {
                window = app.GetMainWindow(automation);
            }

            radioButton1 = window.FindFirstDescendant(cf => cf.ByName("RadioButton1")).AsRadioButton();
            radioButton2 = window.FindFirstDescendant(cf => cf.ByName("RadioButton2")).AsRadioButton();
            radioButton3 = window.FindFirstDescendant(cf => cf.ByName("RadioButton3")).AsRadioButton();
            radioButton4 = window.FindFirstDescendant(cf => cf.ByName("RadioButton4")).AsRadioButton();
            radioButton5 = window.FindFirstDescendant(cf => cf.ByName("RadioButton5")).AsRadioButton();
            radioButton6 = window.FindFirstDescendant(cf => cf.ByName("RadioButton6")).AsRadioButton();
        }

        [Test]
        public void RadioButtonTest_1()
        {
            // Checking if a radio button in both the groups are clickable
            Assert.That(radioButton1.IsChecked, Is.False);
            Assert.That(radioButton2.IsChecked, Is.False);
            Assert.That(radioButton3.IsChecked, Is.False);
            Assert.That(radioButton4.IsChecked, Is.False);
            Assert.That(radioButton5.IsChecked, Is.False);
            Assert.That(radioButton6.IsChecked, Is.False);

            Mouse.Click(radioButton1.GetClickablePoint());
            Mouse.Click(radioButton5.GetClickablePoint());

            Assert.That(radioButton1.IsChecked, Is.True);
            Assert.That(radioButton2.IsChecked, Is.False);
            Assert.That(radioButton3.IsChecked, Is.False);
            Assert.That(radioButton4.IsChecked, Is.False);
            Assert.That(radioButton5.IsChecked, Is.True);
            Assert.That(radioButton6.IsChecked, Is.False);
        }

        [Test]
        public void RadioButtonTest_2()
        {
            // Re-checking on a checked radio button does not affect it's checked state
            Assert.That(radioButton1.IsChecked, Is.True);
            Assert.That(radioButton5.IsChecked, Is.True);

            Mouse.Click(radioButton1.GetClickablePoint());
            Mouse.Click(radioButton5.GetClickablePoint());
            
            Assert.That(radioButton1.IsChecked, Is.True);
            Assert.That(radioButton5.IsChecked, Is.True);
        }

        [Test]
        public void RadioButtonTest_3()
        {
            // Checking a different radio button
            Assert.That(radioButton1.IsChecked, Is.True);
            Assert.That(radioButton2.IsChecked, Is.False);
            Assert.That(radioButton3.IsChecked, Is.False);

            Mouse.Click(radioButton2.GetClickablePoint());

            Assert.That(radioButton1.IsChecked, Is.False);
            Assert.That(radioButton2.IsChecked, Is.True);
            Assert.That(radioButton3.IsChecked, Is.False);
        }

        [Test]
        public void RadioButtonTest_4()
        {
            // Checking or Selecting a disabled radio button
            Assert.That(radioButton3.IsChecked, Is.False);
            Mouse.Click(radioButton3.GetClickablePoint());
            Assert.That(radioButton3.IsChecked, Is.False);

            radioButton1.Focus();

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            /** 
             * 
             * 
             * TODO: Implement Focus Testing
             * Verify that the currently focused radiobutton is not radiobutton3
             * 
             */
            
        }

        [Test]
        public void RadioButtonTest_5()
        {
            // Test to confirm click in one group does not affect checked radio button in another group
            Assert.That(radioButton2.IsChecked, Is.True);

            Mouse.Click(radioButton4.GetClickablePoint());
            Mouse.Click(radioButton5.GetClickablePoint());
            Mouse.Click(radioButton6.GetClickablePoint());

            Assert.That(radioButton2.IsChecked, Is.True);
        }

        [Test]
        public void RadioButtonTest_6()
        {
            // Test to confirm radio buttons can be checked using space bar
            Assert.That(radioButton1.IsChecked, Is.False);
            Assert.That(radioButton2.IsChecked, Is.True);

            radioButton2.Focus();

            Keyboard.TypeSimultaneously(FlaUI.Core.WindowsAPI.VirtualKeyShort.SHIFT, FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);

            Assert.That(radioButton1.IsChecked, Is.True);
            Assert.That(radioButton2.IsChecked, Is.False);
        }

        [Test]
        public void RadioButtonTest_7()
        {
            // Test to confirm correct initialization from code behind
            Assert.That(radioButton4.IsEnabled, Is.True);
            Assert.That(radioButton5.IsEnabled, Is.True);
            Assert.That(radioButton6.IsEnabled, Is.False);

            // Close the window
            window.Close();
        }
    }
}
