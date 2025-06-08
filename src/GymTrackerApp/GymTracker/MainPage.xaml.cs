using System.Threading.Tasks;

namespace GymTracker
{
    public partial class MainPage : ContentPage
    {
        Database.UsersDatabase usersDatabase = new Database.UsersDatabase();

        public MainPage()
        {
            InitializeComponent(); 
        }

        private async Task Login()
        {
            var users = await usersDatabase.GetUsersAsync();

            if (users.Any(user => user.Username == LoginEntry.Text.Trim()))
            {
                await Shell.Current.GoToAsync("//HomePage");
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
            else if (await usersDatabase.GetUserByLoginAsync(LoginEntry.Text) != null)
            {
                await DisplayAlert("Error", $"User {LoginEntry.Text} already exists", "OK");
                return;
            }

            await usersDatabase.CreateUserAsync(new Database.Users
                {
                    Username = LoginEntry.Text.Trim(),
                    Password = PasswordEntry.Text.Trim()
                });

            LoginEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }
    }
}
