using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.UIA3.Identifiers;


namespace Win11ThemeTest
{
    public class ListBoxTest
    {
        private Application app;
        private Window window;
        public Window listBoxWindow;
        Button testButton;
        ListBox listBox;
        ListBox listBoxlength;
        public ListBoxTest()
        {
            app = Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");

            using (var automation = new UIA3Automation())
            {
                window = app.GetMainWindow(automation);
                testButton = window.FindFirstDescendant(cf => cf.ByAutomationId("testbtn")).AsButton();
                Mouse.Click(testButton.GetClickablePoint());
                Wait.UntilInputIsProcessed();
                listBoxWindow = window.FindFirstDescendant(cf => cf.ByName("ListboxWindow")).AsWindow();
                listBox = listBoxWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstLstbox")).AsListBox();
                listBoxlength = listBoxWindow.FindFirstDescendant(cf => cf.ByAutomationId("tstlengthLstbox")).AsListBox();
            }
        }

        //test if listbox is available in window
        [Test]
        public void lst1_isListboxAvailable()
        {
            Assert.IsNotNull(listBoxWindow);
            Assert.IsNotNull(listBox);
        }

        //test if listbox is empty or populated with default values
        [Test]
        public void lst2_isListBoxEmpty()
        {
            var selectListItems = listBox.Items;
            Assert.That(selectListItems.Length, Is.GreaterThan(0));
        }

        //test if listbox return default selected item
        [Test]
        public void lst3_defaultSelectedItem()
        {
            var selectList = listBox.SelectedItem;
            var selectIndex = listBox.Items.ElementAt(0);
            Assert.That(selectList, Is.EqualTo(selectIndex));
        }

        //test selecting listbox item on mouse click
        [Test]

        public void lst4_selectItemMouseClick()
        {
            var selectList = listBox.SelectedItem;
            listBox.Items.ElementAt(3).Focus();
            Wait.UntilInputIsProcessed();
            listBox.Items.ElementAt(3).Click();
            Wait.UntilInputIsProcessed();
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }
        //test if listbox return selected item
        [Test]
        public void lst5_isSelectedItemByIndex()
        {
            var selectList = listBox.Select(2);
            var textOfIndex = listBox.Items.ElementAt(2);
            Assert.That(selectList.Text, Is.EqualTo(textOfIndex.Text));
        }

        //test list item by text
        [Test]
        public void lst6_isSelectedByItemText()
        {
            var selectList = listBox.Select("Red");
            var selectedItem = listBox.SelectedItem;
            Assert.That(selectList, Is.EqualTo(selectedItem));
        }

        //test scrollbar for fixed length of listbox
        [Test]
        public void lst7_verticalScrollBarforFixedlength()
        {
            Assert.That(listBox.Patterns.Scroll.Pattern.VerticallyScrollable.Value, Is.False);
            Assert.That(listBoxlength.Patterns.Scroll.Pattern.VerticallyScrollable.Value, Is.True);
        }

        //test keyboard navigate with down arrow
        [Test]
        public void lst8_keyBoarNavigateDown()
        {
            var selectList = listBox.SelectedItem;
            listBox.SelectedItem.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.DOWN);
            var newSelectList = listBox.SelectedItem;
            Assert.That(selectList, Is.Not.EqualTo(newSelectList));
        }

        //test keyboard navigate with up arrow
        [Test]
        public void lst9_keyBoarNavigateUp()
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

        public void lstb1_verticalScroll()
        {
            double defaultScroll = 0;
            Assert.That(listBoxlength.Patterns.Scroll.Pattern.VerticalScrollPercent, Is.EqualTo(defaultScroll));
            listBoxlength.Patterns.Scroll.Pattern.Scroll(ScrollAmount.NoAmount, ScrollAmount.SmallIncrement);
            Assert.That(listBoxlength.Patterns.Scroll.Pattern.VerticalScrollPercent, Is.Not.EqualTo(defaultScroll)); 
            listBoxWindow.Close();
            window.Close();
        }
     
    }
}
