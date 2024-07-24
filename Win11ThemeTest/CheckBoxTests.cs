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

                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = LaunchApplication(appPath);
                using var automation = new UIA3Automation();
                window = app?.GetMainWindow(automation);
                testButton = window?.FindFirstDescendant(cf => cf.ByAutomationId("testchkbtn")).AsButton();
                ClickButton(testButton);
                checkboxWindow = window?.FindFirstDescendant(cf => cf.ByName("CheckboxWindow")).AsWindow();
                checkBox = checkboxWindow?.FindFirstDescendant(cf => cf.ByAutomationId("tstCheckbox")).AsCheckBox();
                threeStateCheckBox = checkboxWindow?.FindFirstDescendant(cf => cf.ByAutomationId("threestateCheckbox")).AsCheckBox();
                selectCheckBox = checkboxWindow?.FindFirstDescendant(cf => cf.ByName("Select all")).AsCheckBox();        
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

        #region simplecheckbox
        //test if checkbox is available in window
        [Test]
        public void Checkbox1_IsCheckboxAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(checkboxWindow, Is.Not.Null);
                Assert.That(checkBox, Is.Not.Null);
            });
        }

        //test if checkbox is not checked by default
        [Test]
        public void Checkbox2_IsNotChecked()
        {
            Assert.That(checkBox, Is.Not.Null);
            Assert.That(checkBox.IsChecked, Is.False);
        }

        //test if checkbox is  checked on toggle
        [Test]
        public void Checkbox3_IsChecked()
        {
            Assert.That(checkBox, Is.Not.Null);
            Assert.That(checkBox.IsChecked, Is.False);
            checkBox.Toggle();
            Assert.That(checkBox.IsChecked, Is.True);
            checkBox.Toggle();
        }

        //test if checkbox is  checked with space key
        [Test]
        public void Checkbox4_IsCheckedWithSpaceKey()
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
        public void Checkbox5_IsCheckedOnMouseClick()
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
        public void CheckboxThreeState1_Is3StateCheckboxAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(checkboxWindow, Is.Not.Null);
                Assert.That(threeStateCheckBox, Is.Not.Null);
            });
        }

        //test if the state is ON with single togle
        [Test]
        public void CheckboxThreeState2_Is3StateCheckboxToggleOn()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.On));
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test for intermediate toggle state
        [Test]
        public void CheckboxThreeState3_Is3StateCheckboxToggleIntermediate()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.Indeterminate));
            threeStateCheckBox.Toggle();
        }

        //test for OFF state
        [Test]
        public void CheckboxThreeState4_Is3StateCheckboxToggleOff()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
            Assert.That(threeStateCheckBox.ToggleState, Is.EqualTo(ToggleState.Off));
        }

        //test for toggle on mouse click
        [Test]
        public void CheckboxThreeState5_Is3StateCheckboxToggleMouseClick()
        {
            Assert.That(threeStateCheckBox, Is.Not.Null);
            threeStateCheckBox.Click();
            Assert.That(threeStateCheckBox.IsChecked, Is.True);
            threeStateCheckBox.Toggle();
            threeStateCheckBox.Toggle();
        }

        //test for toggle on Space key
        [Test]
        public void CheckboxThreeState6_Is3StateCheckboxToggleSpaceKey()
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
        public void CheckboxThreeState7_Is3stateCheckboxSelectAll()
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
        public void CheckboxThreeState8_Is3stateCheckboxDeselectAll()
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
        public void CheckboxThreeState9_Is3stateCheckboxSelectOneOption()
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
        #endregion
    }
}
