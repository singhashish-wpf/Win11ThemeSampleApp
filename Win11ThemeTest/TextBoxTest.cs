using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;
using FlaUI.UIA3.Identifiers;
using System.Drawing;
using System.Configuration;

namespace Win11ThemeTest
{
    [TestFixture]
    public class TextBoxTests
    {
        private readonly Application? app;
        private readonly Window? mainWindow;
        public Window? textWindow;
        readonly TextBox? textBox;
        readonly TextBox? disabledTextBox;
        readonly TextBox? multiLineTextBox;
        readonly Button? txtButton;
        readonly UIA3Automation automation = new();

        public TextBoxTests()
        {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = LaunchApplication(appPath);
                mainWindow = app?.GetMainWindow(automation);
                txtButton = mainWindow?.FindFirstDescendant(cf => cf.ByAutomationId("txtBoxButton")).AsButton();
                ClickButton(txtButton);
                textWindow = mainWindow?.FindFirstDescendant(cf => cf.ByName("TextWindow")).AsWindow();
                textBox = textWindow?.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt")).AsTextBox();
                disabledTextBox = textWindow?.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt_disabled")).AsTextBox();
                multiLineTextBox = textWindow?.FindFirstDescendant(cf => cf.ByAutomationId("tbTxt_multiline")).AsTextBox();
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

        [Test]
        public void Textbox_FindTextBox()
        {
            Assert.Multiple(() =>
            {
                Assert.That(textWindow, Is.Not.Null);
                Assert.That(textBox, Is.Not.Null);
            });
        }

        #region FunctionalTests
        [Test]
        public void TextBox_EnterText()
        {
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.That(textBox.Text, Is.EqualTo("Hello World!"));
        }

        //Verify that the text box accepts alphanumeric characters.
        [Test]
        public void TextBox_EnterAlphaNumericText()
        {
            var expectedText = "/\\d.*[a-zA-Z]|[a-zA-Z].*\\d/";
            Assert.That(textBox, Is.Not.Null);
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
        public void TextBox_EnterSpecialCharText()
        {
            var expectedText = "@#$%^&*";
            Assert.That(textBox, Is.Not.Null);
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
        public void TextBox_EnterEmptyText()
        {
            var emptyText = string.Empty;
            Assert.That(textBox, Is.Not.Null);
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
        public void TextBox_EnterEmailIdText()
        {
            var expectedText = "ram.shyam1234@yahoo.com";
            Assert.That(textBox, Is.Not.Null);
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

        [Test]
        public void TextBox_MultiLineTextbox()
        {
            string testDescription = "New line text.\nLine1\nLine2 \nLine3";
            Assert.That(multiLineTextBox, Is.Not.Null);
            multiLineTextBox.Text = testDescription;
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(800));
            string expectedText = "New line text." + "\n" + "Line1" + "\n" + "Line2 " + "\n" + "Line3";
            // Get text from the textbox and verify
            Assert.That(expectedText, Is.EqualTo(multiLineTextBox.Text));
        }

        //Test the undo and redo functionality within the text box.
        [Test]
        public void TextBox1_UndoText()
        {
            var expectedText = "";
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("This is a textbox. Trying to perform Functionality test for Undo");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Z);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
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
        public void TextBox12_RedoText()
        {
            var expectedText = "This is a textbox. Trying to perform Redo";
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("This is a textbox. Trying to perform Redo");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            //Undo
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Z);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Z);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            //Redo
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_Y);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
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
        public void TextBox2_RightClickTestCut()
        {
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(textWindow, Is.Not.Null);
            var cutText = textWindow.FindFirstDescendant(cf => cf.ByName("Cut")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.That(cutText, Is.Not.Null);
            Assert.That(cutText.IsEnabled, Is.True);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            cutText.Click();
            Wait.UntilInputIsProcessed();
        }

        [Test]
        public void TextBox21_RightClickTestCopy()
        {
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.KEY_A);
            Keyboard.Release(FlaUI.Core.WindowsAPI.VirtualKeyShort.CONTROL);
            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(textWindow, Is.Not.Null);
            var pasteText = textWindow.FindFirstDescendant(cf => cf.ByName("Copy")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            pasteText.GetClickablePoint();
            Assert.That(pasteText, Is.Not.Null);
            pasteText.Click();
            Wait.UntilInputIsProcessed();
        }

        [Test]
        public void TextBox212_RightClickTest_Paste()
        {
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("Hello World!Hello World!Hello World!Hello World!Hello World!Hell World!Hello World!Hello World!Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            textBox.RightClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            Assert.That(textWindow, Is.Not.Null);
            var pasteText = textWindow.FindFirstDescendant(cf => cf.ByName("Paste")).AsMenuItem();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Assert.That(pasteText, Is.Not.Null);
            pasteText.Click();
        }

        #endregion

        #region NegativeTests
        /* Negative Test Scenarios */
        //Attempt to enter code snippets or HTML code into the input box to see if the same is rejected.
        [Test]
        public void TextBox3_HtmlTextBox()
        {
            Assert.That(textBox, Is.Not.Null);
            textBox.Text = string.Empty;
            textBox.Enter("<script>alert(\"123\")</script>");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(800));
            Assert.That(textBox.Text, Is.EqualTo("<script>alert(\"123\")</script>"));
        }

        /* Test Cases For Disabled TextBox
         */
        [Test]
        public void TextBox3_DisabledTextBoxAvailability()
        {
            Assert.Multiple(() =>
            {
                Assert.That(textWindow, Is.Not.Null);
                Assert.That(disabledTextBox, Is.Not.Null);
            });
        }

        //Check if any pre-populated value should be displayed as per requirement.
        [Test]
        public void TextBox3_DisabledTextBox()
        {
            Assert.That(textWindow, Is.Not.Null);
            Wait.UntilInputIsProcessed();
            Assert.That(disabledTextBox, Is.Not.Null);
            Assert.That(disabledTextBox.Text, Is.EqualTo("TextBox Disabled"));
        }

        //Check if you cannot edit disabled TextBox.
        [Test]
        public void TextBox_DisabledEditTextBox()
        {
            Assert.That(textWindow, Is.Not.Null);
            Wait.UntilInputIsProcessed();
            Assert.That(disabledTextBox, Is.Not.Null);
            Assert.That(disabledTextBox.IsEnabled, Is.False);
        }
        #endregion

        #region UITests
        [Test]
        public void TextBox_ClickOnTextbox()
        {
            Assert.That(textBox, Is.Not.Null);
            Mouse.Click(textBox.GetClickablePoint());
            Assert.That(textBox.IsEnabled, Is.True);
        }

        [Test]
        public void TextBox_FontFamily()
        {
            string expected_FontFamily = "Segoe UI";
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("Hello World!");
            var colorRange = textBox.Patterns.Text.Pattern;
            var textFont = (string)colorRange.DocumentRange.GetAttributeValue(automation.TextAttributeLibrary.FontName);
            Assert.That(textFont, Is.EqualTo(expected_FontFamily));
        }

        [Test]
        public void TextBox_TextForegroundColor()
        {
            // var automation = new UIA3Automation();
            Assert.That(textBox, Is.Not.Null);
            textBox.Enter("Hello World!");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            var expectedColor = ColorTranslator.FromHtml("#E4000000");
            var pattern = textBox.Patterns.Text.Pattern;
            var foreColorInt = (int)pattern.DocumentRange.GetAttributeValue(TextAttributes.ForegroundColor);
            var actualColor = Color.FromArgb(foreColorInt);
            Console.WriteLine("actualColor........{0}", actualColor);
            AssertColorEquality(actualColor, Color.FromArgb(0, expectedColor));
        }

        private static void AssertColorEquality(Color actual, Color expected)
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

        [Test]
        public void TextBox4_Cleanup()
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
