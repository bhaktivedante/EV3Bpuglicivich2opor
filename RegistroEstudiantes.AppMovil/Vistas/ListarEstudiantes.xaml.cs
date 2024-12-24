using Firebase.Database;
using RegistroEstudiantes.Modelos.Modelos;
using System.Collections.ObjectModel;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class ListarEstudiantes : ContentPage
{
    private FirebaseClient client = new FirebaseClient("https://registroestudiantes-7ec8c-default-rtdb.firebaseio.com/");
    public ObservableCollection<Estudiante> Lista { get; set; } = new ObservableCollection<Estudiante>();

    public ListarEstudiantes()
    {
        InitializeComponent();
        BindingContext = this;
        CargarLista();
    }

    private void CargarLista()
    {
        client.Child("Estudiantes").AsObservable<Estudiante>().Subscribe(estudiante =>
        {
            if (estudiante?.Object != null)
            {
                estudiante.Object.FirebaseKey = estudiante.Key; // Guardar la clave única
                if (!Lista.Any(e => e.FirebaseKey == estudiante.Object.FirebaseKey))
                {
                    Lista.Add(estudiante.Object);
                }
            }
        });
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text?.ToUpper() ?? string.Empty;

        if (!string.IsNullOrEmpty(filtro))
        {
            listaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToUpper().Contains(filtro));
        }
        else
        {
            listaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoEstudianteBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearEstudiante());
    }

    private async void EditarButton_Clicked(object sender, EventArgs e)
    {
        // Obtener el estudiante desde el parámetro del botón
        if (sender is Button button && button.CommandParameter is Estudiante estudiante)
        {
            await Navigation.PushAsync(new EditarEstudiantes(estudiante));
        }
    }
}
