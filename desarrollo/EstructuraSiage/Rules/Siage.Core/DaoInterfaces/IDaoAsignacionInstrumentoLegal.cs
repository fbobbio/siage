using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoAsignacionInstrumentoLegal : IDao<AsignacionInstrumentoLegal, int>
    {
        List<AsignacionInstrumentoLegal> GetByEmpresaId(int idEmpresa);

        AsignacionInstrumentoLegal GetAsignacionInstrumentoByTipoAdquisicion(int idTipoAdquisicion);

        bool ExisteAsignacion(int idTipoAdquisicion);

        void DarDeBajaAsignacionByIdTipoAdquisicion(int idTipoAdquisicion);

        List<AsignacionInstrumentoLegal> GetAsignacionInstrumentoLegalLegalByPredioId(int id);

        List<AsignacionInstrumentoLegal> GetAsignacionesInstrumentoLegalByIdDiagramacion(int idDiagramacion);
    }
}

