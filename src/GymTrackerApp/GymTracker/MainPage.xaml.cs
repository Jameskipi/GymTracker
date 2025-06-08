using GymTracker.Database;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GymTracker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task Login()
        {
            var users = await Constants.appDatabase.GetUsersAsync();
            var user = users.FirstOrDefault(user => user.Username == LoginEntry.Text.Trim());

            if (user != null && user.Password == PasswordEntry.Text.Trim())
            {
                Constants.currentUserID = user.ID;
                await Shell.Current.GoToAsync("//HomePage");
            }
            else if (user != null && user.Password != PasswordEntry.Text.Trim())
            {
                await DisplayAlert("Error", $"Incorrect password for user {LoginEntry.Text}", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"User {LoginEntry.Text} not found", "OK");
            }

            LoginEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Username and Password cannot be empty", "OK");
                return;
            }

            await Login();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Username and Password cannot be empty", "OK");
                return;
            }
            else if (await Constants.appDatabase.GetUserByLoginAsync(LoginEntry.Text) != null)
            {
                await DisplayAlert("Error", $"User {LoginEntry.Text} already exists", "OK");
                return;
            }

            var users = await Constants.appDatabase.GetUsersAsync();

            await Constants.appDatabase.CreateUserAsync(new Database.Users
            {
                ID = users.Count,
                Username = LoginEntry.Text.Trim(),
                Password = PasswordEntry.Text.Trim()
            });

            await DisplayAlert("", $"User {LoginEntry.Text} successfully created", "OK");

            LoginEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }
    }
}
