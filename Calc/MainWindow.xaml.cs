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
using System.ComponentModel;


namespace Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double memoryValue;
        public string memoryOpreation;
        public bool memorised;
        public bool isEngineerMode;
        int factorialValue = 1;

        public MainWindow()
        {
            InitializeComponent();
            isEngineerMode = true;
            memoryValue = 0;
            memoryOpreation = "";
            memorised = false;
            factorialWorker.DoWork += factorialWorker_DoWork;
            factorialWorker.ProgressChanged += factorialWorker_ProgressChanged;
            factorialWorker.WorkerReportsProgress = true;
            ModeSwitcher();

        }

        BackgroundWorker factorialWorker = new BackgroundWorker();
        private void factorialWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(new StatusBarChangingDelegate(StatusBarChanging),("Factorial working..."));

            int incoming = int.Parse(e.Argument.ToString());
            int sleepingTime = 4;
            for (int i = 1; i <= sleepingTime; i++)
            {
                System.Threading.Thread.Sleep(1000);
                factorialWorker.ReportProgress((int)(i * 100 / sleepingTime));
            }
            for (int j = 2; j <= incoming; j++) // цикл начинаем с 2, т.к. нет смысла начинать с 1 
            {
                factorialValue = factorialValue * j;
            }
            string message = ($"Факториал числа {incoming} = {factorialValue}");
            MessageBox.Show(message);
            factorialValue = 1;
            this.Dispatcher.Invoke(new ProgressBarClearingDelegate(ProgressBarClear));
            this.Dispatcher.Invoke(new StatusBarChangingDelegate(StatusBarChanging), ("Factorial done!"));
        }
        private void factorialWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarChangingDelegate progressUpdate = new ProgressBarChangingDelegate(ProgressBarChanging);
            progressUpdate(e.ProgressPercentage);
        }

        public delegate void StatusBarChangingDelegate(string message);
        public void StatusBarChanging(string message)
        {
            StatusBarMessage.Content = message;
        }
        public delegate void ProgressBarChangingDelegate(int message);
        public void ProgressBarChanging(int message)
        {
            Progress.Value = message;
        }
        public delegate void ProgressBarClearingDelegate();
        public void ProgressBarClear()
        {
            Progress.Value = Progress.Minimum;
        }


        private void ModeSwitcher()
        {
            if (!isEngineerMode)
            {
                EngineerColumn.Width = new GridLength(145, GridUnitType.Pixel); ;
                isEngineerMode = true;
            }
            else if (isEngineerMode)
            {
                EngineerColumn.Width = new GridLength(0, GridUnitType.Pixel); ;
                isEngineerMode = false;
            }
            return;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void SimpleMode_Click(object sender, RoutedEventArgs e)
        {
            if (isEngineerMode)
            {
                StatusBarChangingDelegate update = new StatusBarChangingDelegate(StatusBarChanging);
                update("Switched to Simple Mode");
                ModeSwitcher();
            }
            return;
        }
        private void EngineerMode_Click(object sender, RoutedEventArgs e)
        {
            if (!isEngineerMode)
            {
                StatusBarChangingDelegate update = new StatusBarChangingDelegate(StatusBarChanging);
                update("Switched to Engineer Mode");
                ModeSwitcher();
            }
            return;
        }
        private void QuadrEquation_Click(object sender, RoutedEventArgs e)
        {
            QuadrEqWindow subWindow = new QuadrEqWindow();
            subWindow.Show();
        }

        private void NumbersGrid_Click(object sender, RoutedEventArgs e)
        {
            // обработчик работает только для кнопок
            Button btn = e.Source as Button;
            // Если не кнопка
            if (btn == null)
            { return; }
            // Для кнопок у которых есть значение (контент)
            string value;
            try
            { value = btn.Content.ToString(); }
            catch
            {
                StatusBarChangingDelegate update = new StatusBarChangingDelegate(StatusBarChanging);
                update("Error on buttons");
                return;
            }
            if (String.IsNullOrEmpty(value))
            { return; }
            //Свичи
            switch (value)
            {
                case "+/-":
                    try
                    {
                        double i = Double.Parse(CalcTextBox.Text);
                        i = (-i);
                        CalcTextBox.Text = i.ToString();
                    }
                    catch
                    {
                        CalcTextBox.Text = "Error";
                    }
                    break;
                case ",":
                    if (!CalcTextBox.Text.Contains('.'))
                    { CalcTextBox.Text += "."; }
                    break;
                // добавялем новое значение в текстовое поле и удаляем стартовый 0
                default:
                    CalcTextBox.Text = CalcTextBox.Text.TrimStart('0') + value;
                    break;
            }
        }
        private void ActivityGrid_Click(object sender, RoutedEventArgs e)
        {
            // обработчик работает только для кнопок
            Button btn = e.Source as Button;
            // Если не кнопка
            if (btn == null)
            { return; }
            // Для кнопок у которых есть значение (контент)
            string value;
            try
            { value = btn.Content.ToString(); }
            catch
            {
                StatusBarChangingDelegate update = new StatusBarChangingDelegate(StatusBarChanging);
                update("Error on buttons");
                return;
            }
            if (String.IsNullOrEmpty(value))
            { return; }
            //Свичи
            switch (value)
            {
                //Очистка последнего символа
                case "Del":
                    if (!String.IsNullOrEmpty(CalcTextBox.Text))
                    {
                        CalcTextBox.Text = CalcTextBox.Text.Substring(0, CalcTextBox.Text.Length - 1);
                    }
                    if (String.IsNullOrEmpty(CalcTextBox.Text))
                    {
                        CalcTextBox.Text += '0';
                    }
                    break;
                //Очистка поля
                case "C":
                    CalcTextBox.Text = "0";
                    memoryValue = 0;
                    memoryOpreation = "";
                    memorised = false;
                    break;
                default:
                    ActivityResultator(value);
                    break;
            }
            return;
        }
        private void EngineerGrid_Click(object sender, RoutedEventArgs e)
        {
            // обработчик работает только для кнопок
            Button btn = e.Source as Button;
            // Если не кнопка
            if (btn == null)
            { return; }
            string value;
            try
            { value = btn.Content.ToString(); }
            catch
            {
                StatusBarChangingDelegate update = new StatusBarChangingDelegate(StatusBarChanging);
                update("Error on buttons");
                return;
            }
            // Для кнопок у которых есть значение (контент)
            if (String.IsNullOrEmpty(value))
            { return; }
            
            EngineerResultator(value);
            return;
        }

        private void ActivityResultator(string operation)
        {
            if (!memorised && operation == "=")
            {
                
            }
            else if (!memorised && operation != "=")
            {
                memoryValue = double.Parse(CalcTextBox.Text);
                memoryOpreation = operation;
                memorised = true;
                CalcTextBox.Text = "0";
            }
            else if (memorised && operation == "=")
            {
                double i;
                switch (memoryOpreation)
                {
                    case "x^y":
                        i = Math.Pow(memoryValue, double.Parse(CalcTextBox.Text));
                        break;
                    case "/":
                        i = memoryValue / double.Parse(CalcTextBox.Text);
                        break;
                    case "*":
                        i = memoryValue * double.Parse(CalcTextBox.Text);
                        break;
                    case "+":
                        i = memoryValue + double.Parse(CalcTextBox.Text);
                        break;
                    case "-":
                        i = memoryValue * double.Parse(CalcTextBox.Text);
                        break;
                    default:
                        CalcTextBox.Text = "Error";
                        return;
                }
                memorised = false;
                CalcTextBox.Text = i.ToString();
                return;
            }
            else
            {
                double i;
                switch (memoryOpreation)
                {
                    case "x^y":
                        i = Math.Pow(memoryValue, double.Parse(CalcTextBox.Text));
                        memoryValue = i;
                        break;
                    case "/":
                        i = memoryValue / double.Parse(CalcTextBox.Text);
                        memoryValue = i;
                        break;
                    case "*":
                        i = memoryValue * double.Parse(CalcTextBox.Text);
                        memoryValue = i;
                        break;
                    case "+":
                        i = memoryValue + double.Parse(CalcTextBox.Text);
                        memoryValue = i;
                        break;
                    case "-":
                        i = memoryValue * double.Parse(CalcTextBox.Text);
                        memoryValue = i;
                        break;
                    default:
                        CalcTextBox.Text = "Error";
                        return;
                }
                memoryOpreation = operation;
                CalcTextBox.Text = "0";
            }
        }
        private void EngineerResultator(string operation)
        {
            if (operation == "Sqrt")
            {
                try
                {
                    CalcTextBox.Text = Math.Sqrt(Double.Parse(CalcTextBox.Text)).ToString();
                }
                catch
                {
                    CalcTextBox.Text = "Error";
                }
                return;
            }
            else if (operation == "x^2")
            {
                CalcTextBox.Text = (Double.Parse(CalcTextBox.Text) * Double.Parse(CalcTextBox.Text)).ToString();
                return;
            }
            else if (operation == "x^y" && !memorised)
            {
                memoryValue = double.Parse(CalcTextBox.Text);
                memoryOpreation = operation;
                memorised = true;
                CalcTextBox.Text = "0";
                return;
            }
            else if (operation == "x^y" && memorised)
            {
                double j;
                switch (memoryOpreation)
                {
                    case "x^y":
                        j = Math.Pow(memoryValue, double.Parse(CalcTextBox.Text));
                        memoryValue = j;
                        break;
                    case "/":
                        j = memoryValue / double.Parse(CalcTextBox.Text);
                        memoryValue = j;
                        break;
                    case "*":
                        j = memoryValue * double.Parse(CalcTextBox.Text);
                        memoryValue = j;
                        break;
                    case "+":
                        j = memoryValue + double.Parse(CalcTextBox.Text);
                        memoryValue = j;
                        break;
                    case "-":
                        j = memoryValue * double.Parse(CalcTextBox.Text);
                        memoryValue = j;
                        break;
                    default:
                        CalcTextBox.Text = "Error";
                        return;
                }
                memoryOpreation = operation;
                CalcTextBox.Text = "0";
                return;
            }
            else if(operation == "1/x")
            {
                CalcTextBox.Text = (1 / Double.Parse(CalcTextBox.Text)).ToString();
                return;
            }
            else if (operation == "3√x")
            {
                CalcTextBox.Text = Math.Pow(Double.Parse(CalcTextBox.Text),(1.0/3)).ToString();
                return;
            }
            else if (operation == "x!")
            {
                int i=0;
                try
                { i = int.Parse(CalcTextBox.Text); }
                catch
                {
                    CalcTextBox.Text = "Error";
                    StatusBarChangingDelegate update = new StatusBarChangingDelegate(StatusBarChanging);
                    update("Only round allowed");
                }
                if (factorialWorker.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    factorialWorker.RunWorkerAsync(CalcTextBox.Text);
                }
            }
            else { return; }
        }
    }
}
