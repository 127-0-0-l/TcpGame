using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client
{
    public partial class MainWindow
    {
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

        private void EnableMap()
        {
            Dispatcher.Invoke(() =>
            {
                grdMap.IsEnabled = true;
                grdMap.Background = colorMapRegular;
            });
        }

        private void DisableMap()
        {
            Dispatcher.Invoke(() =>
            {
                grdMap.IsEnabled = false;
                grdMap.Background = colorMapItemRegular;
            });
        }

        private void SendMessage(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(msg);
            ns.Write(buffer, 0, buffer.Length);
        }
    }
}
