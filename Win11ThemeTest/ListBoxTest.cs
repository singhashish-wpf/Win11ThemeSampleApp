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
        private readonly Application? app;
        private readonly Window? window;
        public Window? listBoxWindow;
        readonly Button? testButton;
        readonly ListBox? listBox;
        readonly ListBox? listBoxLength;
        public ListBoxTest()
        {
           
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = LaunchApplication(appPath);
                using var automation = new UIA3Automation();
                window = app?.GetMainWindow(automation);
                testButton = window?.FindFirstDescendant(cf => cf.ByAutomationId("listBoxtestbtn")).AsButton();
                ClickButton(testButton);
                listBoxWindow = window?.FindFirstDescendant(cf => cf.ByName("ListboxWindow")).AsWindow();
                listBox = listBoxWindow?.FindFirstDescendant(cf => cf.ByAutomationId("tstLstbox")).AsListBox();
                listBoxLength = listBoxWindow?.FindFirstDescendant(cf => cf.ByAutomationId("tstlengthLstbox")).AsListBox();            
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
        //test if listBox is available in window
        [Test]
        public void ListBox1_IsListBoxAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(listBoxWindow, Is.Not.Null);
                Assert.That(listBox, Is.Not.Null);
            });
        }

        //test if listBox is empty or populated with default values
        [Test]
        public void ListBox2_IsListBoxEmpty()
        {
            Assert.That(listBox, Is.Not.Null);
            var selectListItems = listBox.Items;
            Assert.That(selectListItems.Length, Is.GreaterThan(0));
        }

        //test if listBox return default selected item
        [Test]
        public void ListBox3_DefaultSelectedItem()
        {
            Assert.That(listBox, Is.Not.Null);
            var selectList = listBox.SelectedItem;
            var selectIndex = listBox.Items.ElementAt(0);
            Assert.That(selectList, Is.EqualTo(selectIndex));
        }

        //test selecting listBox item on mouse click
        [Test]
        public void ListBox4_SelectItemMouseClick()
        {
            Assert.That(listBox, Is.Not.Null);
            var selectList = listBox.SelectedItem;
            listBox.Items.ElementAt(3).Click();
            Wait.UntilInputIsProcessed();
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }

        //test if listBox return selected item
        [Test]
        public void ListBox5_IsSelectedItemByIndex()
        {
            Assert.That(listBox, Is.Not.Null);
            var selectList = listBox.Select(2);
            var textOfIndex = listBox.Items.ElementAt(2);
            Assert.That(selectList.Text, Is.EqualTo(textOfIndex.Text));
        }

        //test list item by text
        [Test]
        public void ListBox6_IsSelectedByItemText()
        {
            Assert.That(listBox, Is.Not.Null);
            var selectList = listBox.Select("Red");
            var selectedItem = listBox.SelectedItem;
            Assert.That(selectList, Is.EqualTo(selectedItem));
        }

        //test scrollbar for fixed length of listBox
        [Test]
        public void ListBox7_VerticalScrollBarForFixedLength()
        {
            Assert.That(listBox, Is.Not.Null);           
            Assert.That(listBox.Patterns.Scroll.Pattern.VerticallyScrollable.Value, Is.False);
            Assert.That(listBoxLength, Is.Not.Null);
            Assert.That(listBoxLength.Patterns.Scroll.Pattern.VerticallyScrollable.Value, Is.True);
        }

        //test keyboard navigate with down arrow
        [Test]
        public void ListBox8_KeyBoardNavigateDown()
        {
            Assert.That(listBox, Is.Not.Null);
            var selectList = listBox.SelectedItem;
            listBox.SelectedItem.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.DOWN);
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }

        //test keyboard navigate with up arrow
        [Test]
        public void ListBox9_KeyBoardNavigateUp()
        {
            Assert.That(listBox, Is.Not.Null);
            listBox.Select(2);
            var selectList = listBox.SelectedItem;
            listBox.SelectedItem.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.UP);
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }

        //Test vertical scrolling for listBox with fixed length
        [Test]
        public void ListBoxs1_VerticalScroll()
        {
            Assert.That(listBoxLength, Is.Not.Null);
            double defaultScroll = 0;
            Assert.That(listBoxLength.Patterns.Scroll.Pattern.VerticalScrollPercent, Is.EqualTo(defaultScroll));
            listBoxLength.Patterns.Scroll.Pattern.Scroll(ScrollAmount.NoAmount, ScrollAmount.SmallIncrement);
            Assert.That(listBoxLength.Patterns.Scroll.Pattern.VerticalScrollPercent, Is.Not.EqualTo(defaultScroll));
        }

        [Test]
        public void ListBoxs2_CloseWindows()
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
