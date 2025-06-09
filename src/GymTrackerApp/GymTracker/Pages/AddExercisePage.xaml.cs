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
        var templates = await Constants.appDatabase.GetTemplatesAsync(Constants.currentUserID);

        foreach (ExerciseTemplates template in templates)
        {
            exerciseList.Add($"{template.ExerciseName} - {template.ExerciseMaxNum} sets");
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
        if (TemplatePicker.SelectedIndex == -1)
        {
            return;
        }

        var pickerValue = (string)TemplatePicker.ItemsSource[TemplatePicker.SelectedIndex];
        string name = pickerValue.Split('-')[0].Trim();
        int sets = Convert.ToInt32(pickerValue.Split('-')[1].Replace("sets", string.Empty).Trim());

        await Constants.appDatabase.CreateExerciseAsync(new Database.Exercises
        {
            Workout_ID = Constants.currentWorkoutID,
            ExerciseName = name,
            ExerciseMaxNum = sets,
            ExerciseCurrNum = 0,
            ExerciseIsDone = 0
        });

        await Shell.Current.GoToAsync("//ExercisePage");
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
        var alreadyExisting = await Constants.appDatabase.GetSpecificTemplateAsync(ExerciseNameEntry.Text, Convert.ToInt32(ExerciseSetsEntry.Text), Constants.currentUserID);
        if (alreadyExisting.Any())
        {
            await DisplayAlert("Error", $"Template {ExerciseNameEntry.Text} - {Convert.ToInt32(ExerciseSetsEntry.Text)} sets already exists", "OK");
            return;
        }

        bool answear = await DisplayAlert("Template creation", $"Do you want to create the template: \n{ExerciseNameEntry.Text} - {ExerciseSetsEntry.Text} sets ?", "No", "Yes");
        if (answear)
        {
            return;
        }

        await Constants.appDatabase.CreateExerciseAsync(new Database.Exercises
        {
            Workout_ID = Constants.currentWorkoutID,
            ExerciseName = ExerciseNameEntry.Text,
            ExerciseMaxNum = Convert.ToInt32(ExerciseSetsEntry.Text),
            ExerciseCurrNum = 0,
            ExerciseIsDone = 0
        });

        await Constants.appDatabase.CreateTemplateAsync(new Database.ExerciseTemplates
        {
            User_ID = Constants.currentUserID,
            ExerciseName = ExerciseNameEntry.Text,
            ExerciseMaxNum = Convert.ToInt32(ExerciseSetsEntry.Text),
        });

        await Shell.Current.GoToAsync("//ExercisePage");
    }

    private async void OnTemplateRemoveClicked(object sender, EventArgs e)
    {
        if (TemplatePicker.SelectedIndex == -1)
        {
            return;
        }

        var pickerValue = (string)TemplatePicker.ItemsSource[TemplatePicker.SelectedIndex];
        string name = pickerValue.Split('-')[0].Trim();
        int sets = Convert.ToInt32(pickerValue.Split('-')[1].Replace("sets", string.Empty).Trim());

        bool answear = await DisplayAlert("Template remove", $"Are you sure you want to remove the template: \n{name} - {sets} sets ?", "No", "Yes");
        if (answear)
        {
            return;
        }

        var template = await Constants.appDatabase.GetSpecificTemplateAsync(name, sets, Constants.currentUserID);
        await Constants.appDatabase.RemoveTemplateAsync(template.First());

        FillTemplatesList();
        TemplatePicker.SelectedIndex = -1;
    }
}