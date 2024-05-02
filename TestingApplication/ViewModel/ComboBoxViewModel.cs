﻿

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication.ViewModel
{
    public partial class ComboBoxViewModel : ObservableObject
    {
        [ObservableProperty]
        private IList<string> _comboBoxFontFamilies = new ObservableCollection<string>
        {
        "Arial",
        "Comic Sans MS",
        "Segoe UI",
        "Times New Roman"
        };


        [ObservableProperty]
        private IList<int> _comboBoxFontSizes = new ObservableCollection<int>
    {
        8,
        9,
        10,
        11,
        12,
        14,
        16,
        18,
        20,
        24,
        28,
        36,
        48,
        72
        };
    }
}