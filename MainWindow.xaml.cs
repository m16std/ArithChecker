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

        private bool add_output(string word, int depth)
        {
            for (int i = 0; i < depth; i++)
                output_textbox.Text += "        ";
            output_textbox.Text += word + "\n";
            return true;
        }

        private bool checker(int left, int right, int depth, bool mand, bool isfunc)
        {
            if(left == right)
            {
                if(mand)
                    add_output("Не найдено число или идентификатор соответсвующий оператору", depth);
                return !mand;
            }

            //Секция разбиения на слогаемые, множители и тп

            for (int i = left; i < right; i++)
            {
                
                if (Regex.IsMatch(input_textbox.Text[i].ToString(), @"[\+\-]$"))
                {
                    if (depth == 0)
                    {
                        add_output(input_textbox.Text[i].ToString(), depth);
                        return checker(left, i, depth + 1, false, false) && checker(i + 1, right, depth + 1, true, false);
                    }
                    else
                    if (i != 0)
                    {
                        if (Regex.IsMatch(input_textbox.Text[i-1].ToString(), @"[\(\*\/\%]$"))
                        {
                            i++;
                        }
                        else
                        {
                            add_output(input_textbox.Text[i].ToString(), depth);
                            return checker(left, i, depth + 1, true, false) && checker(i + 1, right, depth + 1, true, false);
                        }
                    }

                }
                if (input_textbox.Text[i] == '(') 
                {
                    int vloshennost = 0;
                    while (true)
                    {
                        if (input_textbox.Text[i] == '(')
                            vloshennost++;

                        if (input_textbox.Text[i] == ')')
                        {
                            vloshennost--;
                            if(vloshennost == 0)
                                break;
                        }
                           
                        if (i == right - 1 && input_textbox.Text[i] != ')')
                        {
                            add_output("Не найдена закрывающая скобка (секция сложения/вычитания)", depth);
                            return false;
                        }
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
                    int vloshennost = 0;
                    while (true)
                    {
                        if (input_textbox.Text[i] == '(')
                            vloshennost++;

                        if (input_textbox.Text[i] == ')')
                        {
                            vloshennost--;
                            if (vloshennost == 0)
                                break;
                        }

                        if (i == right - 1 && input_textbox.Text[i] != ')')
                        {
                            add_output("Не найдена закрывающая скобка (секция сложения/вычитания)", depth);
                            return false;
                        }
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
                        {
                            add_output("(", depth);
                            return checker(left, i, depth, false, true) && checker(i + 1, j, depth + 1, true, false) && add_output(")", depth);
                        }    
                    }
                    return false;
                }
                if (input_textbox.Text[i] == ')')
                {
                    add_output("Закрывающая скобка встречена первой (секция раскрытия скобок)", depth);
                    return false;
                }
            }



            //Секция проверки слогаемых на адекватность

            bool isnum;
            bool isiden;
            bool flag = true;
            int abs_left = left;
            if (Regex.IsMatch(input_textbox.Text[left].ToString(), @"[\-\+]$"))
            {
                abs_left++;
                if (abs_left == right)
                {
                    add_output("Встречен оператор + или - в значении сохранения или инвертирования знака без соответсвующего числа или выражения", depth);
                    return false;
                }
            }

            //проверка на число
            int dot_count = 0;
            for (int i = abs_left; i < right; i++)
            {
                if (!Char.IsNumber(input_textbox.Text[i]) && input_textbox.Text[i] != '.')
                {
                    flag = false; //если в слове есть что-то кроме цифр и . то беда
                }
                if (input_textbox.Text[i] == '.')
                {
                    dot_count++;
                    if (dot_count == 2)
                        flag = false;
                }

            }
            isnum = flag;

            //является ли идентификатором
            flag = true;
            if (!Char.IsLetter(input_textbox.Text[abs_left]))
            {
                flag = false;
            }
            for (int i = abs_left; i < right; i++)
            {
                if (!Char.IsLetter(input_textbox.Text[i]) && !Char.IsNumber(input_textbox.Text[i]) && input_textbox.Text[i] != '_')
                {
                    flag = false; //если в слове есть что-то кроме букв, цифр и _ то беда
                }
            }
            isiden = flag;

            if (isfunc && !isiden)
            {
                add_output("Перед скобкой стоит не выражение", depth);
                return false;
            }
            if (isnum || isiden)
            {
                add_output(input_textbox.Text.Substring(left, right - left), depth);
                return true;
            }
            add_output("Недопустимый символ в строке:" + input_textbox.Text.Substring(left, right - left), depth);
            return false;
        }

        // a + b * ( c - d / 0.5 ) + f * 2 * sin(0)
        // -(-a+b*(c+d/-0.5)+f*-2*-sin(-0)+(-k_))

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
