using System.Collections.Generic;
using Siage.Services.Core.Models;


namespace Siage.Services.Core.InterfacesUC
{
    public interface ITituloRules
    {
        TituloModel TituloDelete(TituloModel model);
        TituloModel GetTituloById(int tituloId);
        TituloModel TituloSave(TituloModel model);
        TituloModel TituloUpdate(TituloModel model);
        List<TituloModel> GetTituloByFiltros(string nombre, int? nivel, bool dadosDeBaja, int? idEcuela);
        void ValidarRelacionPlanEstudio(TituloModel model);


        List<TituloModel> GetTitulo(string filtroNombre, int? filtroNivel, bool p, int idEmpresa);
    }           
}