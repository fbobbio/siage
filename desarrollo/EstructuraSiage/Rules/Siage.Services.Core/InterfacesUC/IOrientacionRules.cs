using System.Collections.Generic;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IOrientacionRules
    {
        //De la orientacion
        OrientacionModel OrientacionSave(OrientacionModel modelo);
        OrientacionModel OrientacionUpdate(OrientacionModel modelo);
        OrientacionModel OrientacionDelete(OrientacionModel entidad);
        List<OrientacionModel> GetByIdOrientacionNullable(int? filtroOrientacion);
        List<OrientacionModel> GetOrientacionByFiltros(int? idOrientacion, int? idSubOrientacion);
        List<OrientacionModel> GetOrientacionByNombres(string orientacion, string subOrientacion);
        OrientacionModel GetOrientacionByEspecialidad(int idEspecialidad);
        OrientacionModel GetOrientacionById(int idOrientacion);
        List<OrientacionModel> GetOrientacionAll();
        List<OrientacionModel> GetOrientacionActivas();
        List<OrientacionModel> GetOrientacionesConSuborientacion();
        bool ExisteDetalle(int idEntidad, bool esDetalle);
        //De la suborientacion
        SubOrientacionModel SubOrientacionSave(SubOrientacionModel model, int idPadre);
        SubOrientacionModel SubOrientacionUpdate(SubOrientacionModel model, int idPadre);
        SubOrientacionModel SubOrientacionDelete(SubOrientacionModel modelo);

        List<PlanEstudioModel> GetPlanesEstudioByIdOrientacion(int idOrientacion);
        List<PlanEstudioModel> GetPlanesEstudioByIdSubOrientacion(int idSubOrientacion);
    }
}
