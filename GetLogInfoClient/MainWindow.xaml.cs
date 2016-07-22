using GetLogInfoClient.Dialog;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GetLogInfoClient
{
    public partial class MainWindow : Window
    {
        private static MainWindowViewModel ViewModel = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModel;
        }
        private void UnIncludeFilteText_KeyUp(object sender, KeyEventArgs e)
        {
            ViewModel.UnIncludeFilteText = ((TextBox)sender).Text;
        }
        private void IncludeFilteText_KeyUp(object sender, KeyEventArgs e)
        {
            ViewModel.IncludeFilteText = ((TextBox)sender).Text;
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.SelectedItem == null)
            {
                return;
            }
            else
            {
                var dialog = new LogInfoDetailDialog(ViewModel.SelectedItem);
                dialog.ShowDialog();
            }
        }
    }
}
