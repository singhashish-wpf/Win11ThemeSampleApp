using System.Diagnostics;

namespace Win11ThemeTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        public void IfExists()
        {
            // Define the process name
            string processName = "TestingApplication";

            // Find the process by name
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                Console.WriteLine("Process not found.");
                return;
            }
            else
            {
                // Get the first process instance
                Process process = processes[0];
                process.CloseMainWindow();
            }
        }
    }
}