using Siage.Core.Domain;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoCarrera : IDao<Carrera, int>
    {
        List<Carrera> GetAllByEscuela(int idEscuela);
        List<Carrera> GetByFiltros(string filtroNombre, EstadoCarreraEnum? filtroEstado);
        List<Carrera> GetByFiltros(int? id, string filtroNombre,  EstadoCarreraEnum? filtroEstado);
        bool ExisteNombreCarrera(string nombre, int? idCarrera);
        List<Carrera> CarrerasVigentesPorEscuela(int escuelaId);
        List<Carrera> CarrerasVigentesPorDireccionDeNivel(int idNivel);
        List<Carrera> GetByEstudianteMatriculado(int idEstudiante);
        List<Carrera> GetCarrerasVigentes();
    }
}