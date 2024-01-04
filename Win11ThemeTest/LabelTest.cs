using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;

namespace Win11ThemeTest
{
    public class LabelTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LabelControlTest()
        {
            var app = FlaUI.Core.Application.Launch(@"..\\..\\..\\..\\Win11ThemeSampleApp\\bin\\x64\\Debug\\net9.0-windows\\win-x64\\Win11ThemeSampleApp.exe");
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                var label = window.FindFirstDescendant(cf => cf.ByName("Demo label")).AsLabel();

                Assert.That(label.ActualHeight, Is.EqualTo(30));
                Assert.That(label.ActualWidth, Is.EqualTo(100));
                Assert.That(label.Text, Is.EqualTo("Demo label"));
                Assert.False(label.IsEnabled);
                Assert.That(label.HelpText, Is.EqualTo("Demo help text"));
            }
        }
    }
}