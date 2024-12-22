using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Modelos.Modelos
{
    public class Estudiante
    {
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }
        public string? CorreoElectronico { get; set; }
        public int Edad { get; set; }

        public required Curso Curso { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";

    }
}
