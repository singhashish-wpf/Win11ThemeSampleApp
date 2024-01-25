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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;

namespace Win11ThemeTest
{
    [TestFixture]
    public class textBoxTests
    {
        private Application app;
        private Window window;      
        private Window MainWindow;
        private Window TextWindow;
        private Window btnWindow;
        MenuItems menuItemWindow;
        TextBox textBox;
        TextBox disablexTextBox;
        Button txtButton;
        TextBox multilineTextbox;
        public textBoxTests()
        {
            app = FlaUI.Core.Application.Launch(@"..\\..\\..\\..\\TestingApplication\\bin\\Debug\\net9.0-windows\\win-x64\\TestingApplication.exe");
            using (var automation = new UIA3Automation())
            {
                MainWindow = app.GetMainWindow(automation);
                txtButton = MainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtBoxButton")).AsButton();
                Mouse.Click(txtButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                TextWindow = MainWindow.FindFirstDescendant(cf => cf.ByName("TextWindow")).AsWindow();
                btnWindow = MainWindow.FindFirstDescendant(cf => cf.ByName("lbl")).AsWindow();               
            }
        }

        [Test]
        public void tb1_findTextBox()
        {
            Assert.IsNotNull(TextWindow);
            textBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
            Assert.IsNotNull(textBox);
        }

        #region FunctionalTests

        [Test]
        public void tb2_enterText()
        {
            textBox.Enter("Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.That(textBox.Text, Is.EqualTo("Hello World!"));
        }

        //Verify that the text box accepts alphanumeric characters.
        [Test]
        public void tb3_enterAplhaNumericText()
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
        public void tb4_enterSpecialCharText()
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
        public void tb5_enterEmptyText()
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
        public void tb6_enterEmailIdText()
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
        public void tb7_UndoText()
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
        public void tb8_RedoText()
        {
            textBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
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
        [Test]
        public void tb9_rightClickTest_Cut()
        {
            textBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();

            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));

            var cutText = TextWindow.FindFirstDescendant(cf => cf.ByName("Cut")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.IsNotNull(cutText);
            
            Assert.IsTrue(cutText.IsEnabled);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            cutText.Click();
            Wait.UntilInputIsProcessed();      

        }

        [Test]
        public void tbb11_rightClickTest_Copy()
        {
            textBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();

            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
           
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);

            //Assert.IsNotNull(textBox);
            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));

            var pasteText = TextWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(3000));
            pasteText.GetClickablePoint();
            Assert.IsNotNull(pasteText);
            pasteText.Click();
            Wait.UntilInputIsProcessed();
           
        }

        [Test]
        public void tbb10_rightClickTest_Paste()
        {
            textBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();

            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));

            var pasteText = TextWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.IsNotNull(pasteText);
            pasteText.Click();
        }

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
       */


        //Attempt to enter code snippets or HTML code into the input box to see if the same is rejected.
        [Test]
        public void tb1_htmlTextBox()
        {
            textBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
            textBox.Text = string.Empty;
            textBox.Enter("<script>alert(\"123\")</script>");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(textBox.Text, Is.EqualTo("<script>alert(\"123\")</script>"));
        }       

        /* Test Cases For Disabled TextBox
         */

        [Test]
        public void tb1_disabledTextBoxAvailability()
        {
            Assert.IsNotNull(TextWindow);
            disablexTextBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt_disabled")).AsTextBox();
            Assert.IsNotNull(disablexTextBox);
        }

        //Check if any pre-populated value should be displayed as per requirement.
        [Test]
        public void tb1_disabledTextBox()
        {
            Assert.IsNotNull(TextWindow);
            disablexTextBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt_disabled")).AsTextBox();    
            Assert.That(disablexTextBox.Text, Is.EqualTo("TextBox Disabled"));
        }

        //Check if you cannot edit disabled TextBox.
        [Test]
        public void tb1_disabledEditTextBox()
        {
            Assert.IsNotNull(TextWindow);
            disablexTextBox = TextWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt_disabled")).AsTextBox();
            Assert.That(disablexTextBox.IsEnabled, Is.False);           
        }

        /* Test Cases for Multi-line Text Box
         */    

        #endregion

        #region UITests

        [Test]
        public void tbb1_clickOnTextbox()
        {
            Mouse.Click(textBox.GetClickablePoint());
            Assert.That(textBox.IsEnabled, Is.True);
        }

        [Test]
        public void tbb2_mouseHover()
        {
            Mouse.MoveTo(textBox.GetClickablePoint());
            Assert.That(textBox.Text, Is.EqualTo("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!"));
        }

        [Test]
        public void tbb3_fontFamily()
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
        public void tbb4_fontSize()
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
        public void tbb5_textForegroundColor()
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
        public void tbb6_textBackgroundColor()
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
        #endregion
    }
}
