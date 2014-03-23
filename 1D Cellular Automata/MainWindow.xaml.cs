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
        IntegerUpDown numberControl;
        DrawWindow drawWindow;
        int xPointer;
        int yPointer;
        int squareSize;

        public MainWindow()
        {
            drawWindow = new DrawWindow();
            InitializeComponent();
            numberControl = new IntegerUpDown();
            numberControl.AllowSpin = true;
            numberControl.Maximum = 256;
            numberControl.Minimum = 0;
            numberControl.AllowSpin = true;
            numberControl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            numberControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            numberControl.Margin = new Thickness(45, 23, 0, 0);
            numberControl.Value = 1;
            this.mainGrid.Children.Add(numberControl);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Tick += TimerTick;
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
            while ( xPointer < (canvas.Width / squareSize) )
            {
                char[] currentCells = automaton.Cells;
                Rectangle rectangle = new Rectangle();
                rectangle.Width = rectangle.Height = squareSize;
                if (currentCells[xPointer] == '1')
                {
                    rectangle.Fill = new SolidColorBrush(Colors.Azure);
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
            int caNum = (int)numberControl.Value;
            canvas = new Canvas();
            canvas.Width = 550;
            canvas.Height = 600;
            canvas.Background = new SolidColorBrush(Colors.White);
            Canvas.SetTop(canvas, 20);
            Canvas.SetLeft(canvas, 20);
            drawWindow.Content = canvas;

            automaton = new Automaton((int)canvas.Width, caNum);
            squareSize = automaton.Size;

            drawWindow.Show();
            timer.Start();
        }
    }
}
