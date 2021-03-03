using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client
{
    public partial class MainWindow : Window
    {
        private SolidColorBrush colorMapRegular = (SolidColorBrush)new BrushConverter().ConvertFrom("#374045");
        private SolidColorBrush colorMapItemRegular = (SolidColorBrush)new BrushConverter().ConvertFrom("#475055");
        private SolidColorBrush colorMapRedLite = (SolidColorBrush)new BrushConverter().ConvertFrom("#f47c7c");
        private SolidColorBrush colorMapRed = (SolidColorBrush)new BrushConverter().ConvertFrom("#f47c7c");
        private SolidColorBrush colorMapBlueLite = (SolidColorBrush)new BrushConverter().ConvertFrom("#70a1d7");
        private SolidColorBrush colorMapBlue = (SolidColorBrush)new BrushConverter().ConvertFrom("#70a1d7");
        private SolidColorBrush colorIndicatorRed = (SolidColorBrush)new BrushConverter().ConvertFrom("#f05454");
        private SolidColorBrush colorIndicatorGreen = (SolidColorBrush)new BrushConverter().ConvertFrom("#52d681");
        private SolidColorBrush colorHeaderButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#f5f5f5");
        private SolidColorBrush colorHeaderButtonEnter = (SolidColorBrush)new BrushConverter().ConvertFrom("#f8b500");
        private SolidColorBrush colorHeaderButtonDisabled = (SolidColorBrush)new BrushConverter().ConvertFrom("#f8b500");
        private SolidColorBrush colorButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#4d606e");
        private SolidColorBrush colorButtonEnter = (SolidColorBrush)new BrushConverter().ConvertFrom("#393e46");

        private enum PlayerColor
        {
            Red,
            Blue,
            Undefined
        }

        Rectangle[,] horisontalLines;
        Rectangle[,] verticalLines;
        Rectangle[,] squares;

        private PlayerColor playerColor = PlayerColor.Undefined;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdMap.Background = colorMapItemRegular;

            // Move window.
            grdHeader.MouseDown += (s, a) =>
            {
                DragMove();
            };

            // Change color in close/minimize buttons
            // when mouse enter/leave
            foreach (Rectangle rct in grdHeader.Children.OfType<Rectangle>())
            {
                rct.MouseEnter += (s, a) =>
                {
                    rct.Fill = colorHeaderButtonEnter;
                };
                rct.MouseLeave += (s, a) =>
                {
                    rct.Fill = colorHeaderButton;
                };
            }

            // Change color in connect button
            // when mouse enter/leave
            grdConnect.MouseEnter += (s, a) =>
            {
                grdConnect.Background = colorButtonEnter;
            };
            grdConnect.MouseLeave += (s, a) =>
            {
                grdConnect.Background = colorButton;
            };

            // Create lines and squeres on map.
            CreateMap();
        }

        private void rctBtnMinimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void rctBtnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Disconnect();
            }
            catch { }
            finally
            {
                Close();
            }
        }

        private void CreateMap()
        {
            horisontalLines = new Rectangle[6, 5];
            verticalLines = new Rectangle[5, 6];
            squares = new Rectangle[5, 5];

            for (int line = 0; line < 6; line++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Rectangle rct = CreateRectangle(5, 54, 7 + column * 61, 1 + line * 61);
                    ActivateLine(rct);
                    rct.Tag = $"h{line}{column}";
                    horisontalLines[line, column] = rct;
                }
            }

            for (int line = 0; line < 5; line++)
            {
                for (int column = 0; column < 6; column++)
                {
                    Rectangle rct = CreateRectangle(54, 5, 1 + column * 61, 7 + line * 61);
                    ActivateLine(rct);
                    rct.Tag = $"v{line}{column}";
                    verticalLines[line, column] = rct;
                }
            }

            for (int line = 0; line < 5; line++)
            {
                for (int column = 0; column < 5; column++)
                {
                    Rectangle squere = CreateRectangle(54, 54, 7 + column * 61, 7 + line * 61);
                    squere.Tag = 0;
                    squares[line, column] = squere;
                }
            }
        }

        private Rectangle CreateRectangle(int h, int w, int l, int t)
        {
            Rectangle rct = new Rectangle();
            rct.Height = h;
            rct.Width = w;
            rct.HorizontalAlignment = HorizontalAlignment.Left;
            rct.VerticalAlignment = VerticalAlignment.Top;
            rct.Margin = new Thickness(l, t, 0, 0);
            rct.Fill = colorMapItemRegular;

            grdMap.Children.Add(rct);

            return rct;
        }
    }
}
