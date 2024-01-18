using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.UIA3.Identifiers;
using FlaUI.UIA3.Patterns;
using System.Drawing;
using System.Linq;
using NuGet.Frameworks;

namespace Win11ThemeTest
{
    [TestFixture]
    public class textBoxTests
    {
        private Application app;
        private Window window;
        private Window[] txtWindow;
        private Window[] textWindow;
        MenuItems menuItemWindow;
        TextBox textBox;
        Button txtButton;
        public textBoxTests()
        {
            app = FlaUI.Core.Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");
            using (var automation = new UIA3Automation())
            {
                window = app.GetMainWindow(automation);
                txtButton = window.FindFirstDescendant(cf => cf.ByAutomationId("txtBoxButton")).AsButton();
                Mouse.Click(txtButton.GetClickablePoint());
                textWindow = app.GetAllTopLevelWindows(automation);
                for (int i = 0; i < textWindow.Length; i++)
                {
                    if (textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox().IsAvailable)
                    {
                        //txtwindow = app.GetMainWindow(textWindow[i].Automation);
                        textBox = textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
                    }
                }
            }
        }
        #region FunctionalTests
        
        [Test]
        public void tb_enterText()
        {
            textBox.Enter("Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.That(textBox.Text, Is.EqualTo("Hello World!"));
        }

        //Verify that the text box accepts alphanumeric characters.
        [Test]
        public void tb_enterAplhaNumericText()
        {
            var expectedText = "/\\d.*[a-zA-Z]|[a-zA-Z].*\\d/";

            textBox.Enter("/\\d.*[a-zA-Z]|[a-zA-Z].*\\d/");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            //Assert.That(textBox.Text, Is.EqualTo("/\\d.*[a-zA-Z]|[a-zA-Z].*\\d/"));
            if (textBox.Text != expectedText)
            {
                Assert.Fail("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
            if (textBox.Text == expectedText)
            {
                Assert.Pass("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
        }

        //Verify that the text box accepts special characters.
        [Test]
        public void tb_enterSpecialCharText()
        {
            var expectedText = "@#$%^&*";

            textBox.Enter("@#$%^&*");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            // Assert.That(textBox.Text, Is.EqualTo("@#$%^&*"));
            if (textBox.Text != "@#$%^&*")
            {
                Assert.Fail("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
            if (textBox.Text == expectedText)
            {
                Assert.Pass("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
        }

        //Check if the text box can handle empty input.
        [Test]
        public void tb_enterEmptyText()
        {
            var emptyText = string.Empty;

            textBox.Enter(emptyText);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            //Assert.That(textBox.Text, Is.EqualTo("@#$%^&*"));
            if (textBox.Text != string.Empty)
            {
                Assert.Fail("Result : {0} and Expected {1} ", textBox.Text, string.Empty);
            }
            if (textBox.Text == string.Empty)
            {
                Assert.Pass("Result : {0} and Expected {1} ", textBox.Text, string.Empty);
            }
        }

        //Test the text box with input up to the maximum character limit.


        //Test input validation for correct data formats (e.g., email validation).
        [Test]
        public void tb_enterEmailIdText()
        {
            var expectedText = "ram.shyam1234@yahoo.com";
            textBox.Enter("ram.shyam1234@yahoo.com");

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            //Assert.That(textBox.Text, Is.EqualTo("@#$%^&*"));
            if (textBox.Text != expectedText)
            {
                Assert.Fail("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
            if (textBox.Text == expectedText)
            {
                Assert.Pass("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
        }

        //Test the undo and redo functionality within the text box.
        [Test]
        public void tb_UndoText()
        {
            var expectedText = "";
            textBox.Enter("This is a textbox. Trying to perform Functionality test for Undo");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Z);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Z);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);

            if (textBox.Text != expectedText)
            {
                Assert.Fail("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
            if (textBox.Text == expectedText)
            {
                Assert.Pass("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }

        }

        [Test]
        public void tb_RedoText()
        {
            var expectedText = "This is a textbox. Trying to perform Functionality test for Redo";
            textBox.Enter("This is a textbox. Trying to perform Functionality test for Redo");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            //Undo
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Z);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Z);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);

            //Redo
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Y);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Y);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);

            if (textBox.Text != expectedText)
            {
                Assert.Fail("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
            if (textBox.Text == expectedText)
            {
                Assert.Pass("Result : {0} and Expected {1} ", textBox.Text, expectedText);
            }
        }

        //Verify that users can copy and paste text from and to the text box.

        //Confirm that the entered text is retained when navigating away and returning to the page.


        #endregion

        #region PostiveTests
        /* Positive  Test Scenarios
       * 
       *  
       *  
       * 
       */

        //Test if users can add trailing and leading whitespace in the text box.

        //Verify the case sensitivity of the text box and its behavior when the correct input is entered.


        //Verify the placeholder in the text box and its function.


        //Test if the text box allows breaking the sentences or the content into multiple lines.

        #endregion

        #region NegativeTests
        /* Negative Test Scenarios
       * 
       *  
       *  
       * 
       */

        //Attempt to enter code snippets or HTML code into the input box to see if the same is rejected.
        //Check if leaving the text box empty generates an appropriate error message.

        /* Test Cases For Disabled TextBox
         */

        /* Test Cases For Enabled TextBox
        */

        /* Test Cases for Single-Line Text Box
        */


        /* Test Cases for Multi-line Text Box
         */

        /* Test Cases For Text Field Validation
        */

        /* Test Cases for Single-Line Text Box
        */
        #endregion

        #region UITests
        [Test]
        public void tb_isAvailable()
        {
            Assert.That(textBox.IsAvailable);
        }

        [Test]
        public void tb_clickOnTextbox()
        {
            Mouse.Click(textBox.GetClickablePoint());
            Assert.That(textBox.IsEnabled, Is.True);
        }

        [Test]
        public void tb_mouseHover()
        {
            Mouse.MoveTo(textBox.GetClickablePoint());
            Assert.That(textBox.Text, Is.EqualTo("TextBoxValue"));
        }

        [Test]
        public void tb_fontFamily()
        {
            using (var automation = new UIA3Automation())
            {
                string expected_FontFamily = "Segoe UI";
                textBox.Enter("Hello World!");
                var colorRange = textBox.Patterns.Text.Pattern;
                var textFont = (string)colorRange.DocumentRange.GetAttributeValue(automation.TextAttributeLibrary.FontName);
                Assert.That(textFont, Is.EqualTo(expected_FontFamily));

            }
        }

        [Test]
        public void tb_fontSize()
        {
            using (var automation = new UIA3Automation())
            {
                // string expected_FontFamily = "Segoe UI";
                double expected_FontSize = 14;
                textBox.Enter("Hello World!");
                var colorRange = textBox.Patterns.Text.Pattern;
                var textFontSize = colorRange.DocumentRange.GetAttributeValue(automation.TextAttributeLibrary.FontSize);
                //var backColorInt = colorRange.DocumentRange.GetAttributeValue(TextAttributes.FontSize);
                Console.WriteLine("textFontSize...........{0}", textFontSize);
                //Console.WriteLine("backColorInt...........{0}", backColorInt);
                Assert.That(textFontSize, Is.EqualTo(expected_FontSize));
            }
        }

        [Test]
        public void tb_textForegroundColor()
        {
            var automation = new UIA3Automation();
            textBox.Enter("Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            var expectedColor = System.Drawing.ColorTranslator.FromHtml("#E4000000");
            var pattern = textBox.Patterns.Text.Pattern;
            var foreColorInt = (int)pattern.DocumentRange.GetAttributeValue(TextAttributes.ForegroundColor);

            var actualColor = Color.FromArgb(foreColorInt);
            Console.WriteLine("actualColor........{0}", actualColor);
            AssertColorEquality(actualColor, Color.FromArgb(0, expectedColor));
        }

        [Test]
        public void tb_textBackgroundColor()
        {
            using (var automation = new UIA3Automation())
            {
                // textBox.Enter("Hello World!");
                var pattern = textBox.Patterns.Text.Pattern;
                var expectedColor = System.Drawing.ColorTranslator.FromHtml("#B3CB1B1B");

                var backColorInt = (int)pattern.DocumentRange.GetAttributeValue(automation.TextAttributeLibrary.BackgroundColor);

                var backColorInt1 = pattern.DocumentRange.GetAttributeValue(automation.TextAttributeLibrary.BackgroundColor);
                var actualBackColor = Color.FromArgb(backColorInt);
                Console.WriteLine(" actualBackColor.....{0}", actualBackColor);
                Console.WriteLine(" backColorInt1.....{0}", backColorInt1);
                var textPattern = textBox.Patterns.Text.Pattern;

                AssertColorEquality(actualBackColor, Color.FromArgb(0, expectedColor));
            }
        }

        private void AssertColorEquality(Color actual, Color expected)
        {
            if (actual.ToArgb() != expected.ToArgb())
            {
                var message =
                $"Expected: Color[A = {expected.A}, R = {expected.R}, G = {expected.G}, B = {expected.B}]{Environment.NewLine}But was: Color[A = {actual.A}, R = {actual.R}, G = {actual.G}, B = {actual.B}]";
                Assert.Fail(message);
            }
            else
            {
                var message =
                $"Expected: Color[A = {expected.A}, R = {expected.R}, G = {expected.G}, B = {expected.B}]{Environment.NewLine}But was: Color[A = {actual.A}, R = {actual.R}, G = {actual.G}, B = {actual.B}]";
                Assert.Pass(message);
            }
        }


        //Functional Testing
        [Test]
        public void tb_OnMouseRightClick()
        {
            using (var automation = new UIA3Automation())
            {
                textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!Hello World!");
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
                Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
                Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
                //Mouse.MoveTo(textBox.GetClickablePoint()); 
                textBox.RightClick();




                Menu x;
                //window.FindFirstDescendant(cf => cf.ByClassName("PopupRoot")).AsMenu();


                textWindow = app.GetAllTopLevelWindows(automation);
                for (int i = 0; i < textWindow.Length; i++)
                {
                    if (textWindow[i].FindFirstDescendant(cf => cf.ByClassName("PopupRoot")).AsMenu().IsAvailable)
                    {
                        //txtwindow = app.GetMainWindow(textWindow[i].Automation);
                        x = textWindow[i].FindFirstDescendant(cf => cf.ByClassName("PopupRoot")).AsMenu();
                    }
                }


                //Window txtwindow2;
                //txtwindow = app.GetAllTopLevelWindows(automation);
                //for (int i = 0; i < txtwindow.Length; i++)
                //{
                //    if (textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox().IsAvailable)
                //    {
                //        //txtwindow = app.GetMainWindow(textWindow[i].Automation);
                //        txtwindow2 = app.GetMainWindow(txtwindow[i].Automation);
                //        //txtwindow2 = app.GetMainWindow(txtwindow[i].FindFirstDescendant(condition => condition.ByAutomationId("ContextMenu")).AsWindow());
                //        textBox = textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
                //    }
                //}



                //var txtwindow2 = app.GetMainWindow(automation);
                // var contextMenu = txtwindow2.GetContextMenuByFrameworkType(FrameworkType.Wpf);
                //  var menuItem = contextMenu.FindFirstDescendant(cf => cf.ByText("Paste")).AsMenuItem();
                //  menuItem.Click();


                ////var txtwindow2 = app.GetMainWindow(automation);


                ////ConditionFactory condition = new ConditionFactory(new UIA3PropertyLibrary());
                ////MenuItem item = txtwindow2.FindFirst(TreeScope.Subtree, condition.ByText("Copy")).AsMenuItem();
                //////This line works in sample but not for my legacy winform application
                ////item.Click();


                // ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

                //// textWindow.FindFirstDescendant().AsTextBox().RightClick();
                // textWindow.FirstOrDefault(cf => cf.ByAutomationId("ContextMenu"))
                // var contextMenu = txtwindow.ContextMenu;
                // contextMenu.DrawHighlight();
                // contextMenu.Items[0].DrawHighlight();
                // //menuItemWindow = 
                // //int fillCcolor = textBox.Properties.FillColor;
                // //for (int i = 0; i < menuItemWindow.Length; i++)
                // //{
                // //    if (textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox().IsAvailable)
                // //    {
                // //        textBox = textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
                // //    }
                // //}

                //txtwindow = app.GetAllTopLevelWindows(automation);
                //for (int i = 0; i < txtwindow.Length; i++)
                //{
                //    //if (txtwindow[i].FindFirstDescendant(cf => cf.ByName("ContextMenu").And(x.ByControlType(ControlType.Menu)).AsMenu()).IsAvailable)
                //    //{
                //    //    //txtwindow = app.GetMainWindow(textWindow[i].Automation);
                //    //    textBox = textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("ContextMenu")).AsTextBox();
                //    //}
                //}

                //var menu = txtwindow.FindFirstDescendant(x => x.ByName("ContextMenu").And(x.ByControlType(ControlType.Menu))).AsMenu();
                //var menu2 = textWindow.FirstOrDefault(x => x.ContextMenu.Items.Count > 0);
                //var menuItems = menu.Items;
                //var menuItem = menuItems.First(x => x.Name == "Copy");
                //Console.WriteLine("{0}", menuItem);
                //menuItem.Click();

                //  Console.WriteLine("{0}", count);
                //for (int i = 0; i < count; i++)
                //{
                //    Console.WriteLine(menu.Items[i]);
                //    //if (textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("Copy")).AsWindow().IsAvailable)
                //    //{
                //    //    // txtwindow = app.GetMainWindow(textWindow[i].Automation);
                //    //    txtwindow = textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("Copy")).AsWindow();
                //    //}
                //}

                //Mouse.MoveTo(menu.);
                //var contextMenu = txtwindow.ContextMenu;
                //contextMenu.DrawHighlight();
                //contextMenu.Items[0].DrawHighlight();

                ////textWindow = app.GetAllTopLevelWindows(automation);
                ////for (int i = 0; i < textWindow.Length; i++)
                ////{
                ////    if (textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("Copy")).AsWindow().IsAvailable)
                ////    {
                ////        // txtwindow = app.GetMainWindow(textWindow[i].Automation);
                ////        txtwindow = textWindow[i].FindFirstDescendant(cf => cf.ByAutomationId("Copy")).AsWindow();
                ////    }
                ////}

                ////Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.DOWN);
                //textBox.RightClick(2);
                //var contextMenu = window.GetContextMenuByFrameworkType(FrameworkType.Wpf);
                // var menu = textBox.AsMenuItem();
                // var menuItem = contextMenu.FindFirstDescendant(cf => cf.ByText("Paste")).AsMenuItem();
                // Console.WriteLine(" menu.....{0}", contextMenu);
                // Console.WriteLine(" menuItem.....{0}", menuItem);
                //menuItem.Click();
                Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
                Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
                Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.DOWN);

                //Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);            
                //textBox.RightClick();
            }
            #endregion

        }
    }
}
