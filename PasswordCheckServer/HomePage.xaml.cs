﻿using System;
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

namespace PasswordCheckServer
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Brute_Click(object sender, RoutedEventArgs e)
        {
            BruteForcePage brute = new BruteForcePage();
            this.NavigationService.Navigate(brute);
        }

        private void Dictionary_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
