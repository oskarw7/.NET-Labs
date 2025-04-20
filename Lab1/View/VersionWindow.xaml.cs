using System.Windows;

namespace Lab1.View
{
    public partial class VersionWindow : Window
    {
        public VersionWindow()
        {
            InitializeComponent();
            VersionInfo.Text = "Version 1.0.1, May 2025";
        }
    }
}
