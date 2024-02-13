using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestingApplication.ViewModel
{
    public partial class CheckBoxViewModel: ObservableObject
    {

        [ObservableProperty]
        private bool? _selectAllCheckBoxChecked = false;

        [ObservableProperty]
        private bool _optionOneCheckBoxChecked = false;

        [ObservableProperty]
        private bool _optionTwoCheckBoxChecked = false;

        [ObservableProperty]
        private bool _optionThreeCheckBoxChecked = false;

      

        [RelayCommand]
        private void OnSelectAllChecked(object sender)
        {
            if (sender is not CheckBox checkBox)
                return;

            if (checkBox.IsChecked == null)
                checkBox.IsChecked = !(
                    OptionOneCheckBoxChecked && OptionTwoCheckBoxChecked && OptionThreeCheckBoxChecked
                );

            if (checkBox.IsChecked == true)
            {
                OptionOneCheckBoxChecked = true;
                OptionTwoCheckBoxChecked = true;
                OptionThreeCheckBoxChecked = true;
            }
            else if (checkBox.IsChecked == false)
            {
                OptionOneCheckBoxChecked = false;
                OptionTwoCheckBoxChecked = false;
                OptionThreeCheckBoxChecked = false;
            }
        }

        [RelayCommand]
        private void OnSingleChecked(string option)
        {
            if (OptionOneCheckBoxChecked && OptionTwoCheckBoxChecked && OptionThreeCheckBoxChecked)
                SelectAllCheckBoxChecked = true;
            else if (!OptionOneCheckBoxChecked && !OptionTwoCheckBoxChecked && !OptionThreeCheckBoxChecked)
                SelectAllCheckBoxChecked = false;
            else
                SelectAllCheckBoxChecked = null;
        }

       
        
    }
}
