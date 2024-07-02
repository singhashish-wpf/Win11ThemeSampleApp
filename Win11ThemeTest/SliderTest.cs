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
            try
            {
                var appPath = ConfigurationManager.AppSettings["Testpath"];
                app = Application.Launch(appPath);
                using var automation = new UIA3Automation();
                window = app.GetMainWindow(automation);
                testSlider = window.FindFirstDescendant(cf => cf.ByAutomationId("sliderButton")).AsButton();
                Mouse.Click(testSlider.GetClickablePoint());
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
                sliderWindow = window.FindFirstDescendant(cf => cf.ByName("SliderWindow")).AsWindow();
                slider = sliderWindow.FindFirstDescendant(cf => cf.ByAutomationId("slider")).AsSlider();
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

        //test if slider is available
        [Test]
        public void Slider1_isSliderAvailable()
        {
            Assert.Multiple(() =>
            {
                Assert.That(sliderWindow, Is.Not.Null);
                Assert.That(slider, Is.Not.Null);
            });
        }

        [Test]
        public void Slider2_minMaxOfSlider()
        {
            Assert.That(slider, Is.Not.Null);
            Assert.That(slider.Minimum, Is.EqualTo(0));
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Maximum, Is.EqualTo(10));
        }

        [Test]
        public void Slider3_largeIncrement()
        {
            Assert.That(slider, Is.Not.Null);
            slider.LargeIncrement();
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Value, Is.Not.EqualTo(0));
            slider.Value = 0;
        }

        [Test]
        public void Slider4_thumbSlide()
        {
            Assert.That(slider, Is.Not.Null);
            var thumb = slider.Thumb;
            thumb.SlideHorizontally(50);
            Wait.UntilInputIsProcessed();
            Assert.That(slider.Value, Is.EqualTo(5));
            slider.Value = 0;
        }

        [Test]
        public void Slider5_largeDecrement()
        {
            Assert.That(slider, Is.Not.Null);
            slider.LargeIncrement();
            Assert.That(slider.Value, Is.Not.EqualTo(0));
            slider.LargeDecrement();
            Assert.That(slider.Value, Is.EqualTo(0));
        }

        [Test]
        public void Slider6_smallDecrement()
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
        public void Slider7_smallIncrement()
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
        public void Slider8_closeWindows()
        {
            if (app != null)
            {
                app.Close();
                Console.WriteLine("Application closed successfully.");
                Assert.That(app.Close());
            }
            else
            {
                Console.WriteLine("Application not found.");
                Assert.That(app.Close());
            }
        }
    }
}
