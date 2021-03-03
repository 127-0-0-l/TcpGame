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
        private SolidColorBrush colorButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#4d606e");
        private SolidColorBrush colorButtonEnter = (SolidColorBrush)new BrushConverter().ConvertFrom("#393e46");
        private SolidColorBrush colorButtonDisabled = (SolidColorBrush)new BrushConverter().ConvertFrom("#f8b500");

        private enum PlayerColor
        {
            Red,
            Blue,
            Undefined
        }

        private Lines[,] horizontalLines = new Lines[5, 4];
        private Lines[,] verticalLines = new Lines[4, 5];
        private Rectangle[,] squares = new Rectangle[5, 5];

        private PlayerColor playerColor = PlayerColor.Undefined;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void grdHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void CreateMap()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //создание квадратов
                    squares[i, j] = CreateRectangle(40, 40, i * 49 + 2, j * 49 + 2, colorMapItemRegular);
                    grdMap.Children.Add(squares[i, j]);
                    //по периметру
                    if (i == 0 || i == 4 || j == 0 || j == 4)
                    {
                        //угловые
                        if ((i == 0 && j == 0) ||
                            (i == 0 && j == 4) ||
                            (i == 4 && j == 0) ||
                            (i == 4 && j == 4))
                        {
                            squares[i, j].Tag = 2;
                        }
                        else
                            squares[i, j].Tag = 1;
                    }
                    //остальные
                    else
                    {
                        squares[i, j].Tag = 0;
                    }

                    //создание вертикальных линий
                    if (i != 4)
                    {
                        Lines line = new Lines(CreateRectangle(5, 44, (i + 1) * 49 - 5, j * 49, colorMapItemRegular), i, j);

                        //обработка нажатия на вертикальную линию
                        line.Rct.MouseLeftButtonDown += (s, a) =>
                        {
                            PlayerColor plColor = playerColor == PlayerColor.Red ? PlayerColor.Red : PlayerColor.Blue;

                            PaintLine(true, line.X, line.Y, plColor);
                            IncrementSquareTag(line.X, line.Y, plColor);
                            IncrementSquareTag(line.X + 1, line.Y, plColor);

                            SendMessage($"v{line.X}{line.Y}");
                            DisableMap();
                        };

                        verticalLines[i, j] = line;
                        grdMap.Children.Add(verticalLines[i, j].Rct);
                    }

                    //создание горизонтальный линий
                    if (j != 4)
                    {
                        Lines line = new Lines(CreateRectangle(44, 5, i * 49, (j + 1) * 49 - 5, colorMapItemRegular), i, j);

                        //обработка нажатия на горизонтальную линию
                        line.Rct.MouseLeftButtonDown += (s, a) =>
                        {
                            PlayerColor plColor = playerColor == PlayerColor.Red ? PlayerColor.Red : PlayerColor.Blue;

                            PaintLine(false, line.X, line.Y, plColor);
                            IncrementSquareTag(line.X, line.Y, plColor);
                            IncrementSquareTag(line.X, line.Y + 1, plColor);

                            SendMessage($"h{line.X}{line.Y}");
                            DisableMap();
                        };

                        horizontalLines[i, j] = line;
                        grdMap.Children.Add(horizontalLines[i, j].Rct);
                    }
                }
            }

            foreach (var line in grdMap.Children.OfType<Rectangle>().Where(rct => rct.Tag == null))
            {
                line.MouseEnter += (s, a) =>
                {
                    if (playerColor == PlayerColor.Red)
                        line.Fill = colorMapRedLite;
                    else
                        line.Fill = colorMapBlueLite;
                };
                line.MouseLeave += (s, a) =>
                {
                    if (line.Tag == null)
                        line.Fill = colorMapItemRegular;
                };
            }
        }

        private Rectangle CreateRectangle(int width, int height, int left, int top, SolidColorBrush color)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.Fill = color;
            rectangle.HorizontalAlignment = HorizontalAlignment.Left;
            rectangle.VerticalAlignment = VerticalAlignment.Top;
            rectangle.Margin = new Thickness(left, top, 0, 0);
            return rectangle;
        }

        private void PaintLine(bool isVertical, int x, int y, PlayerColor plColor)
        {
            if (plColor == PlayerColor.Red)
            {
                if (isVertical)
                {
                    verticalLines[x, y].Rct.Fill = colorMapRed;
                    verticalLines[x, y].Rct.IsEnabled = false;
                    verticalLines[x, y].Rct.Tag = 0;
                }
                else
                {
                    horizontalLines[x, y].Rct.Fill = colorMapRed;
                    horizontalLines[x, y].Rct.IsEnabled = false;
                    horizontalLines[x, y].Rct.Tag = 0;
                }
            }
            else
            {
                if (isVertical)
                {
                    verticalLines[x, y].Rct.Fill = colorMapBlue;
                    verticalLines[x, y].Rct.IsEnabled = false;
                    verticalLines[x, y].Rct.Tag = 0;
                }
                else
                {
                    horizontalLines[x, y].Rct.Fill = colorMapBlue;
                    horizontalLines[x, y].Rct.IsEnabled = false;
                    horizontalLines[x, y].Rct.Tag = 0;
                }
            }
        }

        private void IncrementSquareTag(int i, int j, PlayerColor plColor)
        {
            if (squares[i, j].Fill == colorMapItemRegular)
            {
                squares[i, j].Tag = (int)squares[i, j].Tag + 1;
                if ((int)squares[i, j].Tag == 4)
                {
                    if (plColor == PlayerColor.Red)
                    {
                        squares[i, j].Fill = colorMapRed;
                        tbCountRed.Text = (int.Parse(tbCountRed.Text) + 1).ToString();
                    }
                    else
                    {
                        squares[i, j].Fill = colorMapBlue;
                        tbCountBlue.Text = (int.Parse(tbCountBlue.Text) + 1).ToString();
                    }
                }
            }
        }
    }
}