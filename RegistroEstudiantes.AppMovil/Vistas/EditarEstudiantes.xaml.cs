using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class EditarEstudiantes : ContentPage
{
    private FirebaseClient client = new FirebaseClient("https://registroestudiantes-7ec8c-default-rtdb.firebaseio.com/");
    private Estudiante estudianteActual;
    public List<Curso> Cursos { get; set; } = new();

    public EditarEstudiantes(Estudiante estudiante)
    {
        InitializeComponent();

        estudianteActual = estudiante;

        // Cargar datos del estudiante seleccionado
        NombreEntry.Text = estudiante.Nombre;
        ApellidoEntry.Text = estudiante.Apellido;
        CorreoEntry.Text = estudiante.CorreoElectronico;
        EdadEntry.Text = estudiante.Edad.ToString();

        // Cargar lista de cursos
        CargarCursos();
    }

    private async void CargarCursos()
    {
        try
        {
            var cursosEnFirebase = await client
                .Child("Cursos")
                .OnceAsync<Curso>();

            Cursos = cursosEnFirebase.Select(x => x.Object).ToList();
            CursoPicker.ItemsSource = Cursos;

            // Seleccionar el curso actual del estudiante
            CursoPicker.SelectedItem = Cursos.FirstOrDefault(c => c.Nombre == estudianteActual.Curso.Nombre);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los cursos: {ex.Message}", "OK");
        }
    }

    private async void OnGuardarButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Actualizar los datos del estudiante
            estudianteActual.Nombre = NombreEntry.Text;
            estudianteActual.Apellido = ApellidoEntry.Text;
            estudianteActual.CorreoElectronico = CorreoEntry.Text;
            estudianteActual.Edad = int.Parse(EdadEntry.Text);

            // Actualizar curso seleccionado
            estudianteActual.Curso = CursoPicker.SelectedItem as Curso;

            // Buscar el estudiante en Firebase
            var estudianteEnFirebase = (await client
                .Child("Estudiantes")
                .OnceAsync<Estudiante>())
                .FirstOrDefault(x => x.Object.NombreCompleto == estudianteActual.NombreCompleto);

            if (estudianteEnFirebase != null)
            {
                // Actualizar registro en Firebase
                await client
                    .Child("Estudiantes")
                    .Child(estudianteEnFirebase.Key)
                    .PutAsync(estudianteActual);

                await DisplayAlert("Éxito", "El estudiante ha sido actualizado correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se encontró el estudiante en la base de datos.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}
