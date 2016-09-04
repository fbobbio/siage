using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoSeccion : IDao<Seccion, int>
    {
        List<Seccion> GetSeccionesByEscuelaPlan(int filtroEscuelaPlan);
        List<Seccion> GetSeccionesByIdEscuela(int idEscuela, string nombre);
        List<Seccion> GetByFiltros(string nombre);
      //  List<Seccion> GetSeccionByEscuelaLogueada(int escuelaId);
        Seccion GetSeccionVigente(string nombre);
        List<DetalleSeccion> GetSecccionByIdEscuela(int idEscuela, string nombre);
        List<Seccion> GetSeccionesNoImplementadasEnUnidadesAcademicasByFiltros(int idEscuela);
    }
    
}