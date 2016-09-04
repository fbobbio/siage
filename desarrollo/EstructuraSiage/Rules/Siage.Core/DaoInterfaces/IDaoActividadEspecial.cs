using System;
using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Core.Domain;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Base.Dto;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoActividadEspecial:IDao<ActividadEspecial, int>
  {

        List<ActividadEspecial> GetActividadEspecialByFiltro(DateTime? fechaDesde, DateTime? fechaHasta, string apellido, string nombre, TipoActividadEspecialEnum? tipoActividad, bool bajas, int idEscuela);
        List<DtoAgenteCargo> DocentesAsignadosYNoAsignadosByIdEscuela(int idEscuela, int idActividad);
        List<DtoAgenteCargo> DocentesAsignadosByActividad(int idEscuela, int idActividad);
        List<DocenteActividadEspecial> GetDocenteActividadEspecialByFiltro(int idActividad);
        string[] GetIdsDocentesAsignadosByIdActividad(int idActividad);
        List<DocenteActividadEspecial> GetDocenteActividadEspecialByFiltro(int idActividad,int idDocente);
        bool ExisteDocenteActividadEspecialByFiltro(int idActividad);
        List<Siage.Base.Dto.DtoActividadEspecial> GetActividadesEspecialesActivas(int idAgente, int idEscuela, DateTime? fechaDesde, DateTime? fechaHasta);

  }
}
