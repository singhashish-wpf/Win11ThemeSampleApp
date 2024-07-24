using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using NUnit.Framework.Internal;
using System.Configuration;

namespace Win11ThemeTest
{
    public class SliderTest
    {
        private readonly Application? app;
        private readonly Window? window;
        public Window? sliderWindow;
        readonly Button? testSlider;
        readonly Slider? slider;

        public SliderTest()
        {
            var appPath = ConfigurationManager.AppSettings["Testpath"];
            app = LaunchApplication(appPath);
            using var automation = new UIA3Automation();
            window = app?.GetMainWindow(automation);
            testSlider = window?.FindFirstDescendant(cf => cf.ByAutomationId("sliderButton")).AsButton();
            ClickButton(testSlider);
            sliderWindow = window?.FindFirstDescendant(cf => cf.ByName("SliderWindow")).AsWindow();
            slider = sliderWindow?.FindFirstDescendant(cf => cf.ByAutomationId("slider")).AsSlider();
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

        //test if slider is available
        [Test]
        public void Slider1_IsSliderAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(sliderWindow, Is.Not.Null);
                Assert.That(slider, Is.Not.Null);
            });
        }

        [Test]
        public void Slider2_MinMaxOfSlider()
        {
            Assert.That(slider, Is.Not.Null);
            Assert.That(slider.Minimum, Is.EqualTo(0));
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Maximum, Is.EqualTo(10));
        }

        [Test]
        public void Slider3_LargeIncrement()
        {
            Assert.That(slider, Is.Not.Null);
            slider.LargeIncrement();
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Value, Is.Not.EqualTo(0));
            slider.Value = 0;
        }

        [Test]
        public void Slider4_ThumbSlide()
        {
            Assert.That(slider, Is.Not.Null);
            var thumb = slider.Thumb;
            thumb.SlideHorizontally(50);
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Value, Is.EqualTo(5));
            slider.Value = 0;
        }

        [Test]
        public void Slider5_LargeDecrement()
        {
            Assert.That(slider, Is.Not.Null);
            slider.LargeIncrement();
            Assert.That(slider.Value, Is.Not.EqualTo(0));
            slider.LargeDecrement();
            Assert.That(slider.Value, Is.EqualTo(0));
        }

        [Test]
        public void Slider6_SmallDecrement()
        {
            Assert.That(slider, Is.Not.Null);
            var btn = slider.FindFirstChild(cf => cf.ByAutomationId("IncreaseLarge")).AsButton();
            btn.Click();
            var increasedValue = slider.Value;
            var smallChangeValue = slider.SmallChange;
            Keyboard.Press(VirtualKeyShort.LEFT);
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Value, Is.EqualTo(increasedValue - smallChangeValue));
        }

        [Test]
        public void Slider7_SmallIncrement()
        {
            Assert.That(slider, Is.Not.Null);
            var btn = slider.FindFirstChild(cf => cf.ByAutomationId("IncreaseLarge")).AsButton();
            btn.Click();
            var increasedValue = slider.Value;
            var smallChangeValue = slider.SmallChange;
            Keyboard.Press(VirtualKeyShort.RIGHT);
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Value, Is.EqualTo(increasedValue + smallChangeValue));
        }

        [Test]
        public void Slider8_CloseWindows()
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
