using RegistroEstudiantes.AppMovil.Vistas;

namespace RegistroEstudiantes.AppMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ListarEstudiantes());
        }
    }
}
