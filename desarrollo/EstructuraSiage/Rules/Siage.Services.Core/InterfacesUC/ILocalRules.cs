using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface ILocalRules
    {
        List<LocalModel> GetLocalByFiltros(string nombre,int? idTipoLocal,int? numeroLocal);
        LocalModel LocalDelete(LocalModel entidad);
        LocalModel GetLocalById(int id);
        LocalModel LocalSave(LocalModel entidad);
        void ValidarLocalDelete(LocalModel local);
        void ValidarLocalSave(LocalModel local);
        void ValidarLocal(LocalModel local);


        void ValidarLocalController(LocalModel model);
    }
}

