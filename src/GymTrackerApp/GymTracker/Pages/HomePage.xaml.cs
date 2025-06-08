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

        foreach (Workouts workout in workoutlist.AsEnumerable().Reverse())
        {
            if (workout.WorkoutDate.Date != DateTime.Now.Date)
            {
                continue;
            }

            Button WorkoutButton = new Button
            {
                StyleId = $"WorkoutButton{workout.ID}",
                Text = workout.WorkoutName,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(16, 15),
                Margin = new Thickness(20, 20, 20, 0),
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Purple,
                BorderColor = Colors.Blue,
                BorderWidth = 3,
            };

            WorkoutButton.Clicked += OnWorkoutButtonClicked;
            TodayBox.Children.Add(WorkoutButton);
        }
    }

    private async void CreateHistoryWorkouts(object sender, EventArgs e)
    {
        var workoutlist = await Constants.appDatabase.GetWorkoutsAsync(Constants.currentUserID);

        HistoryBox.Children.Clear();

        foreach (Workouts workout in workoutlist.AsEnumerable().Reverse())
        {
            if (workout.WorkoutDate.Date == DateTime.Now.Date)
            {
                continue;
            }

            Button WorkoutButton = new Button
            {
                StyleId = $"WorkoutButton{workout.ID}",
                Text = workout.WorkoutName,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(16, 15),
                Margin = new Thickness(20, 20, 20, 0),
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Purple,
                BorderColor = Colors.Blue,
                BorderWidth = 3,
            };

            WorkoutButton.Clicked += OnWorkoutButtonClicked;
            HistoryBox.Children.Add(WorkoutButton);
        }
    }

    private async void OnWorkoutButtonClicked(object sender, EventArgs e)
    {
        int workoutid = Convert.ToInt32(((Button)sender).StyleId.Replace("WorkoutButton", string.Empty));

        Constants.currentWorkoutID = workoutid;
        Constants.currentWorkoutName = ((Button)sender).Text;

        await Shell.Current.GoToAsync("//ExercisePage");
    }

    private async void OnAddWorkoutClicked(object sender, EventArgs e)
    {
        int userID = Constants.currentUserID;

        DateTime date_now = DateTime.Now;
        string european_date = date_now.ToString("yyyy-MM-dd HH:mm");

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

    private async void RemoveWorkout(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(((Button)sender).StyleId.Replace("WorkoutButton", ""));
        await Constants.appDatabase.RemoveWorkoutAsync(id);

        OnRemoveWorkoutClicked(sender, e);
        CreateTodayWorkouts();
    }

    private async void OnRemoveWorkoutClicked(object sender, EventArgs e)
    {
        foreach (Button button in TodayBox.Children)
        {
            if (button.TextColor == Colors.Red)
            {
                button.Text = button.Text[3..];
                button.TextColor = Colors.Blue;
                button.Clicked -= RemoveWorkout;
                button.Clicked += OnWorkoutButtonClicked;

                RemoveWorkoutButton.Text = "- Remove workout";
                RemoveWorkoutButton.TextColor = Colors.White;
            }
            else
            {
                button.Text = " - " + button.Text;
                button.TextColor = Colors.Red;
                button.Clicked -= OnWorkoutButtonClicked;
                button.Clicked += RemoveWorkout;

                RemoveWorkoutButton.Text = "Cancel";
                RemoveWorkoutButton.TextColor = Colors.Red;
            }
        }
    }
}