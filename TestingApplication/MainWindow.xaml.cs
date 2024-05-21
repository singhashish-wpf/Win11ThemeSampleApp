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

        private void clickEvent(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button Clicked");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //  this.NavigationService.Navigate(new Uri("Page3.xaml", UriKind.Relative));         
            ButtonWindow btnwindow = new ButtonWindow();
            btnwindow.Owner = this;
            //Added for fixing blank window issue
            Application.Current.MainWindow = btnwindow;         
            btnwindow.Show();
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {          
            CalendarWindow clndrwindow = new CalendarWindow();
            clndrwindow.Owner = this;
            //Added for fixing blank window issue
            Application.Current.MainWindow = clndrwindow;
            clndrwindow.Show();
        }

        private void TextBoxButton_Click(object sender, RoutedEventArgs e)
        {
            TextWindow tw = new TextWindow();
            tw.Owner = this;
            Application.Current.MainWindow = tw;
            tw.Show();
        }

        private void ComboBoxButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxViewModel vm = new ComboBoxViewModel();
            ComboBoxWindow cbw = new ComboBoxWindow(vm);
            cbw.Owner = this;
            cbw.Show();
        }

        private void CheckboxButton_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxViewModel viewModel=new CheckBoxViewModel();
            CheckboxWindow checkboxWindow = new CheckboxWindow(viewModel);
            //Added for fixing blank window issue
            Application.Current.MainWindow = checkboxWindow;
            checkboxWindow.Owner = this;
            checkboxWindow.Show();
        }

        private void ListboxButton_Click(object sender, RoutedEventArgs e)
        {
            ListboxWindow lstbxwindow = new ListboxWindow();
            lstbxwindow.Owner = this;
            //Added for fixing blank window issue
            Application.Current.MainWindow = lstbxwindow;
            lstbxwindow.Show();
        }

        private void DatepickerButton_Click(object sender, RoutedEventArgs e)
        {
           DatepickerWindow dtWindow = new DatepickerWindow();
            dtWindow.Owner = this;
            //Added for fixing blank window issue
            Application.Current.MainWindow = dtWindow;
            dtWindow.Show();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButtonWindow radioButtonWindow = new RadioButtonWindow();
            radioButtonWindow.Owner = this;
            //Added for fixing blank window issue
            Application.Current.MainWindow = radioButtonWindow;
            radioButtonWindow.Show();
        }
    }
}