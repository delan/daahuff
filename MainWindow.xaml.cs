using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections;

namespace Asgn
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

        private void btnCompress_Click(object sender, RoutedEventArgs e)
        {
            byte[] input = Encoding.UTF8.GetBytes(txtPlain.Text);
            txtCompressed.Text = Base64.encode(input, Base64.Hannes);
        }

        private void btnDecompress_Click(object sender, RoutedEventArgs e)
        {
            string input = txtCompressed.Text;
            txtPlain.Text = Encoding.UTF8.GetString(Base64.decode(input, Base64.Hannes));
        }

        private void btnFreq_Click(object sender, RoutedEventArgs e)
        {
            FrequencyTable table = new FrequencyTable();
            byte[] input = Encoding.UTF8.GetBytes(txtPlain.Text);
            table.generateFromUTF8(input);
            txtFreqTbl.Text = table.ToString();
        }
    }
}
