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
        private Application? app;
        private Window? mainWindow;        
        public Window? comboWindow;
        Button? comboBoxButton;
        ComboBox? comboBox;
        ComboBox? comboBoxEditable;
        ComboBox? comboBoxBind;        
        public comboBoxTests()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    mainWindow = app.GetMainWindow(automation);
                    comboBoxButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("cmbBoxButton")).AsButton();
                    Mouse.Click(comboBoxButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
                    comboWindow = mainWindow.FindFirstDescendant(cf => cf.ByName("ComboBoxWindow")).AsWindow();
                    comboBox = comboWindow.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxList")).AsComboBox();
                    comboBoxEditable = comboWindow.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxEditable")).AsComboBox();
                    comboBoxBind = comboWindow.FindFirstDescendant(cf => cf.ByAutomationId("comboBoxBind")).AsComboBox();
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

        [Test]
        public void comboBox_isAvailable()
        {
            Assert.IsNotNull(comboWindow);            
            Assert.IsNotNull(comboBox);
            Assert.IsNotNull(comboBoxEditable);
            Assert.IsNotNull(comboBoxBind);
        }

        [Test]
        public void comboBox_selectedDefaultItem()
        {
            Assert.IsNotNull(comboBox);
            Assert.That(comboBox.SelectedItem, Is.Not.Null);           
        }

        [Test]
        public void comboBox_isNull()
        {
            Assert.That(comboBox, Is.Not.Null);           
        }

        [Test]
        public void comboBox_isReadableEditable()
        {
            Assert.IsNotNull(comboBox);
            Assert.That(comboBox.IsReadOnly, Is.False);
            Assert.That(comboBox.IsEditable, Is.False);
            Assert.IsNotNull(comboBoxEditable);
            Assert.That(comboBoxEditable.IsEditable, Is.True);
        }
       
        [Test]
        public void comboBox1_selectItem()
        {
            Assert.IsNotNull(comboBox);
            comboBox.Select("Red");
            Assert.That(comboBox.SelectedItem.Name, Is.EqualTo("Red"));            
        }

        //Check if the drop-down is open by clicking on both the drop-down & the drop-down Arrow.
        [Test]       
        public void comboBox1_expandCollapse()
        {
            Assert.IsNotNull(comboBox);
            comboBox.Expand();
            Assert.That(comboBox.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Expanded));
            comboBox.Collapse();
            Assert.That(comboBox.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Collapsed));            
        }      

        [Test]
        public void comboBox2_editableText()
        {          
            Assert.That(comboBoxEditable, Is.Not.Null);
            Assert.That(comboBoxEditable.IsEditable, Is.True);
            comboBoxEditable.EditableText = "10";
            Assert.That(comboBoxEditable.SelectedItem, Is.Not.Null);
            Assert.That(comboBoxEditable.SelectedItem.Text, Is.EqualTo("10"));
        }

        //Check whether the dropdown is clickable or not.
        [Test]
        public void comboBox3_mouseClick()
        {
            Assert.IsNotNull(comboBox);
            Mouse.MoveTo(comboBox.GetClickablePoint());
            Mouse.Click();
            Assert.That(comboBox.ExpandCollapseState, Is.EqualTo(ExpandCollapseState.Expanded));
            comboBox.Collapse();
        }

        [Test]
        public void comboBox4_mouseSelectClick()
        {
            Assert.IsNotNull(comboBox);
            Mouse.MoveTo(comboBox.GetClickablePoint());
            Mouse.Click();            
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));           
            comboBox.Items[3].Click();
            Assert.That(comboBox.SelectedItem.Name, Is.EqualTo("Yellow"));                    
        }

        [Test]
        public void comboBox5_cleanUp()
        {
            Assert.IsNotNull(comboWindow);
            comboWindow.Close();
            Assert.IsTrue(comboWindow.IsOffscreen);
            Assert.IsNotNull(mainWindow);
            mainWindow.Close();
            Assert.IsTrue(mainWindow.IsOffscreen);
        }
    }
}
