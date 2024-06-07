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
        private readonly Application? app;
        private readonly Window? window;
        public Window? checkboxWindow;
        readonly Button? testButton;
        readonly CheckBox? checkBox;
        readonly CheckBox? threeStateCheckBox;
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
                using var automation = new UIA3Automation();
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testchkbtn")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                checkboxWindow = window.FindFirstDescendant(cf => cf.ByName("CheckboxWindow")).AsWindow();
                checkBox = checkboxWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCheckbox")).AsCheckBox();
                threeStateCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByAutomationId("threestateCheckbox")).AsCheckBox();
                selectCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
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
        #region simplecheckbox
        //test if checkbox is available in window
        [Test]
        public void Checkbox1_isCheckboxAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(checkboxWindow, Is.Not.Null);
                Assert.That(checkBox, Is.Not.Null);
            });
        }

        //test if checkbox is not checked by default
        [Test]
        public void Checkbox2_isNotChecked()
        {
            Assert.That(checkBox, Is.Not.Null);
            Assert.That(checkBox.IsChecked, Is.False);
        }

        //test if checkbox is  checked on toggle
        [Test]
        public void Checkbox3_isChecked()
        {
            Assert.That(checkBox, Is.Not.Null);
            Assert.That(checkBox.IsChecked, Is.False);
            checkBox.Toggle();
            Assert.That(checkBox.IsChecked, Is.True);
            checkBox.Toggle();
        }

        //test if checkbox is  checked with space key
        [Test]
        public void Checkbox4_isCheckedWithSpaceKey()
        {
            Assert.That(checkBox, Is.Not.Null);
            checkBox.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Assert.That(checkBox.IsChecked, Is.True);
            checkBox.Toggle();
        }

        //test if checkbox is  checked with mouseclick
        [Test]
        public void Checkbox5_isCheckedOnMouseClick()
        {
            Assert.That(checkBox, Is.Not.Null);
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
        public void CheckboxThreeState1_is3StateCheckboxAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(checkboxWindow, Is.Not.Null);
                Assert.That(threeStateCheckBox, Is.Not.Null);
            });
        }

        //test if the state is ON with single togle
        [Test]
        public void CheckboxThreeState2_is3StateCheckboxToggleOn()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.On));
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test for intermediate toggle state
        [Test]
        public void CheckboxThreeState3_is3StateCheckboxToggleIntermediate()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.Indeterminate));
            threeStateCheckBox.Toggle();
        }

        //test for OFF state
        [Test]
        public void CheckboxThreeState4_is3StateCheckboxToggleOff()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.Off));
        }

        //test for toggle on mouse click
        [Test]
        public void CheckboxThreeState5_is3StateCheckboxToggleMouseClick()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Click();
            Assert.That(threeStateCheckBox.IsChecked, Is.True);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test for toggle on Space key
        [Test]
        public void CheckboxThreeState6_is3StateCheckboxToggleSpaceKey()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Assert.That(threeStateCheckBox.IsChecked, Is.True);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test select all checkbox in 3 state scenario
        [Test]
        public void CheckboxThreeState7_is3stateCheckboxSelectAll()
        {
            Assert.That(checkboxWindow, Is.Not.Null);
            selectCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
            Mouse.MoveTo(selectCheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            option1 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option2 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.Multiple(() =>
            {
                Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.On));
                Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.On));
                Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.On));
            });
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
        }

        //test deselect all checkbox in 3 state scenario
        [Test]
        public void CheckboxThreeState8_is3stateCheckboxDeselectAll()
        {
            Assert.That(selectCheckBox, Is.Not.Null);
            selectCheckBox.Focus();
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectCheckBox.GetClickablePoint());
            Assert.That(checkboxWindow, Is.Not.Null);
            option1 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option2 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.Multiple(() =>
            {
                Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.Off));
                Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.Off));
                Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.Off));
            });
        }

        //test intermediate state checkbox in 3 state scenario
        [Test]
        public void CheckboxThreeState9_is3stateCheckboxSelectOneOption()
        {
            Assert.That(checkboxWindow, Is.Not.Null);
            option1 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option1.Focus();
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(option1.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            selectCheckBox = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
            option2 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = checkboxWindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.Multiple(() =>
            {
                Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.On));
                Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.Off));
                Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.Off));
                Assert.That(selectCheckBox.ToggleState, Is.EqualTo(ToggleState.Indeterminate));
            });
            Mouse.LeftClick(option1.GetClickablePoint());
        }

        [Test]
        public void CloseWindows()
        {
            Assert.That(checkboxWindow, Is.Not.Null);
            checkboxWindow.Close();
            Wait.UntilInputIsProcessed();
            Assert.That(checkboxWindow.IsOffscreen, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(window, Is.Not.Null);
            window.Close();
            Assert.That(window.IsOffscreen, Is.True);
        }
        #endregion
    }
}
