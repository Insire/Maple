using System.Windows;
using System.Windows.Data;
using InsireBot.ViewModel;

namespace InsireBot
{
    /// <summary>
    /// Interaction logic for NewMediaItemDialog.xaml
    /// </summary>
    public partial class NewMediaItemDialog : Window
    {
        public NewMediaItemDialog()
        {
            InitializeComponent();

            var viewModel = DataContext as NewMediaItemsViewModel;
            viewModel.CloseAction = () => { Close(); };
        }
    }
}
