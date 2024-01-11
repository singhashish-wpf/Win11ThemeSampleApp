using System.Text;
using System.Windows;
using System.Windows.Appearance;
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
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SetupDatagrid();
            SystemThemeWatcher.Watch(this);
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            //String themeName = (sender as RadioButton).Content as string;
            //switch (themeName)
            //{
            //    case "Light":
            //        ApplicationThemeManager.Apply(ApplicationTheme.Light);
            //        break;
            //    case "Dark":
            //        ApplicationThemeManager.Apply(ApplicationTheme.Dark);
            //        break;
            //    case "HighContrast":
            //        ApplicationThemeManager.Apply(ApplicationTheme.HighContrast);
            //        break;
            //}
        }

        private void SetupDatagrid()
        {
            List<User> users = new List<User>();
            users.Add(new User() { Id = 1, Name = "John Doe", Birthday = new DateTime(1971, 7, 23) });
            users.Add(new User() { Id = 2, Name = "Jane Doe", Birthday = new DateTime(1974, 1, 17) });
            users.Add(new User() { Id = 3, Name = "Sammy Doe", Birthday = new DateTime(1991, 9, 2) });

            dgSimple.ItemsSource = users;
        }
    }

    public class User
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public DateTime Birthday { get;  set; }
    }
}