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
        try
        {
            // Suscribirse a cambios en Firebase
            client.Child("Estudiantes").AsObservable<Estudiante>().Subscribe(estudiante =>
            {
                if (estudiante?.Object != null)
                {
                    estudiante.Object.FirebaseKey = estudiante.Key; // Guardar la clave única

                    // Verificar si el estudiante ya existe en la lista
                    var estudianteExistente = Lista.FirstOrDefault(e => e.FirebaseKey == estudiante.Object.FirebaseKey);
                    if (estudianteExistente != null)
                    {
                        // Actualizar estudiante existente
                        var index = Lista.IndexOf(estudianteExistente);
                        Lista[index] = estudiante.Object;
                    }
                    else
                    {
                        // Agregar nuevo estudiante
                        Lista.Add(estudiante.Object);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"No se pudo cargar la lista de estudiantes: {ex.Message}", "OK");
        }
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al filtrar los estudiantes: {ex.Message}", "OK");
        }
    }

    private async void NuevoEstudianteBoton_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new CrearEstudiante());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al abrir la página de creación: {ex.Message}", "OK");
        }
    }

    private async void EditarButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Obtener el estudiante desde el parámetro del botón
            if (sender is Button button && button.CommandParameter is Estudiante estudiante)
            {
                await Navigation.PushAsync(new EditarEstudiantes(estudiante));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al abrir la página de edición: {ex.Message}", "OK");
        }
    }
}
