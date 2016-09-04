using System.Collections.Generic;
using Siage.Services.Core.Models;
using System;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface ISolicitudPTRules
    {
        SolicitudPTModel GetSolicitudPTById(int id);
        SolicitudPTVerModel GetSolicitudPTVerById(int id);
        SolicitudPTModel SolicitudPTSave(SolicitudPTModel model);
        List<PuestoDeTrabajoModel> ActivacionSolicitud(int solicitud, List<PuestoDeTrabajoModel> puestos);
        SolicitudPTModel AutorizarSolicitud(int solicitud, bool autorizacion, string observacionRechazo);
        bool ValidarEscuela(int idEmp,out bool autorizada);
        List<UnidadAcademicaModel> GetUnidadesAcademicasByidEmpresa(int idEmp, Siage.Base.MotivoAltaPuestoTrabajoEnum motivo);
        List<TipoCargoModel> GetTipoCargosByIdEmpresa(int idEmp);
        List<SolicitudPTConsultaModel> GetByFilter(string cue, int? cueAnexo, string codigoEmpresa, int? numeroSolicitud, DateTime? fechaDesde, DateTime? fechaHasta, EstadoSolicitudCreacionPuestoDeTrabajoEnum? estado, string codigoPN);
    }
}
