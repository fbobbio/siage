using System.Collections.Generic;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IParametroRules
    {
        ValorParametroModel GetValorParametroVigente(ParametroEnum parametro, int? idEscuela);
        List<ParametroModel> GetParametroByPaquete(PaqueteParametroEnum paquete);
        List<ValorParametroModel> GetValorParametroVigenteByFiltro(ParametroEnum? parametro, string nombre, int? idEscuela);
        List<ValorParametroModel> GetHistorialParametro(ParametroEnum parametro, int? idEscuela);
        ValorParametroModel Save(ValorParametroModel valorParametro);
        ValorParametroModel SaveParametroEscuela(ValorParametroModel valorParametro);
        ParametroModel GetParametroById(int id);
        bool ExisteParametro(ParametroEnum parametro);
    }
}
