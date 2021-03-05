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
        private SolidColorBrush colorMapRed = (SolidColorBrush)new BrushConverter().ConvertFrom("#f23557");
        private SolidColorBrush colorMapBlueLite = (SolidColorBrush)new BrushConverter().ConvertFrom("#70a1d7");
        private SolidColorBrush colorMapBlue = (SolidColorBrush)new BrushConverter().ConvertFrom("#22b2da");
        private SolidColorBrush colorIndicatorGreen = (SolidColorBrush)new BrushConverter().ConvertFrom("#52d681");
        private SolidColorBrush colorHeaderButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#f5f5f5");
        private SolidColorBrush colorHeaderButtonEnter = (SolidColorBrush)new BrushConverter().ConvertFrom("#f8b500");
        private SolidColorBrush colorButton = (SolidColorBrush)new BrushConverter().ConvertFrom("#4d606e");
        private SolidColorBrush colorButtonEnter = (SolidColorBrush)new BrushConverter().ConvertFrom("#393e46");

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
    }
}