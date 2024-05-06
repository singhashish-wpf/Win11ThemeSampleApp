using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.Core.Definitions;
using System.Configuration;

namespace Win11ThemeTest
{
    public class CheckBoxTests
    {
        private Application? app;
        private Window? window;
        public Window? checkboxWindow;
        Button? testButton;
        CheckBox? checkBox;
        CheckBox? threeStateCheckBox;
        CheckBox? selectCheckBox;
        CheckBox? option1;
        CheckBox? option2;
        CheckBox? option3;

        public CheckBoxTests()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testchkbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                    checkboxWindow = window.FindFirstDescendant(cf => cf.ByName("CheckboxWindow")).AsWindow();
                    checkBox = checkboxWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCheckbox")).AsCheckBox();
                    threeStateCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByAutomationId("threestateCheckbox")).AsCheckBox();
                    selectCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
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
        #region simplecheckbox
        //test if checkbox is available in window
        [Test]
        public void checkbox1_isCheckboxAvailable()
        {
            Assert.IsNotNull(checkboxWindow);
            Assert.IsNotNull(checkBox);
        }

        //test if checkbox is not checked by default
        [Test]
        public void checkbox2_isNotChecked()
        {
            Assert.IsNotNull(checkBox);
            Assert.That(checkBox.IsChecked, Is.False);
        }

        //test if checkbox is  checked on toggle
        [Test]
        public void checkbox3_isChecked()
        {
            Assert.IsNotNull(checkBox);
            Assert.That(checkBox.IsChecked, Is.False);
            checkBox.Toggle(); ;
            Assert.That(checkBox.IsChecked, Is.True);
            checkBox.Toggle();
        }

        //test if checkbox is  checked with space key
        [Test]
        public void checkbox4_isCheckedWithSpaceKey()
        {
            Assert.IsNotNull(checkBox);
            checkBox.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Assert.That(checkBox.IsChecked, Is.True);
            checkBox.Toggle();
        }

        //test if checkbox is  checked with mouseclick
        [Test]
        public void checkbox5_isCheckedOnMouseClick()
        {
            Assert.IsNotNull(checkBox);
            Mouse.MoveTo(checkBox.GetClickablePoint());
            Mouse.MoveBy(0, 10);
            Mouse.Down(MouseButton.Left);
            Wait.UntilInputIsProcessed();
            checkBox.Click();
            Assert.That(checkBox.IsChecked, Is.True);
            checkBox.Toggle();
        }
        #endregion

        #region 3StateCheckbox

        //test if three state checkbox is available in window
        [Test]
        public void checkboxThreeState1_is3StateCheckboxAvailable()
        {
            Assert.IsNotNull(checkboxWindow);
            Assert.IsNotNull(threeStateCheckBox);
        }

        //test if the state is ON with single togle
        [Test]
        public void checkboxThreeState2_is3StateCheckboxToggleOn()
        {
            Assert.IsNotNull(threeStateCheckBox);
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.On));
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test for intermediate toggle state
        [Test]
        public void checkboxThreeState3_is3StateCheckboxToggleIntermediate()
        {
            Assert.IsNotNull(threeStateCheckBox);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.Indeterminate));
            threeStateCheckBox.Toggle();
        }

        //test for OFF state
        [Test]
        public void checkboxThreeState4_is3StateCheckboxToggleOff()
        {
            Assert.IsNotNull(threeStateCheckBox);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.Off));
        }

        //test for toggle on mouse click
        [Test]
        public void checkboxThreeState5_is3StateCheckboxToggleMouseClick()
        {
            Assert.IsNotNull(threeStateCheckBox);
            threeStateCheckBox.Click();
            Assert.That(threeStateCheckBox.IsChecked, Is.True);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test for toggle on Space key
        [Test]
        public void checkboxThreeState6_is3StateCheckboxToggleSpaceKey()
        {
            Assert.IsNotNull(threeStateCheckBox);
            threeStateCheckBox.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Assert.That(threeStateCheckBox.IsChecked, Is.True);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test select all checkbox in 3 state scenario
        [Test]
        public void checkboxThreeState7_is3stateCheckboxSelectAll()
        {
            Assert.IsNotNull(checkboxWindow);
            selectCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
            Mouse.MoveTo(selectCheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            option1 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option2 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.On));
            Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.On));
            Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.On));
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
        }

        //test deselect all checkbox in 3 state scenario
        [Test]
        public void checkboxThreeState8_is3stateCheckboxDeselectAll()
        {
            Assert.IsNotNull(selectCheckBox);
            Assert.IsNotNull(checkboxWindow);
            selectCheckBox.Focus();
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
            option1 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option2 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.Off));
        }

        //test intermediate state checkbox in 3 state scenario
        [Test]
        public void checkboxThreeState9_is3stateCheckboxSelectOneOption()
        {
            Assert.IsNotNull(checkboxWindow);
            option1 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option1.Focus();
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(option1.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            selectCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
            option2 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.On));
            Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(selectCheckBox.ToggleState, Is.EqualTo(ToggleState.Indeterminate));
            Mouse.LeftClick(option1.GetClickablePoint());
        }

        [Test]
        public void closeWindows()
        {
            Assert.IsNotNull(checkboxWindow);
            Assert.IsNotNull(window);
            checkboxWindow.Close();
            Wait.UntilInputIsProcessed();
            Assert.IsTrue(checkboxWindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }
        #endregion
    }
}
