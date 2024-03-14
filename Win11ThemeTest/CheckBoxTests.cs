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
        private Application app;
        private Window window;
        public Window chkbxwindow;
        Button testButton;
        CheckBox checkBox;
        CheckBox threestatecheckBox;
        CheckBox selectcheckBox;
        CheckBox option1;
        CheckBox option2;
        CheckBox option3;

        public CheckBoxTests()
        {
            try
            {
                // app = Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testchkbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                    chkbxwindow = window.FindFirstDescendant(cf => cf.ByName("CheckboxWindow")).AsWindow();
                    checkBox = chkbxwindow.FindFirstDescendant(cf => cf.ByAutomationId("tstCheckbox")).AsCheckBox();
                    threestatecheckBox = chkbxwindow.FindFirstDescendant(cf => cf.ByAutomationId("threestateCheckbox")).AsCheckBox();
                    selectcheckBox = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
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
        #region simplecheckbox
        //test if checkbox is available in window
        [Test]
        public void checkbox1_isCheckboxAvailable()
        {
            Assert.IsNotNull(chkbxwindow);
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
        public void checkboxthreestate1_is3StateCheckboxAvailable()
        {
            Assert.IsNotNull(chkbxwindow);
            Assert.IsNotNull(threestatecheckBox);
        }

        //test if the state is ON with single togle
        [Test]
        public void checkboxthreestate2_is3StateCheckboxToggleOn()
        {
            threestatecheckBox.Toggle();
            Assert.That(threestatecheckBox.ToggleState, Is.EqualTo(ToggleState.On));
            threestatecheckBox.Toggle();
            threestatecheckBox.Toggle();
        }

        //test for intermediate toggle state
        [Test]
        public void checkboxthreestate3_is3StateCheckboxToggleintermediate()
        {
            threestatecheckBox.Toggle();
            threestatecheckBox.Toggle();
            Assert.That(threestatecheckBox.ToggleState, Is.EqualTo(ToggleState.Indeterminate));
            threestatecheckBox.Toggle();
        }

        //test for OFF state
        [Test]
        public void checkboxthreestate4_is3StateCheckboxToggleOff()
        {
            threestatecheckBox.Toggle();
            threestatecheckBox.Toggle();
            threestatecheckBox.Toggle();
            Assert.That(threestatecheckBox.ToggleState, Is.EqualTo(ToggleState.Off));
        }

        //test for toggle on mouse click
        [Test]
        public void checkboxthreestate5_is3StateCheckboxToggleMouseClick()
        {
            Mouse.MoveTo(threestatecheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            threestatecheckBox.Click();
            Wait.UntilInputIsProcessed();
            Assert.That(threestatecheckBox.IsChecked, Is.True);
            threestatecheckBox.Toggle();
            threestatecheckBox.Toggle();
        }

        //test for toggle on Space key
        [Test]
        public void checkboxthreestate6_is3StateCheckboxToggleSpaceKey()
        {
            threestatecheckBox.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.SPACE);
            Assert.That(threestatecheckBox.IsChecked, Is.True);
            threestatecheckBox.Toggle();
            threestatecheckBox.Toggle();
        }

        //test select all checkbox in 3 state scenario
        [Test]
        public void checkboxthreestate7_is3stateCheckboxSelectAll()
        {
            selectcheckBox = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
            Mouse.MoveTo(selectcheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectcheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            option1 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option2 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.On));
            Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.On));
            Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.On));
            Mouse.LeftClick(selectcheckBox.GetClickablePoint());
        }

        //test deselect all checkbox in 3 state scenario
        [Test]
        public void checkboxthreestate8_is3stateCheckboxDeselectAll()
        {
            selectcheckBox.Focus();
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectcheckBox.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(selectcheckBox.GetClickablePoint());
            option1 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option2 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.Off));
        }

        //test intermediate state checkbox in 3 state scenario
        [Test]
        public void checkboxthreestate9_is3stateCheckboxSelectOneOption()
        {
            option1 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 1")).AsCheckBox();
            option1.Focus();
            Wait.UntilInputIsProcessed();
            Mouse.LeftClick(option1.GetClickablePoint());
            Wait.UntilInputIsProcessed();
            selectcheckBox = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();
            option2 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 2")).AsCheckBox();
            option3 = chkbxwindow.FindFirstDescendant(cf => cf.ByName("Option 3")).AsCheckBox();
            Assert.That(option1.ToggleState, Is.EqualTo(ToggleState.On));
            Assert.That(option2.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(option3.ToggleState, Is.EqualTo(ToggleState.Off));
            Assert.That(selectcheckBox.ToggleState, Is.EqualTo(ToggleState.Indeterminate));
            Mouse.LeftClick(option1.GetClickablePoint());
        }

        [Test]
        public void closeWindows()
        {
            chkbxwindow.Close();
            Assert.IsTrue(chkbxwindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }
        #endregion
    }
}
