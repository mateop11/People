using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using People.Models;

namespace People.ViewModels;

public class MainPageViewModel : INotifyPropertyChanged
{
    private string _statusMessage;

    public ObservableCollection<Person> People { get; set; }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddPersonCommand { get; }
    public ICommand GetPeopleCommand { get; }

    public MainPageViewModel()
    {
        People = new ObservableCollection<Person>();
        AddPersonCommand = new Command(async () => await AddNewPerson());
        GetPeopleCommand = new Command(async () => await LoadPeople());
    }

    public async Task AddNewPerson(string name = "Default Name")
    {
        try
        {
            await App.PersonRepo.AddNewPerson(name);
            StatusMessage = $"Mateo Pillajo acaba de agregar un nuevo registro: {name}.";
            await LoadPeople();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error al agregar: {ex.Message}";
        }
    }

    public async Task DeletePerson(Person person)
    {
        try
        {
            await App.PersonRepo.DeletePerson(person.Id);
            StatusMessage = $"Mateo Pillajo acaba de eliminar un registro.";
            await LoadPeople();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error al eliminar: {ex.Message}";
        }
    }

    public async Task LoadPeople()
    {
        try
        {
            var people = await App.PersonRepo.GetAllPeople();
            People.Clear();

            foreach (var person in people)
            {
                People.Add(person);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error al cargar registros: {ex.Message}";
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
