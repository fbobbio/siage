using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Data.DAO;
using Siage.Services.Core.Models;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class EmpresaRegistrarTurnosPorEscuela : ValueResolver<EmpresaBase, List<TurnoModel>>
    {
        protected override List<TurnoModel> ResolveCore(EmpresaBase source)
        {
            List<TurnoModel> listaModelTurnos = new List<TurnoModel>();
            if (source.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var escuela = new DaoProvider().GetDaoEscuela().GetById(source.Id);
                if (escuela.TurnosXEscuela != null)
                    listaModelTurnos.AddRange(escuela.TurnosXEscuela.Select(t => Mapper.Map<Turno, TurnoModel>(t.Turno)));
                return listaModelTurnos;
            }
            else if (source.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                var escuelaAnexo = new DaoProvider().GetDaoEscuelaAnexo().GetById(source.Id);
                if (escuelaAnexo.TurnosXEscuela != null)
                    listaModelTurnos.AddRange(escuelaAnexo.TurnosXEscuela.Select(t => Mapper.Map<Turno, TurnoModel>(t.Turno)));
                return listaModelTurnos;
            }
            else
                return listaModelTurnos;
        }
    }

    public class EmpresaModificarEstructuraEscolar : ValueResolver<EmpresaBase, List<DiagramacionCursoRegistrarModel>>
    {
        protected override List<DiagramacionCursoRegistrarModel> ResolveCore(EmpresaBase source)
        {
            var daoProvider = new DaoProvider();
            List<DiagramacionCurso> listadoEstructuraEscolar = new List<DiagramacionCurso>();
            if (source.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || source.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
                listadoEstructuraEscolar = daoProvider.GetDaoDiagramacionCurso().GetByEscuela(source.Id);
            return Mapper.Map<List<DiagramacionCurso>, List<DiagramacionCursoRegistrarModel>>(listadoEstructuraEscolar);
        }
    }

    public class EmpresaObtenerAtributoPrivada : ValueResolver<EmpresaBase, bool>
    {
        protected override bool ResolveCore(EmpresaBase source)
        {
            var daoProvider = new DaoProvider();
            if(source.TipoEmpresa==TipoEmpresaEnum.ESCUELA_MADRE)
            {
                var escuela = daoProvider.GetDaoEscuela().GetById(source.Id);
                return escuela.EscuelaPrivada != null;
            }
            else if(source.TipoEmpresa==TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                var escuelaAnexo = daoProvider.GetDaoEscuelaAnexo().GetById(source.Id);
                return escuelaAnexo.EscuelaPrivada != null;
            }
            else
            return false;
        }
    }
}
