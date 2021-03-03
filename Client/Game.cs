using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client
{
    public partial class MainWindow
    {
        private void ChangeLineColor(string input)
        {
            Dispatcher.Invoke(() =>
            {
                if (input[0] == 'h')
                {
                    if (playerColor == PlayerColor.Red)
                    {
                        horisontalLines[int.Parse(input[1].ToString()), int.Parse(input[2].ToString())].Fill = colorMapBlue;
                    }
                    if (playerColor == PlayerColor.Blue)
                    {
                        horisontalLines[int.Parse(input[1].ToString()), int.Parse(input[2].ToString())].Fill = colorMapRed;
                    }
                }
                if (input[0] == 'v')
                {
                    if (playerColor == PlayerColor.Red)
                    {
                        verticalLines[int.Parse(input[1].ToString()), int.Parse(input[2].ToString())].Fill = colorMapBlue;
                    }
                    if (playerColor == PlayerColor.Blue)
                    {
                        verticalLines[int.Parse(input[1].ToString()), int.Parse(input[2].ToString())].Fill = colorMapRed;
                    }
                }
                EnableMap();
            });
        }

        private void IncrementSquareTag(string xy, SolidColorBrush color)
        {
            int i = int.Parse(xy[1].ToString());
            int j = int.Parse(xy[2].ToString());

            squares[i, j].Tag = (int)squares[i, j].Tag + 1;
            if ((int)squares[i, j].Tag == 4)
            {
                if (color == colorMapRed)
                {
                    squares[i, j].Fill = color;
                    tbCountRed.Text = (int.Parse(tbCountRed.Text) + 1).ToString();
                }
                else
                {
                    squares[i, j].Fill = color;
                    tbCountRed.Text = (int.Parse(tbCountRed.Text) + 1).ToString();
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

        private void ActivateLine(Rectangle rct)
        {
            rct.MouseEnter += (s, a) =>
            {
                if (playerColor == PlayerColor.Blue)
                {
                    rct.Fill = colorMapBlueLite;
                }
                if (playerColor == PlayerColor.Red)
                {
                    rct.Fill = colorMapRedLite;
                }
            };

            rct.MouseLeave += (s, a) =>
            {
                rct.Fill = colorMapItemRegular;
            };

            rct.MouseDown += (s, a) =>
            {
                if (playerColor == PlayerColor.Blue)
                {
                    rct.Fill = colorMapBlue;
                }
                if (playerColor == PlayerColor.Red)
                {
                    rct.Fill = colorMapRed;
                }
                rct.IsEnabled = false;
                IncrementSquareTag(rct.Tag.ToString(), colorButton);
                DisableMap();
                SendMessage(rct.Tag.ToString());
            };
        }

        private void SendMessage(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(msg);
            ns.Write(buffer, 0, buffer.Length);
        }
    }
}
