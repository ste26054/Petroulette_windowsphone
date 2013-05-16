using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GalaSoft.MvvmLight.Threading;

namespace MvvmLight4
{
    public partial class MainPage2 : PhoneApplicationPage
    {
        public MainPage2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {

                MessageBox.Show("Great ! Now, we just need to implement the POST message !");
            });
        }
    }
}