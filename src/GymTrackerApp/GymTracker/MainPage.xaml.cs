using System.Threading.Tasks;

namespace GymTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task Submit()
        {
            await Shell.Current.GoToAsync("//HomePage");
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            await Submit();
        }
    }
}
