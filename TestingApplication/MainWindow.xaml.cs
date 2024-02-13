﻿using System.Text;
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
            CheckBoxViewModel viewModel=new CheckBoxViewModel();
            CheckboxWindow chkbxwindow = new CheckboxWindow(viewModel);
            chkbxwindow.Owner = this;
            chkbxwindow.Show();
        }
    }
}