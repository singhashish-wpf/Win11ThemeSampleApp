using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.UIA3;

namespace Win11ThemeTest
{
    public class cbTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckBoxCheckedTest()
        {
            var app = FlaUI.Core.Application.Launch(@"..\\..\\..\\..\\..\\Win11ThemeSampleApp\\bin\\x64\\Debug\\net9.0-windows\\win-x64\\Win11ThemeSampleApp.exe");
            using (var automation = new UIA3Automation())
            {
                var window = app.GetMainWindow(automation);
                var checkBox = window.FindFirstDescendant(cf => cf.ByName("Test Checkbox")).AsCheckBox();
                Assert.That(checkBox.ToggleState, Is.EqualTo(ToggleState.Off));
                checkBox.Toggle();
                Assert.That(checkBox.ToggleState, Is.EqualTo(ToggleState.On));
            }
        }
    }
}