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

        //Идентификатор - название переменных
        //Ключевое слово - список
        //Разделитель - пробел, таб, перенос
        //Оператор - список
        //Литерал - значение переменных
        //Комментарий - отделён // или /* */

        string[] keywords = { "alignas", "alignof", "andB", "and_eqB", "asma", "auto", "bitandB", "bitorB", "bool", "break", "case", "catch", "char", "char8_tc", "char16_t", "char32_t", "class", "complB", "conceptc", "const", "const_cast", "constevalc", "constexpr", "constinitc", "continue", "co_awaitc", "co_returnc", "co_yieldc", "decltype", "default", "delete", "do", "double", "dynamic_cast", "else", "enum", "explicit", "exportc", "extern", "false", "float", "for", "friend", "goto", "if", "inline", "int", "long", "mutable", "namespace", "new", "noexcept", "notB", "not_eqB", "nullptr", "operator", "orB", "or_eqB", "private", "protected", "public", "register reinterpret_cast", "requiresc", "return", "short", "signed", "sizeof", "static", "string", "static_assert", "static_cast", "struct", "switch", "template", "this", "thread_local", "throw", "true", "try", "typedef", "typeid", "typename", "union", "unsigned", "using Декларации", "using Директива", "virtual", "void", "volatile", "wchar_t", "while", "xorB", "xor_eqB", "cout", "using", "main", "endl", "cin" };
        string[] operators = { ";", "->", "[", "]", "(", ")", "++", "--", "typeid", "const_cast", "dynamic_cast", "reinterpret_cast", "static_cast", "sizeof", "~", "!", "-", "+", "&", "*", "/", "new", "delete", ">>", "<<", ">", "<", "=>", "<=", "==", "!=", "^", "|", "||", "&&", "?:", "=", "*=", "/=", "+=", "-=", "%=", ">>=", "<<=", "&=", "|=", "^=", "throw", ",", ".", ".*", "->*", "?", "" };
        string[] names = { "Ключевые слова", "Операторы", "Идентификаторы", "Литералы", "Комментарии", "Разделители" };

        private void add_output(ref int i, ref int j, string word)
        {
            output_textbox.Text += word + " ";
            i += j - 1;
            j = 0;
        }

        private bool find_keyword(ref int i, ref int j, string[] keywords)
        {

            if (i != 0 && j < input_textbox.Text.Length - i)
            {
                char symb_after = input_textbox.Text[j + i];
                char symb_before = input_textbox.Text[i - 1];

                if (Char.IsSeparator(symb_after) == true || symb_after == '(' || symb_after == ')' || symb_after == ';')
                {
                    if (Char.IsSeparator(symb_before) == true || symb_before == '(' || symb_before == ')')
                    {
                        for (int z = 0; z < keywords.Length; z++)
                        {
                            if (string.Compare(input_textbox.Text.Substring(i, j), keywords[z]) == 0)
                            {
                                add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                                return true;
                            }
                        }
                    }
                }
            }
        return false;
        }


        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";
            int i, j, n;

            for (n = 0; n < 4; n++)
            {
                output_textbox.Text += "● " + names[n] + "\n\n";

                for (i = 0; i < input_textbox.Text.Length; i++)
                {
                    for (j = 1; j < input_textbox.Text.Length - i && j < 20; j++)
                    {

                        find_keyword(ref i, ref j, keywords);
                    }
                }
                output_textbox.Text += "\n\n";
            }

            output_textbox.Text += "● " + names[4] + "\n\n";

        }
    }
}
