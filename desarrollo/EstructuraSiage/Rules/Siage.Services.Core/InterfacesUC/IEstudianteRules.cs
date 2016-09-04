using System.Collections.Generic;
using Siage.Services.Core.Models;
using System;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IEstudianteRules
    {
        List<EstudianteModel> GetEstudianteByFiltros(string filtroSexo ,string filtroDni ,string filtroNombre , string filtroApellido ,int idEscuela);
        EstudianteModel EstudianteSave(EstudianteModel modelo);        
        EstudianteModel EstudianteUpdate(EstudianteModel modelo);
        EstudianteModel GetEstudianteById(int id);
        List<EstudianteModel> GetEstudianteByFiltros(string filtroSexo, string filtroDni, string filtroNombre, string filtroApellido);
        List<ComunicacionModel> GetComunicacionesByIdPersona(string idPersona, string tablaOrigen);
        void EstudianteDelete(EstudianteModel modelo);
        int CalcularEdad(DateTime? idEstudiante);
    }
}
