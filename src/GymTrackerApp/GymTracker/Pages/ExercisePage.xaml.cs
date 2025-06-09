using Microsoft.Extensions.Logging.Abstractions;

namespace GymTracker.Pages;

public partial class ExercisePage : ContentPage
{
	public ExercisePage(ExerciseViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		ExerciseLabel.Text = $"{Constants.currentWorkoutName}";
        LoadExercises();
    }

    private async void LoadExercises()
    {
        var exerciseList = await Constants.appDatabase.GetExercisesAsync(Constants.currentWorkoutID);
        double exerciseDoneSets = 0.0;
        double exerciseMaxSets = 0.0;

        ExerciseBox.Children.Clear();

        foreach (var exercise in exerciseList)
        {
            HorizontalStackLayout layout = new HorizontalStackLayout{};

            Button ExerciseButton = new Button
            {
                StyleId = $"ExerciseButton{exercise.ID}",
                Text = exercise.ExerciseName,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(16, 15),
                Margin = new Thickness(20, 20, 10, 0),
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Purple,
            };

            Label SetsLabel = new Label
            {
                StyleId = $"SetsLabel{exercise.ID}",
                Text = $"{exercise.ExerciseCurrNum} / {exercise.ExerciseMaxNum}",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(16, 15),
                Margin = new Thickness(0, 20, 0, 0),
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Purple,
            };

            Button ExercisePlusButton = new Button
            {
                StyleId = $"ExercisePlusButton{exercise.ID}",
                Text = "+",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(16, 15),
                Margin = new Thickness(5, 20, 5, 0),
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.Purple,
                FontSize = 15,
                FontAttributes = FontAttributes.Bold
            };

            ExercisePlusButton.Clicked += OnExercisePlusButtonClicked;

            ExerciseBox.Children.Add(layout);
            layout.Children.Add(ExerciseButton);
            layout.Children.Add(SetsLabel);
            layout.Children.Add(ExercisePlusButton);

            exerciseDoneSets += exercise.ExerciseCurrNum;
            exerciseMaxSets += exercise.ExerciseMaxNum;
        }

        await OverallProgressBar.ProgressTo(Math.Round(exerciseDoneSets / exerciseMaxSets, 2), 1000, Easing.Linear);
    }

    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnAddExerciseClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AddExercisePage");
    }

    private async void OnExercisePlusButtonClicked(object sender, EventArgs e)
    {
        await Constants.appDatabase.RaiseExerciseAsync(Convert.ToInt32(((Button)sender).StyleId.Replace("ExercisePlusButton", "")));
        LoadExercises();
    }

    private async void OnAddExerciseLoaded(object sender, EventArgs e)
    {
        while (true)
        {
            await AddExerciseButton.ScaleTo(1.1, 500, Easing.CubicInOut);
            await AddExerciseButton.ScaleTo(1.0, 500, Easing.CubicInOut);
        }
    }

    private async void RemoveExercise(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Exercise remove", $"Are you sure you want to remove \n{((Button)sender).Text} ?", "No", "Yes");
        if (!answer)
        {
            int id = Convert.ToInt32(((Button)sender).StyleId.Replace("ExerciseButton", ""));
            await Constants.appDatabase.RemoveExerciseAsync(id);
        }

        OnRemoveExerciseClicked(sender, e);
        LoadExercises();
    }

    private async void OnRemoveExerciseClicked(object sender, EventArgs e)
    {
        foreach (HorizontalStackLayout layout in ExerciseBox.Children)
        {
            Button button = (Button)layout.Children[0];

            if (button.TextColor == Colors.Red)
            {
                button.Text = button.Text[3..];
                button.TextColor = Colors.Blue;
                button.Clicked -= RemoveExercise;

                RemoveExerciseButton.Text = "- Remove exercise";
                RemoveExerciseButton.TextColor = Colors.White;
            }
            else
            {
                button.Text = " - " + button.Text;
                button.TextColor = Colors.Red;
                button.Clicked += RemoveExercise;

                RemoveExerciseButton.Text = "Cancel";
                RemoveExerciseButton.TextColor = Colors.Red;
            }
        }
    }
}