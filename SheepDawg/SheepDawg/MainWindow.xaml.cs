
namespace SheepDawg
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var host = new VisualContainer();
            this.Content = host;
        }
    }
}
