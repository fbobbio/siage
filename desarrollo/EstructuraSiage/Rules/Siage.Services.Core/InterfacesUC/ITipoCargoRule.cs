using System.Collections.Generic;
using Siage.Services.Core.Models;
using Siage.Core.Domain;

namespace Siage.Services.Core.InterfacesUC
{
  public interface ITipoCargoRule
    {

      TipoCargoModel GetTipoCargoById(int id);
      List<TipoCargoModel> GetTipoCargo();
      TipoCargoModel TipoCargoSave(TipoCargoModel modelo,int idUsuario);
      List<TipoCargoModel> FiltroBusquedaTipoCargo(int? codigoPn,string NombTipoCargo);
      List<AgrupamientoModel> GetAgrupamientosAll();
      List<NivelCargoModel> GetNivelCargoAll();
      bool VerificarExistenciaTipoCargo(string nombreCargo,int nivelCargo,int agrupamiento);
      NivelEducativoModel GetNivelEducativoById(int id);
      void AgregarListaNivelesEducativo(TipoCargo entidad,TipoCargoModel modelo);
      void VerificarExistenciaTipoCargoCodigoPn(int? codigoPn);
      TipoCargoModel TipoCargoSaveOUpdate(TipoCargoModel modelo, int IdUsuario);
      
      void TipoCargoDelete(TipoCargoModel modelo, int IdUsuario);

      int ValidarEliminarTipoCargo(int idTipoCargo);
    }
}
