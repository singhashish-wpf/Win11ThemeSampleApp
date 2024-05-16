using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TestingApplication.ViewModel;

namespace TestingApplication
{
    /// <summary>
    /// Interaction logic for CheckboxWindow.xaml
    /// </summary>
    public partial class CheckboxWindow : Window
    {
        public CheckBoxViewModel ViewModel { get; }
        public CheckboxWindow(CheckBoxViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        
    }
}
