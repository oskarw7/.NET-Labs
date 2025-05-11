using System.Windows;

namespace Lab2.View
{
    public partial class VersionWindow : Window
    {
        public VersionWindow()
        {
            InitializeComponent();
            VersionInfo.Text = "Version 1.0.2, May 2025";
        }
    }
}
