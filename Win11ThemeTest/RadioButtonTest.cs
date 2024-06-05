using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using System.Configuration;


namespace Win11ThemeTest
{
    [TestFixture]
    public class radioButtonTests
    {
        private readonly Application? app;
        private readonly Window? window;
        public Button? testButton;
        public Window? radioBtnWindow;
        readonly RadioButton? optionA;
        readonly RadioButton? optionB;
        readonly RadioButton? optionC;
        readonly RadioButton? optionD;

        readonly RadioButton? radioButton1;
        readonly RadioButton? radioButton2;
        readonly RadioButton? radioButton3;
        readonly RadioButton? radioButton4;
        readonly RadioButton? radioButton5;
        readonly RadioButton? radioButton6;
        public radioButtonTests()
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
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("radioButton")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                radioBtnWindow = window.FindFirstDescendant(cf => cf.ByName("RadioButtonWindow")).AsWindow();
                optionA = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("Option A")).AsRadioButton();
                optionB = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("Option B")).AsRadioButton();
                optionC = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("Option C")).AsRadioButton();
                optionD = radioBtnWindow.FindFirstDescendant(cf => cf.ByName("Option D")).AsRadioButton();

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

        //Check if the radio buttons are initially unselected.
        [Test]
        public void RadioButton1_isUncheck()
        {
            Assert.That(optionA, Is.Not.Null);
            Assert.That(optionA.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(optionB, Is.Not.Null);
            Assert.That(optionB.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(optionC, Is.Not.Null);
            Assert.That(optionC.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(optionD, Is.Not.Null);
            Assert.That(optionD.IsChecked, Is.False);
        }

        //Validate that clicking on the label associated with a radio button selects the button.
        [Test]
        public void RadioButton2_labelCheck()
        {
            Assert.That(optionA, Is.Not.Null);
            Mouse.Click(optionA.GetClickablePoint());
            Assert.That(optionA.IsChecked, Is.True);
        }

        // Re-checking on a checked radio button does not affect it's checked state
        [Test]
        public void RadioButton3_state()
        {
            Assert.That(optionA, Is.Not.Null);
            Assert.That(optionA.IsChecked, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(optionB, Is.Not.Null);
            Assert.That(optionB.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(optionC, Is.Not.Null);
            Assert.That(optionC.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(optionD, Is.Not.Null);
            Assert.That(optionD.IsChecked, Is.False);

            optionD.Click();

            Assert.Multiple(() =>
            {
                Assert.That(optionA.IsChecked, Is.False);
                Assert.That(optionB.IsChecked, Is.False);
                Assert.That(optionC.IsChecked, Is.False);
                Assert.That(optionD.IsChecked, Is.True);
            });
        }

        //Verify that only one radio button can be selected at a time.
        //Confirm that selecting one radio button deselects any previously selected option.
        [Test]
        public void RadioButton4_selection()
        {
            Assert.That(optionA, Is.Not.Null);
            Mouse.Click(optionA.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            Assert.That(optionA.IsChecked, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(optionB, Is.Not.Null);
            Assert.That(optionB.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(optionC, Is.Not.Null);
            Assert.That(optionC.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(optionD, Is.Not.Null);
            Assert.That(optionD.IsChecked, Is.False);

            //select option B and make sure option A gets deselected
            Mouse.Click(optionB.GetClickablePoint());

            Assert.Multiple(() =>
            {
                Assert.That(optionA.IsChecked, Is.False);
                Assert.That(optionB.IsChecked, Is.True);
                Assert.That(optionC.IsChecked, Is.False);
                Assert.That(optionD.IsChecked, Is.False);
            });
        }

        // Checking if a radio button in both the groups are clickable
        [Test]
        public void RadioButton5_groupCheck()
        {
            Assert.That(radioButton1, Is.Not.Null);
            Assert.That(radioButton1.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton2, Is.Not.Null);
            Assert.That(radioButton2.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton3, Is.Not.Null);
            Assert.That(radioButton3.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton4, Is.Not.Null);
            Assert.That(radioButton4.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton5, Is.Not.Null);
            Assert.That(radioButton5.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton6, Is.Not.Null);
            Assert.That(radioButton6.IsChecked, Is.False);
       

            radioButton1.Click();
            radioButton5.Click();

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

        // Checking or Selecting a disabled radio button
        [Test]
        public void RadioButtonTest6_KeyboardSelect()
        {
            Assert.That(radioButton3, Is.Not.Null);
            Assert.That(radioButton3.IsChecked, Is.False);
            Mouse.Click(radioButton3.GetClickablePoint());
            Assert.That(radioButton3.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton1, Is.Not.Null);
            radioButton1.Focus();

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);
            // Get the current mouse position
            //var mousePosition = Mouse.Position;

            // Output the mouse position
            // Console.WriteLine($"Mouse Position: X = {mousePosition.X}, Y = {mousePosition.Y}");

            // Get the element at the mouse position
            using var automation2 = new UIA3Automation();

            //var automation = new FlaUI.UIA3.Core.AutomationElement.Automation();
            //var element = automation2.FromPoint(mousePosition);
            var element = automation2.FocusedElement();
            // Check if an element is found at the mouse position
            if (element != null)
            {
                // Get the element's name (or any other property) to identify the focused element
                var elementName = element.Name;

                // Output the focused element's name
                Console.WriteLine($"Focused Element: {elementName}");
                Assert.That(radioButton4, Is.Not.Null);
                Assert.That(radioButton4.Name, Is.EqualTo(elementName));

            }
        }

        // Test to confirm click in one group does not affect checked radio button in another group  
        [Test]
        public void RadioButtonTest7_ClickGroupButton()
        {
            //Group 1 radio button
            Assert.That(radioButton2, Is.Not.Null);
            radioButton2.Click();
            Assert.That(radioButton2.IsChecked, Is.True);

            //Group 2 radio button
            Assert.That(radioButton5, Is.Not.Null);
            radioButton5.Click();
            Assert.That(radioButton5.IsChecked, Is.True);

            Assert.That(radioButton1, Is.Not.Null);
            Assert.That(radioButton1.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();           
            Assert.That(radioButton2.IsChecked, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton3, Is.Not.Null);
            Assert.That(radioButton3.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton4, Is.Not.Null);
            Assert.That(radioButton4.IsChecked, Is.False);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton5.IsChecked, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton6, Is.Not.Null);
            Assert.That(radioButton6.IsChecked, Is.False);
           
        }

        // Test to confirm radio buttons can be checked using space bar
        [Test]
        public void RadioButtonTest8_SelectWithSapceKey()
        {
            Assert.That(radioButton2, Is.Not.Null);
            radioButton2.Click();
            Assert.That(radioButton1, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(radioButton1.IsChecked, Is.False);
                Assert.That(radioButton2.IsChecked, Is.True);
            });
            radioButton2.Focus();

            Keyboard.TypeSimultaneously(FlaUI.Core.WindowsAPI.VirtualKeyShort.SHIFT, FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Wait.UntilInputIsProcessed();
            Assert.Multiple(() =>
            {
                Assert.That(radioButton1.IsChecked, Is.True);
                Assert.That(radioButton2.IsChecked, Is.False);
            });
        }

        // Test to confirm correct initialization from code behind
        [Test]
        public void RadioButtonTest9_testInitialization()
        {
            Assert.That(radioButton4, Is.Not.Null);
            Assert.That(radioButton4.IsEnabled, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(radioButton5, Is.Not.Null);
            Assert.That(radioButton5.IsEnabled, Is.True);
        }

        [Test]
        public void RadioButtonTest91_cleanUp()
        {
            Assert.That(radioBtnWindow, Is.Not.Null);
            radioBtnWindow.Close();
            Wait.UntilInputIsProcessed();
            Assert.That(radioBtnWindow.IsOffscreen, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(window, Is.Not.Null);
            window.Close();
            Assert.That(window.IsOffscreen, Is.True);
        }
    }
}
