using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using System.Configuration;

namespace Win11ThemeTest
{
    public class comboBoxTests
    {
        private readonly Application? app;
        private readonly Window? mainWindow;
        public Window? comboWindow;
        readonly Button? comboBoxButton;
        readonly ComboBox? comboBox;
        readonly ComboBox? comboBoxEditable;
        readonly ComboBox? comboBoxBind;
        public comboBoxTests()
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
                mainWindow = app.GetMainWindow(automation);
                comboBoxButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("cmbBoxButton")).AsButton();
                Mouse.Click(comboBoxButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
                comboWindow = mainWindow.FindFirstDescendant(cf => cf.ByName("ComboBoxWindow")).AsWindow();
                comboBox = comboWindow.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxList")).AsComboBox();
                comboBoxEditable = comboWindow.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxEditable")).AsComboBox();
                comboBoxBind = comboWindow.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxBind")).AsComboBox();
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
        public void ComboBox_isAvailable()
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
        public void ComboBox_selectedDefaultItem()
        {
            Assert.That(comboBox, Is.Not.Null);
            Assert.That(comboBox.SelectedItem, Is.Not.Null);
        }

        [Test]
        public void ComboBox_isNull()
        {
            Assert.That(comboBox, Is.Not.Null);
        }

        [Test]
        public void ComboBox_isReadableEditable()
        {
            Assert.That(comboBox, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(comboBox.IsReadOnly, Is.False);
                Assert.That(comboBox.IsEditable, Is.False);
                
            });
            Assert.That(comboBoxEditable, Is.Not.Null);
            Assert.That(comboBoxEditable.IsEditable, Is.True);
        }

        [Test]
        public void ComboBox1_selectItem()
        {
            Assert.That(comboBox, Is.Not.Null);
            comboBox.Select("Red");
            Assert.That(comboBox.SelectedItem.Name, Is.EqualTo("Red"));
        }

        //Check if the drop-down is open by clicking on both the drop-down & the drop-down Arrow.
        [Test]
        public void ComboBox1_expandCollapse()
        {
            Assert.That(comboBox, Is.Not.Null);
            comboBox.Expand();
            Assert.That(comboBox.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Expanded));
            comboBox.Collapse();
            Assert.That(comboBox.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Collapsed));
        }

        [Test]
        public void ComboBox2_editableText()
        {
            Assert.That(comboBoxEditable, Is.Not.Null);
            Assert.That(comboBoxEditable.IsEditable, Is.True);
            comboBoxEditable.EditableText = "10";
            Assert.That(comboBoxEditable.SelectedItem, Is.Not.Null);
            Assert.That(comboBoxEditable.SelectedItem.Text, Is.EqualTo("10"));
        }

        //Check whether the dropdown is clickable or not.
        [Test]
        public void ComboBox3_mouseClick()
        {
            Assert.That(comboBox, Is.Not.Null);
            comboBox.Click();
            Assert.That(comboBox.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Expanded));
            comboBox.Collapse();
        }

        [Test]
        public void ComboBox4_mouseSelectClick()
        {
            Assert.That(comboBox, Is.Not.Null);
            Mouse.MoveTo(comboBox.GetClickablePoint());
            Mouse.Click();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            comboBox.Items[3].Click();
            Assert.That(comboBox.SelectedItem.Name, Is.EqualTo("Yellow"));
        }

        [Test]
        public void ComboBox5_cleanUp()
        {
            Assert.That(comboWindow, Is.Not.Null);
            comboWindow.Close();
            Wait.UntilInputIsProcessed();
            Assert.That(comboWindow.IsOffscreen, Is.True);
            Wait.UntilInputIsProcessed();
            Assert.That(mainWindow, Is.Not.Null);
            mainWindow.Close();
            Assert.That(mainWindow.IsOffscreen, Is.True);
        }
    }
}
