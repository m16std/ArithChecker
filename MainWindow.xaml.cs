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
using System.Text.RegularExpressions;

namespace ArithChecker
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



        private void add_output(string word, int depth)
        {
            for (int i = 0; i < depth; i++)
                output_textbox.Text += "    ";
            output_textbox.Text += word + "\n";
        }

        private bool checker(int left, int right, int depth)
        {

            if(left == right)
                return true;

            for (int i = left; i < right; i++)
            {
                if (Regex.IsMatch(input_textbox.Text[i].ToString(), @"[\+\-]$") == true)
                {
                    //add_output(input_textbox.Text.Substring(left, i - left), depth);
                    add_output(input_textbox.Text[i].ToString(), depth);
                    //add_output(input_textbox.Text.Substring(i + 1, right - i - 1), depth);
                    return checker(left, i, depth+1) && checker(i + 1, right, depth+1);
                }
                if (input_textbox.Text[i] == '(') 
                {
                    while (input_textbox.Text[i] != ')')
                    {
                        i++;
                    }
                }
            }
            for (int i = left; i < right; i++)
            {
                if (Regex.IsMatch(input_textbox.Text[i].ToString(), @"[\*\/]$") == true)
                {
                    //add_output(input_textbox.Text.Substring(left, i - left), depth);
                    add_output(input_textbox.Text[i].ToString(), depth);
                    //add_output(input_textbox.Text.Substring(i + 1, right - i - 1), depth);
                    return checker(left, i, depth+1) && checker(i+1, right, depth+1);
                }
                if (input_textbox.Text[i] == '(')
                {
                    while (input_textbox.Text[i] != ')')
                    {
                        i++;
                    }
                }
            }
            for (int i = left; i < right; i++)
            {
                if (input_textbox.Text[i] == '(')
                {
                    for (int j = right - 1; j > i; j--)
                    {
                        if (input_textbox.Text[j] == ')')
                            return checker(left, i, depth + 1) && checker(i + 1, j, depth + 1);
                    }
                }
            }

            add_output(input_textbox.Text.Substring(left, right - left), depth);
            return true;
        }
        //a + b * c - d / e
        //a + b * ( c - d / 0.5 ) + f * 2

        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";
            input_textbox.Text = input_textbox.Text.Replace(" ", "");

            checker(0, input_textbox.Text.Length, 0);

            //●

        }
    }
}
