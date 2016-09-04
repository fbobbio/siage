using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Siage.Base;
using Siage.Base.Dto;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using Siage.UCControllers.Resources;

namespace Siage.UCControllers.Rules
{
    public class VinculoEmpresaEdificioRules: IVinculoEmpresaEdificioRules
    {
        #region Atributos
        
        private IDaoProvider _daoProvider;
        private IDaoEdificio _daoEdificio;
        private IDaoVinculoEmpresaEdificio _daoVinculoEmpresaEdificio;
        private IDaoEmpresaBase _daoEmpresa;

        #endregion

        #region Propiedades
        private IDaoProvider DaoProvider
        {
            get
            {
                if (_daoProvider == null)
                {
                    _daoProvider = ServiceLocator.Current.GetInstance<IDaoProvider>();
                }
                return _daoProvider;
            }
        }

        private IDaoVinculoEmpresaEdificio DaoVinculoEmpresaEdificio
        {
            get
            {
                if (_daoVinculoEmpresaEdificio == null)
                {
                    _daoVinculoEmpresaEdificio = DaoProvider.GetDaoVinculoEmpresaEdificio();
                }
                return _daoVinculoEmpresaEdificio;
            }
        }

        private IDaoEmpresaBase DaoEmpresa
        {
            get
            {
                if (_daoEmpresa == null)
                {
                    _daoEmpresa = DaoProvider.GetDaoEmpresaBase();
                }
                return _daoEmpresa;
            }
        }

        private IDaoEdificio DaoEdificio
        {
            get
            {
                if (_daoEdificio == null)
                {
                    _daoEdificio = DaoProvider.GetDaoEdificio();
                }
                return _daoEdificio;
            }
        }

        #endregion

        #region Soporte

        public List<VinculoEmpresaEdificioModel> GetVinculoEmpresaEdificioByFiltros(string filtroCodigoEmpresa, string filtroNombreEmpresa, string filtroCodigoEdificio, EstadoVinculoEmpresaEdificioEnum? filtroEstadoVinculo)
        {
            var lista = DaoVinculoEmpresaEdificio.GetVinculoEmpresaEdificioByFilters(filtroCodigoEmpresa, filtroNombreEmpresa, filtroCodigoEdificio, filtroEstadoVinculo);
            return AutoMapper.Mapper.Map<List<VinculoEmpresaEdificio>, List<VinculoEmpresaEdificioModel>>(lista);
        }

        private void ValidarVinculoEmpresaEdificioSave(RegistrarVinculoEmpresaEdificioModel modelo)
        {
            //TODO Esto no se puede validar con los DECORATORS del modelo?
            if (string.IsNullOrEmpty(modelo.Empresa))
            {
                throw new BaseException(VinculoEmpresaEdificioResource.EMPRESA_NULL);
            }
            if (modelo.ListaEdificios == null || modelo.ListaEdificios.Count == 0)
            {
                throw new BaseException(VinculoEmpresaEdificioResource.EDIFICIO_NUL);
            }
            if (modelo.FechaDesde == null)
            {
                throw new BaseException(VinculoEmpresaEdificioResource.FECHA_NULL);
            }
        }

        private void ValidarVinculoEmpresaEdificioDelete(VinculoEmpresaEdificio entidad)
        {
            var listadoDeErrores = new StringBuilder();
            if(entidad.Empresa == null || (entidad.Empresa.Estado != EstadoEmpresaEnum.AUTORIZADA && entidad.Empresa.Estado != EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE_AUTORIZADO_NOTIFICADO))
            {
                throw new ApplicationException(VinculoEmpresaEdificioResource.ESTADO_EMPRESA);
            }
            var vinculos = DaoVinculoEmpresaEdificio.GetVinculoEmpresaEdificioByFilters(entidad.Empresa.CodigoEmpresa, null);
            
            if (vinculos == null || vinculos.Count == 0)
            {
                listadoDeErrores.AppendLine(VinculoEmpresaEdificioResource.VINCULO_NO_EXISTE);
            }
            // pregunto si el vinculo es el activo para esa empresa, ya que debe haber un vinculo que coincida con el domicilio de la misma
            if(entidad.DeterminaDomicilio)
            {
                listadoDeErrores.AppendLine(VinculoEmpresaEdificioResource.MISMO_DOMICILIO);
            }
            if (!String.IsNullOrEmpty(listadoDeErrores.ToString()))
            {
                throw new BaseException(listadoDeErrores.ToString());
            }

        }

        public PredioModel GetPredioById(int id)
        {
            var predio = new PredioRules().GetPredioById(id);
            return predio;
        }


        public LocalidadModel GetLocalidadById(int id)
        {
            var localidad = new LocalidadRules().GetLocalidadById(id);
            return localidad;
        }

        public VinculoEmpresaEdificioModel GetRegistrarVinculoEmpresaEdificioById(int id)
        {
            var entidad = DaoVinculoEmpresaEdificio.GetById(id);
            return AutoMapper.Mapper.Map<VinculoEmpresaEdificio, VinculoEmpresaEdificioModel>(entidad);
        }

        public VinculoEmpresaEdificioModel GetVinculoEmpresaEdificioById(int id)
        {
            var entidad = DaoVinculoEmpresaEdificio.GetById(id);
            return AutoMapper.Mapper.Map<VinculoEmpresaEdificio, VinculoEmpresaEdificioModel>(entidad);
        }

        #endregion

        #region IVinculoEmpresaEdificioRules Members

