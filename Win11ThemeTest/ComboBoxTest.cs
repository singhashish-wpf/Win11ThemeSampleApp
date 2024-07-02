using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using System.Configuration;

namespace Win11ThemeTest
{
    public class ComboBoxTests
    {
        private readonly Application? app;
        private readonly Window? mainWindow;
        private readonly Window? comboWindow;
        private readonly Button? comboBoxButton;
        private readonly ComboBox? comboBox;
        private readonly ComboBox? comboBoxEditable;
        private readonly ComboBox? comboBoxBind;

        public ComboBoxTests()
        {
            var appPath = ConfigurationManager.AppSettings["Testpath"];
            app = LaunchApplication(appPath);
            using var automation = new UIA3Automation();
            mainWindow = app?.GetMainWindow(automation);

            comboBoxButton = mainWindow?.FindFirstDescendant(cf => cf.ByAutomationId("cmbBoxButton")).AsButton();
            ClickButton(comboBoxButton);

            comboWindow = mainWindow?.FindFirstDescendant(cf => cf.ByName("ComboBoxWindow")).AsWindow();
            comboBox = comboWindow?.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxList")).AsComboBox();
            comboBoxEditable = comboWindow?.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxEditable")).AsComboBox();
            comboBoxBind = comboWindow?.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxBind")).AsComboBox();
        }

        private Application? LaunchApplication(string? appPath)
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

        private void ClickButton(Button? button)
        {
            if (button == null) throw new ArgumentNullException(nameof(button));

            Mouse.Click(button.GetClickablePoint());
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
        }

        private void LogException(Exception ex)
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

        [Test]
        public void ComboBox_IsAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(comboWindow, Is.Not.Null);
                Assert.That(comboBox, Is.Not.Null);
                Assert.That(comboBoxEditable, Is.Not.Null);
                Assert.That(comboBoxBind, Is.Not.Null);
            });
        }

        [Test]
        public void ComboBox_SelectedDefaultItem()
        {
            Assert.That(comboBox, Is.Not.Null);
            Assert.That(comboBox?.SelectedItem, Is.Not.Null);
        }

        [Test]
        public void ComboBox_IsNotNull()
        {
            Assert.That(comboBox, Is.Not.Null);
        }

        [Test]
        public void ComboBox_IsReadableEditable()
        {
            Assert.That(comboBox, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(comboBox?.IsReadOnly, Is.False);
                Assert.That(comboBox?.IsEditable, Is.False);
            });
            Assert.That(comboBoxEditable, Is.Not.Null);
            Assert.That(comboBoxEditable?.IsEditable, Is.True);
        }

        [Test]
        public void ComboBox1_SelectItem()
        {
            Assert.That(comboBox, Is.Not.Null);
            comboBox?.Select("Red");
            Assert.That(comboBox?.SelectedItem.Name, Is.EqualTo("Red"));
        }

        [Test]
        public void ComboBox1_ExpandCollapse()
        {
            Assert.That(comboBox, Is.Not.Null);
            comboBox?.Expand();
            Assert.That(comboBox?.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Expanded));
            comboBox?.Collapse();
            Assert.That(comboBox?.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Collapsed));
        }

        [Test]
        public void ComboBox2_EditableText()
        {
            Assert.That(comboBoxEditable, Is.Not.Null);
            comboBoxEditable.EditableText = "10";
            Assert.That(comboBoxEditable?.SelectedItem, Is.Not.Null);
            Assert.That(comboBoxEditable?.SelectedItem.Text, Is.EqualTo("10"));
        }

        [Test]
        public void ComboBox3_MouseClick()
        {
            Assert.That(comboBox, Is.Not.Null);
            comboBox?.Click();
            Assert.That(comboBox?.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Expanded));
            comboBox?.Collapse();
        }

        [Test]
        public void ComboBox4_MouseSelectClick()
        {
            Assert.That(comboBox, Is.Not.Null);
            Mouse.MoveTo(comboBox.GetClickablePoint());
            Mouse.Click();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            comboBox?.Items[3].Click();
            Assert.That(comboBox?.SelectedItem.Name, Is.EqualTo("Yellow"));
        }

        [Test]
        public void ComboBox5_CleanUp()
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
