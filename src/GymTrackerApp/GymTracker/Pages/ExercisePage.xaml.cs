using Microsoft.Extensions.Logging.Abstractions;

namespace GymTracker.Pages;

public partial class ExercisePage : ContentPage
{
	public ExercisePage(ExerciseViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		ExerciseLabel.Text = $"{Constants.currentWorkoutName}";
    }

    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void OnAddExerciseClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Error", $"Add Exercise", "OK");
    }

    private async void OnAddExerciseLoaded(object sender, EventArgs e)
    {
        while (true)
        {
            await AddExerciseButton.ScaleTo(1.1, 500, Easing.CubicInOut);
            await AddExerciseButton.ScaleTo(1.0, 500, Easing.CubicInOut);
        }
    }

    private async void OnRemoveExerciseClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Error", $"Remove Exercise", "OK");
    }
}