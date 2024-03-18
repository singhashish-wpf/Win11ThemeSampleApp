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
using System.Configuration;

namespace Win11ThemeTest
{
    [TestFixture]
    public class textBoxTests
    {
        private Application app;
        private Window mainWindow;
        public Window textWindow;
        TextBox textBox;
        TextBox disabledTextBox;
        Button txtButton;
        UIA3Automation automation = new UIA3Automation();
        
        public textBoxTests()
        {
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);

                mainWindow = app.GetMainWindow(automation);
                txtButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("txtBoxButton")).AsButton();
                Mouse.Click(txtButton.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
                textWindow = mainWindow.FindFirstDescendant(cf => cf.ByName("TextWindow")).AsWindow();
                textBox = textWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
                disabledTextBox = textWindow.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt_disabled")).AsTextBox();
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

        [Test]
        public void z_Cleanup()
        {            
            textWindow.Close();
            mainWindow.Close();
        }

        [Test]
        public void tb_findTextBox()
        {
            Assert.IsNotNull(textWindow);           
            Assert.IsNotNull(textBox);
        }

        #region FunctionalTests

        [Test]
        public void tb1_enterText()
        {
            textBox.Enter("Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.That(textBox.Text, Is.EqualTo("Hello World!"));
        }

        //Verify that the text box accepts alphanumeric characters.
        [Test]
        public void tb1_enterAplhaNumericText()
        {
            var expectedText = "/\\d.*[a-zA-Z]|[a-zA-Z].*\\d/";

            textBox.Enter("/\\d.*[a-zA-Z]|[a-zA-Z].*\\d/");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));            
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
        public void tb1_enterSpecialCharText()
        {
            var expectedText = "@#$%^&*";

            textBox.Enter("@#$%^&*");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));            
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
        public void tb1_enterEmptyText()
        {
            var emptyText = string.Empty;

            textBox.Enter(emptyText);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));            
            if (textBox.Text != string.Empty)
            {
                Assert.Fail("Result : {0} and Expected {1} ", textBox.Text, string.Empty);
            }
            if (textBox.Text == string.Empty)
            {
                Assert.Pass("Result : {0} and Expected {1} ", textBox.Text, string.Empty);
            }
        }      

        //Test input validation for correct data formats (e.g., email validation).
        [Test]
        public void tb1_enterEmailIdText()
        {
            var expectedText = "ram.shyam1234@yahoo.com";
            textBox.Enter("ram.shyam1234@yahoo.com");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));            
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
        public void tb2_UndoText()
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
        public void tb3_RedoText()
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
        [Test]
        public void tb4_rightClickTest_Cut()
        {         
            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            var cutText = textWindow.FindFirstDescendant(cf => cf.ByName("Cut")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.IsNotNull(cutText);
            
            Assert.IsTrue(cutText.IsEnabled);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            cutText.Click();
            Wait.UntilInputIsProcessed();      

        }

        [Test]
        public void tb5_rightClickTest_Copy()
        {           
            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
           
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
         
            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            var pasteText = textWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            pasteText.GetClickablePoint();
            Assert.IsNotNull(pasteText);
            pasteText.Click();
            Wait.UntilInputIsProcessed();
           
        }

        [Test]
        public void tb6_rightClickTest_Paste()
        {          
            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            var pasteText = textWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.IsNotNull(pasteText);
            pasteText.Click();
        }    

        #endregion

        #region NegativeTests
        /* Negative Test Scenarios
       *        
       */

        //Attempt to enter code snippets or HTML code into the input box to see if the same is rejected.
        [Test]
        public void tb7_htmlTextBox()
        {            
            textBox.Text = string.Empty;
            textBox.Enter("<script>alert(\"123\")</script>");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.That(textBox.Text, Is.EqualTo("<script>alert(\"123\")</script>"));
        }       

        /* Test Cases For Disabled TextBox
         */

        [Test]
        public void tb8_disabledTextBoxAvailability()
        {
            Assert.IsNotNull(textWindow);
            Assert.IsNotNull(disabledTextBox);
        }

        //Check if any pre-populated value should be displayed as per requirement.
        [Test]
        public void tb8_disabledTextBox()
        {
            Assert.IsNotNull(textWindow);                
            Assert.That(disabledTextBox.Text, Is.EqualTo("TextBox Disabled"));
        }

        //Check if you cannot edit disabled TextBox.
        [Test]
        public void tb1_disabledEditTextBox()
        {
            Assert.IsNotNull(textWindow);            
            Assert.That(disabledTextBox.IsEnabled, Is.False);           
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
        public void tbb3_fontFamily()
        {
            string expected_FontFamily = "Segoe UI";
            textBox.Enter("Hello World!");
            var colorRange = textBox.Patterns.Text.Pattern;
            var textFont = (string)colorRange.DocumentRange.GetAttributeValue(automation.TextAttributeLibrary.FontName);
            Assert.That(textFont, Is.EqualTo(expected_FontFamily));


        }       

        [Test]
        public void tbb5_textForegroundColor()
        {
           // var automation = new UIA3Automation();
            textBox.Enter("Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            var expectedColor = System.Drawing.ColorTranslator.FromHtml("#E4000000");
            var pattern = textBox.Patterns.Text.Pattern;
            var foreColorInt = (int)pattern.DocumentRange.GetAttributeValue(TextAttributes.ForegroundColor);

            var actualColor = Color.FromArgb(foreColorInt);
            Console.WriteLine("actualColor........{0}", actualColor);
            AssertColorEquality(actualColor, Color.FromArgb(0, expectedColor));
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
