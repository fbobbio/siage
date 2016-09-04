using System.Collections.Generic;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoParametro : IDao<Parametro, int>
    {
        List<Parametro> GetParametroByPaquete(PaqueteParametroEnum paquete);
        ValorParametro GetValorParametroVigente(ParametroEnum parametro, int? idEscuela);
        bool ExisteParametro(ParametroEnum parametro);
        List<ValorParametro> GetValorParametroVigenteByFiltro(ParametroEnum? parametro, string nombre, int? idEscuela);
        List<ValorParametro> GetHistorialParametro(ParametroEnum parametro, int? idEscuela);
    }
}
