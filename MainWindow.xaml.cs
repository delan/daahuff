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
            FrequencyTable table = new FrequencyTable();
            table.loadUIString(txtFreqTbl.Text);
            if (table.freq.Count > 0)
            {
                byte[] input = Encoding.UTF8.GetBytes(txtPlain.Text);
                try
                {
                    byte[] output = HuffmanTranscoder.deflateUTF8(input, table);
                    txtCompressed.Text = Base64.encode(output, Base64.Hannes);
                }
                catch (KeyNotFoundException)
                {
                    MessageBox.Show("Please ensure that the frequency table is complete for the given input.");
                }
            }
            else
            {
                MessageBox.Show("Please ensure that the frequency table is not empty.");
            }
        }

        private void btnDecompress_Click(object sender, RoutedEventArgs e)
        {
            FrequencyTable table = new FrequencyTable();
            table.loadUIString(txtFreqTbl.Text);
            if (table.freq.Count > 0)
            {
                byte[] input = Base64.decode(txtCompressed.Text, Base64.Hannes);
                byte[] output = HuffmanTranscoder.inflate(input, table);
                txtPlain.Text = Encoding.UTF8.GetString(output);
            }
            else
            {
                MessageBox.Show("Please ensure that the frequency table is not empty.");
            }
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
