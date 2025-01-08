namespace People;
using People.ViewModels;
using People.Models;


public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    private async void OnPersonTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is not Person selectedPerson)
            return;

        bool confirm = await DisplayAlert("Delete Person",
            $"Are you sure you want to delete {selectedPerson.Name}?",
            "Yes", "No");

        if (confirm)
        {
            var viewModel = BindingContext as MainPageViewModel;
            await viewModel.DeletePerson(selectedPerson);
        }
    }
}
