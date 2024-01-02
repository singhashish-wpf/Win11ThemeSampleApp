using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Win11ThemeSampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeRadioButtons();
        }

        public void InitializeRadioButtons()
        {
            List<RadioButtonData> radioButtons = new List<RadioButtonData>
            {
                new RadioButtonData { Name = "radioButton2_1", Content = "RadioButton4"  },
                new RadioButtonData { Name = "radioButton2_2", Content = "RadioButton5" },
                new RadioButtonData { Name = "radioButton2_3", Content = "RadioButton6" }
            };

            foreach (var radioButtonItem in radioButtons)
            {
                var radioButton = new RadioButton
                {
                    Name = radioButtonItem.Name,
                    Content = radioButtonItem.Content,
                    GroupName = "Group2"
                };

                if (radioButton.Name == "radioButton2_3")
                {
                    radioButton.IsEnabled = false;
                }

                getRadioButtons.Children.Add(radioButton);
            }
        }
    }

    public class RadioButtonData
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }
}