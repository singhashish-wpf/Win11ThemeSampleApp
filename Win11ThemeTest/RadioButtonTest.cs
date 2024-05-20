using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using System.Text;
using System.Configuration;

namespace Win11ThemeTest
{
    [TestFixture]
    public class rbTests
    {
        private Application app;
        private Window window;
        readonly Button? testButton;
        public Window? radioBtnWindow;
        RadioButton radioButton1;
        RadioButton radioButton2;
        RadioButton radioButton3;
        RadioButton radioButton4;
        RadioButton radioButton5;
        RadioButton radioButton6;

        public rbTests()
        {
          
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using var automation = new UIA3Automation();
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("radioButton")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                radioBtnWindow = window.FindFirstDescendant(cf => cf.ByName("RadiobuttonWindow")).AsWindow();
                radioButton1 = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("RadioButton1")).AsRadioButton();
                radioButton2 = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("RadioButton2")).AsRadioButton();
                radioButton3 = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("RadioButton3")).AsRadioButton();
                radioButton4 = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("RadioButton4")).AsRadioButton();
                radioButton5 = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("RadioButton5")).AsRadioButton();
                radioButton6 = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("RadioButton6")).AsRadioButton();
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

        [Test]
        public void RadioButtonTest_1()
        {
            Assert.Multiple(() =>
            {
                // Checking if a radio button in both the groups are clickable
                Assert.That(radioButton1.IsChecked, Is.False);
                Assert.That(radioButton2.IsChecked, Is.False);
                Assert.That(radioButton3.IsChecked, Is.False);
                Assert.That(radioButton4.IsChecked, Is.False);
                Assert.That(radioButton5.IsChecked, Is.False);
                Assert.That(radioButton6.IsChecked, Is.False);
            });
            Mouse.Click(radioButton1.GetClickablePoint());
            Mouse.Click(radioButton5.GetClickablePoint());
            Assert.Multiple(() =>
            {
                Assert.That(radioButton1.IsChecked, Is.True);
                Assert.That(radioButton2.IsChecked, Is.False);
                Assert.That(radioButton3.IsChecked, Is.False);
                Assert.That(radioButton4.IsChecked, Is.False);
                Assert.That(radioButton5.IsChecked, Is.True);
                Assert.That(radioButton6.IsChecked, Is.False);
            });
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
