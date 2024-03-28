using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
    /// Interaction logic for ComboBoxWindow.xaml
    /// </summary>
    public partial class ComboBoxWindow : Window
    {
        public ComboBoxViewModel ViewModel { get; }

        public ComboBoxWindow(ComboBoxViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;            
                 
            InitializeComponent();
        }
    }
}
