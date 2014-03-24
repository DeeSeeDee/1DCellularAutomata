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
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace _1D_Cellular_Automata
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Canvas canvas;
        Automaton automaton;
        DispatcherTimer timer;
        IntegerUpDown caRuleNumber;
        IntegerUpDown timerDelay;
        IntegerUpDown cellSize;
        DrawWindow drawWindow;
        int xPointer;
        int yPointer;
        int squareSize;

        public MainWindow()
        {
            drawWindow = new DrawWindow();
            InitializeComponent();

            caRuleNumber = new IntegerUpDown();
            caRuleNumber.AllowSpin = true;
            caRuleNumber.Maximum = 256;
            caRuleNumber.Minimum = 0;
            caRuleNumber.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            caRuleNumber.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            caRuleNumber.Margin = new Thickness(95, 25, 0, 0);
            caRuleNumber.Value = 1;

            timerDelay = new IntegerUpDown();
            timerDelay.AllowSpin = true;
            timerDelay.Maximum = 1000;
            timerDelay.Minimum = 1;
            timerDelay.Value = 100;
            timerDelay.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            timerDelay.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            timerDelay.Margin = new Thickness(95, 55, 0, 0);

            cellSize = new IntegerUpDown();
            cellSize.AllowSpin = true;
            cellSize.Maximum = 20;
            cellSize.Minimum = 1;
            cellSize.Value = 10;
            cellSize.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            cellSize.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            cellSize.Margin = new Thickness(95, 80, 0, 0);

            this.mainGrid.Children.Add(caRuleNumber);
            this.mainGrid.Children.Add(timerDelay);
            this.mainGrid.Children.Add(cellSize);
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            drawWindow.Closed += drawWindow_Closed;
        }

        void drawWindow_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        void TimerTick(object sender, EventArgs e)
        {
            timer.Stop();
            if (CheckMaxHeight())
            {
                return;
            }
            DrawRow();
            timer.Start();
        }

        private bool CheckMaxHeight()
        {
            if (this.yPointer >= this.canvas.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DrawRow()
        {
            xPointer = 0;
            while (xPointer < (canvas.Width / squareSize))
            {
                char[] currentCells = automaton.Cells;
                Rectangle rectangle = new Rectangle();
                rectangle.Width = rectangle.Height = squareSize;
                if (currentCells[xPointer] == '1')
                {
                    rectangle.Fill = new SolidColorBrush(Colors.DimGray);
                }
                else
                {
                    rectangle.Fill = new SolidColorBrush(Colors.Peru);
                }
                Canvas.SetLeft(rectangle, xPointer * squareSize);
                Canvas.SetTop(rectangle, yPointer);
                canvas.Children.Add(rectangle);
                xPointer++;
            }
            yPointer += squareSize;
            automaton.Generate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.yPointer = 0;
            drawWindow.Content = null;
            timer.Interval = new TimeSpan(0, 0, 0, 0, (int)timerDelay.Value);
            int caNum = (int)caRuleNumber.Value;
            canvas = new Canvas();
            canvas.Width = 550;
            canvas.Height = 600;
            canvas.Background = new SolidColorBrush(Colors.White);
            Canvas.SetTop(canvas, 20);
            Canvas.SetLeft(canvas, 20);
            drawWindow.Content = canvas;

            automaton = new Automaton((int)canvas.Width, caNum, (int)cellSize.Value);
            squareSize = automaton.Size;

            drawWindow.Show();
            timer.Start();
        }
    }
}
