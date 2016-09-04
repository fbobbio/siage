using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoHorarioPuestoDeTrabajo : IDao<HorarioPuestoDeTrabajo, int>
    {
        bool ValidarSumaDeHorasDePuestoDeTrabajo(int idPuestoTrabajo, int sumaCantidadHoras);

        List<PuestoDeTrabajo> GetPuestosDeTrabajoByFiltros(string filtroCodigoEmpresa, string filtroNombreEmpresa,
                                                           string filtroCue, int? filtroCueAnexo, string filtroNombreTipoCargo);

        List<string[]> HorarioByPuestoTrabajoUnidadAcademica(int idPuestoTrabajo);
        Agente GetDatosAgenteByPuestoDeTrabajo(int idPuestoDeTrabajo);
    }
}

