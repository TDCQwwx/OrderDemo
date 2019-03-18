﻿using OrderDemo.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderDemo.Views
{
    /// <summary>
    /// UCTakeOrderOrderState.xaml 的交互逻辑
    /// </summary>
    public partial class UCTakeOrderOrderState : UserControl
    {
        public UCTakeOrderOrderState()
        {
            InitializeComponent();
            this.DataContext = UserViewModel.InstanceUserViewModel;
        }
        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
