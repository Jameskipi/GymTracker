using GymTracker.Database;

namespace GymTracker.Pages;

public partial class AddExercisePage : ContentPage
{
	public AddExercisePage(AddExerciseViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

        ExerciseLabel.Text = $"{Constants.currentWorkoutName}";

        FillTemplatesList();
    }

    private async void FillTemplatesList()
    {
        var exerciseList = new List<string>();
        var templates = await Constants.appDatabase.GetTemplatesAsync();

        foreach (ExerciseTemplates template in templates)
        {
            exerciseList.Add(template.ExerciseName);
        }

        TemplatePicker.ItemsSource = exerciseList;
    }

    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ExercisePage");
    }

    private void OnStepperValueChanged(object sender, EventArgs e)
    {
        ExerciseSetsEntry.Text = ((Stepper)sender).Value.ToString();
    }

    private async void OnTemplateConfirmClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Error", $"Confirm template", "OK");
    }

    private async void OnExerciseConfirmClicked(object sender, EventArgs e)
    {
        await Constants.appDatabase.CreateExerciseAsync(new Database.Exercises
        {
            Workout_ID = Constants.currentWorkoutID,
            ExerciseName = ExerciseNameEntry.Text,
            ExerciseMaxNum = Convert.ToInt32(ExerciseSetsEntry.Text),
            ExerciseCurrNum = 0,
            ExerciseIsDone = 0
        });

        await Shell.Current.GoToAsync("//ExercisePage");
    }

    private async void OnTemplateCreateClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Error", $"Template create", "OK");
    }

    private async void OnTemplateRemoveClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Error", $"Template remove", "OK");
    }
}