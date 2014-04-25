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
            table.LoadUIString(txtFreqTbl.Text);
            if (table.Freq.Count > 0 && txtPlain.Text.Length > 0)
            {
                try
                {
                    DAABitArray output = HuffmanTranscoder.Deflate(txtPlain.Text, table);
                    txtCompressed.Text = Base64.Encode(output);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Please ensure that the symbol and plain fields are not empty.");
            }
        }

        private void btnDecompress_Click(object sender, RoutedEventArgs e)
        {
            FrequencyTable table = new FrequencyTable();
            table.LoadUIString(txtFreqTbl.Text);
            if (table.Freq.Count > 0 && txtCompressed.Text.Length > 0)
            {
                try
                {
                    DAABitArray input = Base64.Decode(txtCompressed.Text);
                    txtPlain.Text = HuffmanTranscoder.Inflate(input, table);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Please ensure that the symbol and compressed fields are not empty.");
            }
        }

        private void btnFreq_Click(object sender, RoutedEventArgs e)
        {
            FrequencyTable table = new FrequencyTable();
            table.Generate(txtPlain.Text);
            txtFreqTbl.Text = table.ToUIString();
        }
    }
}
