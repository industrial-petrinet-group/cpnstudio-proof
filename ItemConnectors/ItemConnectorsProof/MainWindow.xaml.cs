using System.Windows;

namespace ItemConnectorsProof
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            var orangeEllipse = this.FindResource("OrangeEllipse");
            var blueEllipse = this.FindResource("BlueEllipse");

            var item1 = new DesignerItem();
            item1.Content = orangeEllipse;
            DesignerCanvas.AddItem(item1, new Point(50,50));

            var item2 = new DesignerItem();
            item2.Content = blueEllipse;
            DesignerCanvas.AddItem(item2, new Point(100, 100));


        }
    }
}