        /// <summary>
        /// VinculoEmpresaEdificioUnicoSave guarda un vínculo de empresa a edificio asumiendo ESTRICTAMENTE que la lista
        /// de edificios del model viene con sólo 1 elemento 
        /// </summary>
        /// <param name="modelo">Modelo del vínculo a guardar </param>
        /// <returns>Modelo del Vínculo guardado</returns>
        public VinculoEmpresaEdificioModel VinculoEmpresaEdificioUnicoSave(RegistrarVinculoEmpresaEdificioModel modelo)
        {
            return VinculoEmpresaEdificioSave(modelo)[0];
        }

        /// <summary>
        /// VinculoEmpresaEdificioUnicoSave guarda un vínculo de empresa a edificio asumiendo ESTRICTAMENTE que la lista
        /// de edificios del model viene con sólo 1 elemento 
        /// </summary>
        /// <param name="modelo">Modelo del vínculo a guardar </param>
        /// <returns>Modelo del Vínculo guardado</returns>
        public VinculoEmpresaEdificioModel VinculoEmpresaEdificioUnicoValidar(RegistrarVinculoEmpresaEdificioModel modelo)
        {
            return VinculoEmpresaEdificioRegla(modelo,false)[0];
        }

        public List<VinculoEmpresaEdificioModel> VinculoEmpresaEdificioSave(RegistrarVinculoEmpresaEdificioModel modelo)
        {
            return VinculoEmpresaEdificioRegla(modelo, true);
        }

        /// <summary>
        /// VinculoEmpresaEdificioRegla realiza todas las validaciones de los vínculos y según el parámetro persiste o no
        /// </summary>
        /// <param name="modelo">Modelo del vínculo a guardar </param>
        /// <returns>Modelo del Vínculo guardado</returns>
        private List<VinculoEmpresaEdificioModel> VinculoEmpresaEdificioRegla(RegistrarVinculoEmpresaEdificioModel modelo, bool persist)
        {
            ValidarVinculoEmpresaEdificioSave(modelo);
            int idEmpresa;
            int.TryParse(modelo.Empresa, out idEmpresa);
            var listaVinculos = new List<VinculoEmpresaEdificio>();
            var listadoDeErrores = new StringBuilder();
            if (idEmpresa > 0)
            {
                var empresa = DaoEmpresa.GetById(idEmpresa);

                foreach (var idEdificio in modelo.ListaEdificios)
                {
                    if (DaoVinculoEmpresaEdificio.VerificarVinculoEmpresaEdificio(idEdificio, idEmpresa))
                    {
                        var edificio = DaoEdificio.GetById(idEdificio);
                        var vinculo = new VinculoEmpresaEdificio();
                        vinculo.Empresa = empresa;
                        vinculo.Edificio = edificio;
                        vinculo.Estado = EstadoVinculoEmpresaEdificioEnum.ACTIVO;
                        if (modelo.FechaDesde != null) vinculo.FechaDesde = modelo.FechaDesde.Value;
                        vinculo.Observacion = modelo.Observacion;
                        vinculo.DeterminaDomicilio = false;
                        if (persist)
                        {
                            vinculo = DaoVinculoEmpresaEdificio.SaveOrUpdate(vinculo);
                        }
                        listaVinculos.Add(vinculo);
                    }
                    else
                    {
                        var edificio = DaoEdificio.GetById(idEdificio);
                        listadoDeErrores.AppendLine("El vinculo con el edificio " + edificio.IdentificadorEdificio +
                                                    " ya existe");
                    }
                }
            }
            if (!String.IsNullOrEmpty(listadoDeErrores.ToString()))
            {
                throw new BaseException(listadoDeErrores.ToString());
            }
            return AutoMapper.Mapper.Map<List<VinculoEmpresaEdificio>, List<VinculoEmpresaEdificioModel>>(listaVinculos);
        }
       

        public VinculoEmpresaEdificioModel VinculoEmpresaEdificioDelete(VinculoEmpresaEdificioModel modelo)
        {
            var entidad = DaoVinculoEmpresaEdificio.GetById(modelo.Id);
            entidad.Motivo = modelo.Motivo;
            ValidarVinculoEmpresaEdificioDelete(entidad);
            entidad.FechaHasta = DateTime.Today;
            entidad.Estado = EstadoVinculoEmpresaEdificioEnum.INACTIVO;
            entidad = DaoVinculoEmpresaEdificio.SaveOrUpdate(entidad);
            return AutoMapper.Mapper.Map<VinculoEmpresaEdificio, VinculoEmpresaEdificioModel>(entidad);
        }

        public List<DtoVinculoEmpresaReporte> GetVinculosEmpresaByFilter(string filtroIdentificadorEmpresa, string filtroNombreEmpresa, int? filtroDepartamento, int? filtroLocalidad, int? filtroBarrio, int? filtroCalle)
        {
            var lista = DaoProvider.GetDaoVinculoEmpresaEdificio().GetVinculosEmpresaByFilter(filtroIdentificadorEmpresa, filtroNombreEmpresa, filtroDepartamento, filtroLocalidad, filtroBarrio, filtroCalle);
            return lista;
        }

        #endregion


        public bool ValidarEliminarVinculo(int idVinculo)
        {
            var entidad = DaoVinculoEmpresaEdificio.GetById(idVinculo);
            if (entidad.Empresa == null || (entidad.Empresa.Estado != EstadoEmpresaEnum.AUTORIZADA && entidad.Empresa.Estado != EstadoEmpresaEnum.EN_PROCESO_DE_CIERRE_AUTORIZADO_NOTIFICADO))
            {
                return false;
            }
            return true;
        }
    }
}
