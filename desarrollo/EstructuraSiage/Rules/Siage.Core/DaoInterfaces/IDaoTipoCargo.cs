using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoTipoCargo : IDao<TipoCargo, int>
    {

        List<TipoCargo> GetByFiltros();
        List<TipoCargo> GetTipoCargosByIdEmpresa(int idEmp);
        List<TipoCargo> GetByFiltrosTipoCargo(int? codigoPn, string nombTipoCargo, string agrupamiento, string nivelCargo, RolCargoEnum? idRolCargo, EstadoTipoCargoEnum? estado);
        List<TipoCargo> GetVerificarExistenciaTipoCargo(string nombreCargo,int nivelCargo,int agrupamiento);
        List<TipoCargo> GetByFiltroTipoCargoCodigoPn(int codigoPn);
        bool VerificarTipoCargoEnSolicitudCreacion(int idTipoCargo);
        bool VerificarTipoCargoEnEstructuraEmpresa(int idTipoCargo);
        bool VerificarBajaEnPuestoDeTrabajo(int idTipoCargo);
        bool VerificarTipoCargoEnSolicitudAmpliacion(int idTipoCargo);
    }
}

