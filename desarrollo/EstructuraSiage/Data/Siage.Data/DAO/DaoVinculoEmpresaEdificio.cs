using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;

namespace Siage.Data.DAO
{
    public class DaoVinculoEmpresaEdificio : DaoBase<VinculoEmpresaEdificio, int>, IDaoVinculoEmpresaEdificio
    {
        public DaoVinculoEmpresaEdificio()
        { 
        
        }

        public List<VinculoEmpresaEdificio> GetVinculoEmpresaEdificioByFilters(string filtroCodigoEmpresa, string filtroNombreEmpresa, string filtroCodigoEdificio, EstadoVinculoEmpresaEdificioEnum? filtroEstadoVinculo)
        {
            var query = Session.QueryOver<VinculoEmpresaEdificio>();
            var empresa = new EmpresaBase();
            var edificio = new Edificio();
            if (!String.IsNullOrEmpty(filtroCodigoEmpresa) || !String.IsNullOrEmpty(filtroNombreEmpresa))
            {
                query.JoinQueryOver<EmpresaBase>(x => x.Empresa, () => empresa);
                if (!String.IsNullOrEmpty(filtroCodigoEmpresa))
                {
                    query.WhereRestrictionOn(() => empresa.CodigoEmpresa).IsLike("%" + filtroCodigoEmpresa.ToUpper() +
                                                                                 "%");
                }
                if (!String.IsNullOrEmpty(filtroNombreEmpresa))
                {
                    query.WhereRestrictionOn(() => empresa.Nombre).IsLike("%" + filtroNombreEmpresa.ToUpper() +
                                                                                 "%");
                }
            }

            if (filtroEstadoVinculo.HasValue)
                query.And(x => x.Estado == filtroEstadoVinculo);

            if (!String.IsNullOrEmpty(filtroCodigoEdificio))
            {
                query.JoinQueryOver<Edificio>(x => x.Edificio, () => edificio);
                query.WhereRestrictionOn(() => edificio.IdentificadorEdificio).IsLike("%" + filtroCodigoEdificio.ToUpper() +
                                                             "%");
            }


            return (List<VinculoEmpresaEdificio>)query.List<VinculoEmpresaEdificio>();
        }

        public List<VinculoEmpresaEdificio> GetVinculoEmpresaEdificioByFilters(string filtroCodigoEmpresa, string filtroNombreEmpresa)
        {
            var query = Session.QueryOver<VinculoEmpresaEdificio>();
            var empresa = new EmpresaBase();
            var edificio = new Edificio();
            if (!String.IsNullOrEmpty(filtroCodigoEmpresa) || !String.IsNullOrEmpty(filtroNombreEmpresa))
            {
                query.JoinQueryOver<EmpresaBase>(x => x.Empresa, () => empresa);
                if (!String.IsNullOrEmpty(filtroCodigoEmpresa))
                {
                    query.Where(() => empresa.CodigoEmpresa == filtroCodigoEmpresa.ToUpper());
                }
                if (!String.IsNullOrEmpty(filtroNombreEmpresa))
                {
                    query.Where(() => empresa.Nombre == filtroNombreEmpresa.ToUpper());
                }
            }

            return (List<VinculoEmpresaEdificio>)query.List<VinculoEmpresaEdificio>();
        }


        public bool VerificarVinculoEmpresaEdificio(int idEdificio, int idEmpresa)
        {
            var query = Session.QueryOver<VinculoEmpresaEdificio>();

            if (idEdificio > 0)
                query.And(x => x.Edificio.Id == idEdificio);
            if (idEmpresa > 0)
                query.And(x => x.Empresa.Id == idEmpresa);
            query.And(x => x.Estado == EstadoVinculoEmpresaEdificioEnum.ACTIVO);
            var lista = (List<VinculoEmpresaEdificio>)query.List<VinculoEmpresaEdificio>();
            return lista.Count <= 0;
        }

        public void BorrarVinculosDeEmpresa(int idEmpresa)
        {
            //FALTA IMPLEMENTAR
        }

        public List<DtoVinculoEmpresaReporte> GetVinculosEmpresaByFilter(string filtroIdentificadorEmpresa, string filtroNombreEmpresa, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle)
        {
            var query = Session.QueryOver<DtoVinculoEmpresaReporte>();

            if (!string.IsNullOrEmpty(filtroIdentificadorEmpresa))
                query.And(x => x.IdentificadorEmpresa == filtroIdentificadorEmpresa);

            if (!string.IsNullOrEmpty(filtroNombreEmpresa))
                query.And(x => x.NombreEmpresa == filtroNombreEmpresa);

            if (filtroDepartamento.HasValue && filtroDepartamento > 0)
                query.And(x => x.IdDeptoProvincialEmpresa == filtroDepartamento.Value);

            if (filtroLocalidad.HasValue && filtroLocalidad > 0)
                query.And(x => x.IdLocalidadEmpresa == filtroLocalidad.Value);

            if (filtroBarrio.HasValue && filtroBarrio > 0)
                query.And(x => x.IdBarrioEmpresa == filtroBarrio.Value);

            if (filtroCalle.HasValue && filtroCalle > 0)
                query.And(x => x.IdCalleEmpresa == filtroCalle.Value);

            return (List<DtoVinculoEmpresaReporte>)query.List<DtoVinculoEmpresaReporte>();
        }
    }
}
