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
using static System.Net.Mime.MediaTypeNames;

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
                output_textbox.Text += "        ";
            output_textbox.Text += word + "\n";
        }

        private bool checker(int left, int right, int depth, bool mand, bool isfunc)
        {
            if(left == right)
                return !mand;

            //Секция разбиения на слогаемые, множители и тп

            for (int i = left; i < right; i++)
            {
                if(depth == 0 && input_textbox.Text[i] == '-')
                {
                    add_output(input_textbox.Text[i].ToString(), depth);
                    return checker(left, i, depth + 1, false, false) && checker(i + 1, right, depth + 1, true, false);
                }
                if (Regex.IsMatch(input_textbox.Text[i].ToString(), @"[\+\-]$"))
                {
                    add_output(input_textbox.Text[i].ToString(), depth);
                    return checker(left, i, depth+1, true, false) && checker(i + 1, right, depth+1, true, false);
                }
                if (input_textbox.Text[i] == '(') 
                {
                    while (true)
                    {
                        if (input_textbox.Text[i] == ')')
                            break;
                        if (i == right - 1 && input_textbox.Text[i] != ')')
                            return false;
                        i++;
                    }
                }
            }
            for (int i = left; i < right; i++)
            {
                if (Regex.IsMatch(input_textbox.Text[i].ToString(), @"[\*\/\%]$"))
                {
                    add_output(input_textbox.Text[i].ToString(), depth);
                    return checker(left, i, depth+1, true, false) && checker(i+1, right, depth+1, true, false);
                }
                if (input_textbox.Text[i] == '(')
                {
                    while (true)
                    {
                        if (input_textbox.Text[i] == ')')
                            break;
                        if (i == right - 1 && input_textbox.Text[i] != ')')
                            return false;
                        i++;
                    } 
                }
            }

            //Раскрытие скобок

            for (int i = left; i < right; i++)
            {
                if (input_textbox.Text[i] == '(')
                {
                    for (int j = right - 1; j > i; j--)
                    {
                        if (input_textbox.Text[j] == ')')
                            return checker(left, i, depth, false, true) && checker(i + 1, j, depth+1, true, false);
                    }
                    return false;
                }
                if (input_textbox.Text[i] == ')')
                {
                    return false;
                }
            }

            //Секция проверки слогаемых на адекватность

            bool isnum;
            bool isiden;
            bool flag = true;

            //проверка на число
            int dot_count = 0;
            for (int i = left; i < right; i++)
            {
                if (!Char.IsNumber(input_textbox.Text[i]) && input_textbox.Text[i] != '.')
                {
                    flag = false; //если в слове есть что-то кроме цифр и . то беда
                }
                if (!Char.IsNumber(input_textbox.Text[i]) && input_textbox.Text[i] != '.')
                {
                    dot_count++;
                    if (dot_count == 2)
                        flag = false;
                }

            }
            isnum = flag;
            //является ли идентификатором
            flag = true;
            if (!Char.IsLetter(input_textbox.Text[left]))
            {
                flag = false; //первый символ обязательно буква
            }
            for (int i = left; i < right; i++)
            {
                if (!Char.IsLetter(input_textbox.Text[i]) && !Char.IsNumber(input_textbox.Text[i]) && input_textbox.Text[i] != '_')
                {
                    flag = false; //если в слове есть что-то кроме букв, цифр и _ то беда
                }
            }
            isiden = flag;

            if (isfunc && !isiden)
            {
                return false;
            }
            if (isnum || isiden)
            {
                add_output(input_textbox.Text.Substring(left, right - left), depth);
                return true;
            }
            return false;
        }

        // a + b * ( c - d / 0.5 ) + f * 2 * sin(0)

        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";
            input_textbox.Text = input_textbox.Text.Replace(" ", "");
            bool nice = checker(0, input_textbox.Text.Length, 0, true, false);
            if(nice)
                output_textbox.Text += "\n\nnice";
            else
                output_textbox.Text += "\n\nfucked up";
        }
    }
}
