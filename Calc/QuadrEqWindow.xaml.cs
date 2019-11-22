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
using System.Windows.Shapes;
using System.ComponentModel;

namespace Calc
{
    /// <summary>
    /// Interaction logic for QuadrEqWindow.xaml
    /// </summary>
    public partial class QuadrEqWindow : Window
    {
        public QuadrEqWindow()
        {
            InitializeComponent();
        }

        public delegate void ResultChangingDelegate(string message);
        public void StatusBarChanging(string message)
        {
            ResultLabel.Content = message;
        }

        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            double a = 0;
            double b = 0;
            double c = 0;

            try
            {
                a = Double.Parse(aTextBox.Text);
                b = Double.Parse(bTextBox.Text);
                c = Double.Parse(cTextBox.Text);
            }
            catch
            {
                int zero = 0;
                ResultLabel.Content = "Error in your input";
                aTextBox.Text = zero.ToString();
                bTextBox.Text = zero.ToString();
                cTextBox.Text = zero.ToString();
            }
            CalculateQuEq(a, b, c);
            return;
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CalculateQuEq(double a, double b, double c)
        {
            double D;
            double x1;
            double x2;
            ResultChangingDelegate resultUpdate = new ResultChangingDelegate(StatusBarChanging);
            string resultStr =" ";

            if (a == 0 && b != 0)
            {
                x1 = (-c / b);
                resultStr = ($"Result: x = {x1}");
                resultUpdate(resultStr);
            }
            else if (b == 0 && a != 0)
            {
                x1 = Math.Sqrt((-c/a));
                resultStr = ($"Result: x = {x1}");
                resultUpdate(resultStr);
            }
            else
            {
                D = Math.Pow(b, 2) - 4 * a * c;
                if (D >= 0)
                {
                    x1 = (-b + Math.Sqrt(D)) / (2 * a);
                    x2 = (-b - Math.Sqrt(D)) / (2 * a);
                    resultStr = ($"Result: x1 = {x1} , x2 = {x2}");
                    resultUpdate(resultStr);
                    return;
                }
                else
                {
                    resultStr = ("No real roots here =(");
                    resultUpdate(resultStr);
                    return;
                }
            }
        }
    }

    class Reshenie
    {
        private double a;
        private double b;
        private double c;
        private double D;
        private double x1;
        private double x2;
        public Reshenie(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        public void raschet()
        {
            D = Math.Pow(b, 2) - 4 * a * c;
            if (D > 0 || D == 0)
            {
                x1 = (-b + Math.Sqrt(D)) / (2 * a);
                x2 = (-b - Math.Sqrt(D)) / (2 * a);
                Console.WriteLine("x1= {0}\n x2= {1}", x1, x2);
                Console.ReadKey();

            }


            else
            {
                Console.WriteLine("Действительных корней нет");
                Console.ReadKey();
            }

        }


    }
}

