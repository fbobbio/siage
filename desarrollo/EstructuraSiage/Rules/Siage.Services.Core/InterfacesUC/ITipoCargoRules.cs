using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
  public interface ITipoCargoRules
    {

      TipoCargoModel GetTipoCargoById(int id);
      List<TipoCargoModel> GetTipoCargo();
      TipoCargoModel TipoCargoSave(TipoCargoModel modelo);
      List<TipoCargoModel> FiltroBusquedaTipoCargo(int? codigoPn, string nombTipoCargo, string agrupamiento, string nivelCargo, RolCargoEnum? idRolCargo, EstadoTipoCargoEnum? estado);
      List<AgrupamientoCargoModel> GetAgrupamientosAll();
      List<NivelCargoModel> GetNivelCargoAll();
      bool VerificarExistenciaTipoCargo(string nombreCargo,int nivelCargo,int agrupamiento);
      NivelEducativoModel GetNivelEducativoById(int id);
      void AgregarListaNivelesEducativo(TipoCargo entidad,TipoCargoModel modelo);
      void VerificarExistenciaTipoCargoCodigoPn(int? codigoPn);
      TipoCargoModel TipoCargoSaveOUpdate(TipoCargoModel modelo);
      
      void TipoCargoDelete(TipoCargoModel modelo);

      int ValidarEliminarTipoCargo(int idTipoCargo);
    }
}
