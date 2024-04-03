using System.Configuration;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;


namespace Win11ThemeTest
{
    public class ListBoxTest
    {
        private Application app;
        private Window window;
        public Window listBoxWindow;
        Button testButton;
        ListBox listBox;
        ListBox listBoxLength;
        public ListBoxTest()
        {
            try
            {
                //app = Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using (var automation = new UIA3Automation())
                {
                    window = app.GetMainWindow(automation);
                    testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("listBoxtestbtn")).AsButton();
                    Mouse.Click(testButton.GetClickablePoint());
                    Wait.UntilInputIsProcessed();
                    listBoxWindow = window.FindFirstDescendant(cf => cf.ByName("ListboxWindow")).AsWindow();
                    listBox = listBoxWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstLstbox")).AsListBox();
                    listBoxLength = listBoxWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstlengthLstbox")).AsListBox();
                }
            }
            catch (Exception ex)
            {
                var filePath = ConfigurationManager.AppSettings["logpath"];
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
        }

        //test if listbox is available in window
        [Test]
        public void listBox1_isListBoxAvailable()
        {
            Assert.IsNotNull(listBoxWindow);
            Assert.IsNotNull(listBox);
        }

        //test if listbox is empty or populated with default values
        [Test]
        public void listBox2_isListBoxEmpty()
        {
            var selectListItems = listBox.Items;
            Assert.That(selectListItems.Length, Is.GreaterThan(0));
        }

        //test if listbox return default selected item
        [Test]
        public void listBox3_defaultSelectedItem()
        {
            var selectList = listBox.SelectedItem;
            var selectIndex = listBox.Items.ElementAt(0);
            Assert.That(selectList, Is.EqualTo(selectIndex));
        }

        //test selecting listbox item on mouse click
        [Test]

        public void listBox4_selectItemMouseClick()
        {
            var selectList = listBox.SelectedItem;
            listBox.Items.ElementAt(3).Click();
            Wait.UntilInputIsProcessed();
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }

        //test if listbox return selected item
        [Test]
        public void listBox5_isSelectedItemByIndex()
        {
            var selectList = listBox.Select(2);
            var textOfIndex = listBox.Items.ElementAt(2);
            Assert.That(selectList.Text, Is.EqualTo(textOfIndex.Text));
        }

        //test list item by text
        [Test]
        public void listBox6_isSelectedByItemText()
        {
            var selectList = listBox.Select("Red");
            var selectedItem = listBox.SelectedItem;
            Assert.That(selectList, Is.EqualTo(selectedItem));
        }

        //test scrollbar for fixed length of listbox
        [Test]
        public void listBox7_verticalScrollBarforFixedlength()
        {
            Assert.That(listBox.Patterns.Scroll.Pattern.VerticallyScrollable.Value, Is.False);
            Assert.That(listBoxLength.Patterns.Scroll.Pattern.VerticallyScrollable.Value, Is.True);
        }

        //test keyboard navigate with down arrow
        [Test]
        public void listBox8_keyBoarNavigateDown()
        {
            var selectList = listBox.SelectedItem;
            listBox.SelectedItem.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.DOWN);
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }

        //test keyboard navigate with up arrow
        [Test]
        public void listBox9_keyBoarNavigateUp()
        {
            listBox.Select(2);
            var selectList = listBox.SelectedItem;
            listBox.SelectedItem.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.UP);
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }

        //Test vertical scrolling for listbox with fixed length
        [Test]
        public void listBoxs1_verticalScroll()
        {
            double defaultScroll = 0;
            Assert.That(listBoxLength.Patterns.Scroll.Pattern.VerticalScrollPercent, Is.EqualTo(defaultScroll));
            listBoxLength.Patterns.Scroll.Pattern.Scroll(ScrollAmount.NoAmount, ScrollAmount.SmallIncrement);
            Assert.That(listBoxLength.Patterns.Scroll.Pattern.VerticalScrollPercent, Is.Not.EqualTo(defaultScroll));
        }

        [Test]
        public void listBoxs2_closeWindows()
        {
            listBoxWindow.Close();
            Assert.IsTrue(listBoxWindow.IsOffscreen);
            window.Close();
            Assert.IsTrue(window.IsOffscreen);
        }

    }
}
