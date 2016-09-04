using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IAccidenteLaboralRules
    {
        List<AccidenteLaboralConsultaModel> GetAccidenteLaboralByFiltros(DateTime? filtroFechaDesde, DateTime? filtroFechaHasta, string filtroApellido,
                                            string filtroNombre, TipoSiniestroEnum? filtroTipoSiniestro, DateTime? filtroFechaSiniestro,
                                            DateTime? filtroFechaAnulacion, int? idAgente);

        List<DtoEmpresaExternaConsultaModel> GetPrestadoresMedicos();

        AccidenteLaboralModel AccidenteLaboralSave(AccidenteLaboralModel accidenteModel);
        AccidenteLaboralModel AccidenteLaboralDelete(AccidenteLaboralModel accidenteModel);
        AccidenteLaboralModel AccidenteLaboralUpdate(AccidenteLaboralModel accidenteModel);

        DatosDniAgenteModel GetDatosAgenteByIdAgente(int idAgente);

        AccidenteLaboralModel GetAccidenteLaboralById(int idAccidenteLaboral);

    }
}
