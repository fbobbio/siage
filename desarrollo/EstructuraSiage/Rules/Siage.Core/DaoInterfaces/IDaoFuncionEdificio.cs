using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoFuncionEdificio : IDao<FuncionEdificio, int>
    {
        List<FuncionEdificio> GetByFiltros();
        List<FuncionEdificio> GetByFiltros(int? id, string nombre, string descripcion, bool? dadosDeBaja);

        List<FuncionEdificio> GetByFiltros(string nombre);

        List<FuncionEdificio> ValidarFuncionEdificioExistente(int? idFuncion, string nombre, bool? dadosDeBaja);
    }
}

