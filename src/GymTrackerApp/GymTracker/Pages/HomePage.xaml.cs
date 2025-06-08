using GymTracker.Database;
using Microsoft.Extensions.Logging.Abstractions;

namespace GymTracker.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        CreateTodayWorkouts();
    }

    private async void CreateTodayWorkouts()
    {
        var workoutlist = await Constants.appDatabase.GetWorkoutsAsync(Constants.currentUserID);

        TodayBox.Children.Clear();

        foreach (Workouts workout in workoutlist)
        {
            Button WorkoutButton = new Button
            {
                Text = workout.WorkoutName,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(16, 15),
                Margin = new Thickness(20, 20, 20, 0),
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Purple,
                BorderColor = Colors.Blue,
                BorderWidth = 3
            };

            TodayBox.Children.Add(WorkoutButton);
        }
    }

    private async void OnAddWorkoutClicked(object sender, EventArgs e)
    {
        int userID = Constants.currentUserID;

        DateTime date_now = DateTime.Now;
        string european_date = date_now.ToString("yyyy-MM-dd HH:mm:ss");

        await Constants.appDatabase.CreateWorkoutAsync(new Database.Workouts
        {
            User_ID = userID,
            WorkoutDate = date_now,
            WorkoutName = $"{date_now.DayOfWeek} Workout {european_date}"
        });
        
        var workouts = await Constants.appDatabase.GetWorkoutsAsync(userID);
        CreateTodayWorkouts();
    }

    private async void OnAddWorkoutLoaded(object sender, EventArgs e)
    {
        while (true)
        {
            await AddWorkoutButton.ScaleTo(1.1, 500, Easing.CubicInOut);
            await AddWorkoutButton.ScaleTo(1.0, 500, Easing.CubicInOut);
        }
    }
}