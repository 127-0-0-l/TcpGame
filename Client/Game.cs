using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Client
{
    public partial class MainWindow
    {
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
