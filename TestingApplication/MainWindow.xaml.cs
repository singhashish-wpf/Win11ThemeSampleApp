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
using TestingApplication.ViewModel;


namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClickEvent(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button Clicked");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //  this.NavigationService.Navigate(new Uri("Page3.xaml", UriKind.Relative));         
            ButtonWindow btnwindow = new()
            {
                Owner = this
            };
            //Added for fixing blank window issue
            Application.Current.MainWindow = btnwindow;         
            btnwindow.Show();
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            CalendarWindow clndrwindow = new()
            {
                Owner = this
            };
            //Added for fixing blank window issue
            Application.Current.MainWindow = clndrwindow;
            clndrwindow.Show();
        }

        private void TextBoxButton_Click(object sender, RoutedEventArgs e)
        {
            TextWindow tw = new()
            {
                Owner = this
            };
            Application.Current.MainWindow = tw;
            tw.Show();
        }

        private void ComboBoxButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxViewModel vm = new();
            ComboBoxWindow cbw = new(vm)
            {
                Owner = this
            };
            Application.Current.MainWindow = cbw;
            cbw.Show();
        }

        private void CheckboxButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxViewModel viewModel = new();
            CheckboxWindow checkboxWindow = new(viewModel);
            //Added for fixing blank window issue
            Application.Current.MainWindow = checkboxWindow;
            checkboxWindow.Owner = this;
            checkboxWindow.Show();
        }

        private void ListboxButton_Click(object sender, RoutedEventArgs e)
        {
            ListboxWindow lstboxwindow = new()
            {
                Owner = this
            };
            //Added for fixing blank window issue
            Application.Current.MainWindow = lstboxwindow;
            lstboxwindow.Show();
        }
        private void SliderButton_Click(object sender, RoutedEventArgs e)
        {
            SliderWindow sliderWindow = new()
            {
                Owner = this
            };
            Application.Current.MainWindow = sliderWindow;
            sliderWindow.Show();
        }
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButtonWindow radioButtonWindow = new()
            {
                Owner = this
            };
            //Added for fixing blank window issue
            Application.Current.MainWindow = radioButtonWindow;
            radioButtonWindow.Show();
        }
    }
}