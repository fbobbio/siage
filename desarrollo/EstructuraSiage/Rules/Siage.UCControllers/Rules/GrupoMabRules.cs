using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;
using Siage.Data.DAO;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;

namespace Siage.UCControllers.Rules
{
    /** 
    * <summary> GrupoMabRules GrupoMabRules
    *	
    * </summary>
    * <remarks>
    *		Autor: Owner
    *		Fecha: 6/30/2011 4:34:13 PM
    * </remarks>
    */
    public class GrupoMabRules : IGrupoMabRules
    {
        #region Atributos

        private IDaoProvider _daoProvider;

        #endregion

        #region Propiedades

        private IDaoProvider DaoProvider
        {
            get
            {
                if (_daoProvider == null)
                    _daoProvider = new DaoProvider();
                return _daoProvider;
            }
        }

        #endregion

        #region IGrupoMabRules Members

        public List<CodigoMovimientoMabModel> GetCodigosMovimientoByGrupoMabId(int grupoMabId)
        {
            var lista = DaoProvider.GetDaoCodigoMovimientoMab().GetCodigosMovimientoByGrupoMab(grupoMabId);
            return Mapper.Map<List<CodigoMovimientoMab>, List<CodigoMovimientoMabModel>>(lista);
        }

        public List<EstadoPuestoModel> GetAllEstadosPuesto()
        {
            var lista = DaoProvider.GetDaoEstadoPuestoTrabajo().GetAll();
            return Mapper.Map<List<EstadoPuesto>, List<EstadoPuestoModel>>(lista);
        }

        public List<GrupoMabModel> GetGrupoMabByFiltros(TipoGrupoMabEnum? tipoGrupoMabEnum, int? numeroGrupoMab, string codigoMovimientoMab)
        {
            var gruposModelPorFiltro = new List<GrupoMabModel>();
            var gruposPorFiltro = DaoProvider.GetDaoGrupoMab().GetGrupoMabByFiltros(tipoGrupoMabEnum, numeroGrupoMab, codigoMovimientoMab);

            foreach (var i in gruposPorFiltro)
                gruposModelPorFiltro.Add(GenerarModel(i));

            return (List<GrupoMabModel>)gruposModelPorFiltro;
        }

        public List<EstadoAsignacionModel> GetAllEstadosAsignacion()
        {
            var lista = DaoProvider.GetDaoEstadoAsignacion().GetAll();
            return Mapper.Map<List<EstadoAsignacion>, List<EstadoAsignacionModel>>(lista);
        }

        public List<EstadoPuestoDeTrabajoEnum> GetEstadoPuestosPorEjecucionMabId(int ejecucionMabId)
        {
           return DaoProvider.GetDaoEjecucionMab().GetEstadoPuestosPorEjecucionMabId(ejecucionMabId);
        }

        public GrupoMabModel GrupoMabSave(GrupoMabModel model)
        {
            //ValidarGrupoMab(model);

            if (model.Id == 0)
            {
                model.NumeroGrupo = DaoProvider.GetDaoGrupoMab().GetUltimoNumeroCorrelativo();
            }

            var grupoMabGuardado = DaoProvider.GetDaoGrupoMab().SaveOrUpdate(GenerarEntidad(model));
            var codigos = new List<CodigoMovimientoMab>();

            CodigoMovimientoMab codigo;

            foreach (var c in model.CodigosMovimientoMab)
            {
                codigo = DaoProvider.GetDaoCodigoMovimientoMab().GetById(c.Id);
                codigos.Add(codigo);
                codigo.GrupoMab = grupoMabGuardado;
                DaoProvider.GetDaoCodigoMovimientoMab().SaveOrUpdate(codigo);
            }

            //cuando edito
            if(model.Id>0)
            {
                var codigosSinGrupo = DaoProvider.GetDaoCodigoMovimientoMab().GetCodigosMovimientoQueNoEstanEn(codigos,
                                                                                                               model.Id);
                foreach (var codigoNoAsignado in codigosSinGrupo)
                {
                    codigoNoAsignado.GrupoMab = null;
                    DaoProvider.GetDaoCodigoMovimientoMab().SaveOrUpdate(codigoNoAsignado);
                }
            }
           

            return GenerarModel(grupoMabGuardado);
        }

        //TODO falta implementar
        public GrupoMabModel GrupoMabReactivar(GrupoMabModel model)
        {
            return null;
        }

        public GrupoMabModel GetGrupoMabById(int grupoMabId)
        {
            var entidad = DaoProvider.GetDaoGrupoMab().GetById(grupoMabId);
            return GenerarModel(entidad);
        }

        public void GrupoMabDelete(GrupoMabModel model)
        {
            var codigosParaGrupoMab = DaoProvider.GetDaoCodigoMovimientoMab().GetCodigosMovimientoByGrupoMab(model.Id);
            var tieneAsociado = false;
            
            foreach (var codigoMab in codigosParaGrupoMab)
            {
                if (DaoProvider.GetDaoMab().TieneAsociadoCodigoMovimiento(codigoMab.Id))
                {
                    tieneAsociado = true;
                    break;
                }
            }

            var entidadAEliminar = DaoProvider.GetDaoGrupoMab().GetById(model.Id);

            if (tieneAsociado)
                throw new BaseException(Resources.Mab.MabPoseeCodigosMovimientoAsignado);

            foreach (var codigoMab in codigosParaGrupoMab)
            {
                codigoMab.GrupoMab = null;
                DaoProvider.GetDaoCodigoMovimientoMab().SaveOrUpdate(codigoMab);
            }

            DaoProvider.GetDaoGrupoMab().Delete(entidadAEliminar);
        }

        //TODO bajar a DAO con un metodo que solo devuelva la cantidad
        public int GetCantidadCodigosMovimientoMab(int grupoMabId)
        {
            return DaoProvider.GetDaoCodigoMovimientoMab().GetCantidadCodigoMovimientoMabPorGrupoMab(grupoMabId);
        }

        #endregion

        #region Soporte

        private void ValidarGrupoMab(GrupoMabModel model)
        {
            model.EstadoAnteriorPtId = model.EstadoAnteriorPtId.HasValue ? model.EstadoAnteriorPtId : 0;
            model.EstadoAnteriorPtAnteriorId = model.EstadoAnteriorPtAnteriorId.HasValue ? model.EstadoAnteriorPtAnteriorId : 0;


            var error = new StringBuilder();

            if (model.CodigosMovimientoMab == null)
                model.CodigosMovimientoMab = new List<CodigoMovimientoMabModel>();

            if (!model.EnptId.HasValue)
                model.EnptId = 0;
            
            if (!model.EnPTanteriorId.HasValue)
                model.EnPTanteriorId = 0;
            //es un nuevo grupo
            if (model.Id == 0 && model.CodigosMovimientoMab.Count == 0)
                error.Append(Resources.Mab.GrupoSinCodigos);

            if (model.TipoGrupo == 0)
                error.Append("El campo Tipo Grupo(*) es obligatorio");

           

            if (error.Length > 0)
                throw new BaseException(error.ToString());
        }
        
        private GrupoMabModel GenerarModel(GrupoMab entidad)
        {
            var model= Mapper.Map<GrupoMab, GrupoMabModel>(entidad);
            //model.EstadoPosteriorPtId = (int)entidad.Enpt.EstadoPosteriorPt.Valor;    
            model.EstadoPosteriorPtId = (int) (entidad.Enpt != null && entidad.Enpt.EstadoPosteriorPt!=null? entidad.Enpt.EstadoPosteriorPt.Valor:0);
            model.EstadoPosteriorPtAnteriorId =
                (int) (entidad.EnPTanterior != null && entidad.EnPTanterior.EstadoPosteriorPt!=null ? entidad.EnPTanterior.EstadoPosteriorPt.Valor : 0);
            return model;
        }

        //TODO separar este metodo en varios submetodos asi se hace mas facil de entender y mantener
        private GrupoMab GenerarEntidad(GrupoMabModel model)
        {
            ValidarGrupoMab(model);
            var entidad = Mapper.Map<GrupoMabModel, GrupoMab>(model);
            var estadoPtAnteriorEjecucionMab = new List<EstadoPuestoDeTrabajoEnum>();
            var estadoPtAnteriorEjecucionMabAnterior = new List<EstadoPuestoDeTrabajoEnum>();

            if (model.EstadosPuestoAnterioresPt != null)
                foreach (var estadoPuestoModel in model.EstadosPuestoAnterioresPt)
                    estadoPtAnteriorEjecucionMab.Add(estadoPuestoModel.Valor);

            if (model.EstadosPuestoAnterioresPtAnterior != null)
                foreach (var estadoPuestoModel in model.EstadosPuestoAnterioresPtAnterior)
                    estadoPtAnteriorEjecucionMabAnterior.Add(estadoPuestoModel.Valor);

            entidad.Enpt = new EjecucionMab();
            entidad.EnPTanterior = new EjecucionMab();
          
            

            entidad.Enpt.Liquidacion = model.Liquidacion;
            entidad.Enpt.ModificaSitRev = model.ModificaSitRev;
            entidad.Enpt.GeneraVacante = model.GeneraVacante;
            entidad.Enpt.ModificaEstadoAnteriorEnPT = model.ModificaEstadoAnteriorPuesto;
            entidad.Enpt.ModificaEstadoAsignacionEnPT = model.ModificaEstadoAsignacionPuesto;
            entidad.Enpt.ModificaEstadoPosteriorEnPT = model.ModificaEstadoPosteriorPuesto;

            if (model.EstadoAsignacionId.HasValue)
            {
                entidad.Enpt.EstadoAsignacion = new EstadoAsignacion();
                entidad.Enpt.EstadoAsignacion =
                    DaoProvider.GetDaoEstadoAsignacion().GetById(model.EstadoAsignacionId.Value);

            }
            entidad.EnPTanterior.Liquidacion = model.LiquidacionAnterior;
            entidad.EnPTanterior.ModificaSitRev = model.ModificaSitRevAnterior;
            entidad.EnPTanterior.GeneraVacante = model.GeneraVacanteAnterior;
            entidad.EnPTanterior.ModificaEstadoAnteriorEnPT = model.ModificaEstadoAnteriorPuestoAnterior;
            entidad.EnPTanterior.ModificaEstadoAsignacionEnPT = model.ModificaEstadoAsignacionPuestoAnterior;
            entidad.EnPTanterior.ModificaEstadoPosteriorEnPT = model.ModificaEstadoPosteriorPuestoAnterior;

            if (model.EstadoAsignacionAnteriorId.HasValue)
            {
                entidad.EnPTanterior.EstadoAsignacion = new EstadoAsignacion();
                entidad.EnPTanterior.EstadoAsignacion =
                    DaoProvider.GetDaoEstadoAsignacion().GetById(model.EstadoAsignacionAnteriorId.Value);
            }
            //obtenemos los estados (anteriores y posterior) de puesto de trabajo actual
            var listaEstadosNuevoPuesto = new List<EjecucionMABEstadosPuesto>();

            if (model.EstadosPuestoAnterioresPt != null && model.ModificaEstadoAnteriorPuesto)
            {
                foreach (var emep in model.EstadosPuestoAnterioresPt)
                {
                    var emepNuevo = new EjecucionMABEstadosPuesto();
                    emepNuevo.EjecucionMab = entidad.Enpt;
                    emepNuevo.EstadoPuesto = DaoProvider.GetDaoEstadoPuestoTrabajo().GetByFilter(emep.Valor);
                    emepNuevo.EsPosterior = false;
                    listaEstadosNuevoPuesto.Add(emepNuevo);
                }
             

            }

            if (model.EstadoPosteriorPtId.HasValue && model.ModificaEstadoPosteriorPuesto)
            {
                listaEstadosNuevoPuesto.Add(new EjecucionMABEstadosPuesto()
                {
                    EjecucionMab = entidad.Enpt,
                    EsPosterior = true,
                    EstadoPuesto = DaoProvider.GetDaoEstadoPuestoTrabajo().GetById(model.EstadoPosteriorPtId.Value)
                });
            }

            //obtenemos los estados (anteriores y posterior) de puesto de trabajo anterior
            var listaEstadosPuestoAnterior = new List<EjecucionMABEstadosPuesto>();

            if (model.EstadosPuestoAnterioresPtAnterior != null && model.ModificaEstadoAnteriorPuestoAnterior)
            {
                foreach (var emepAnterior in model.EstadosPuestoAnterioresPtAnterior)
                {
                    var emepAnteriorNuevo = new EjecucionMABEstadosPuesto();
                    
                    emepAnteriorNuevo.EjecucionMab = entidad.EnPTanterior;
                    emepAnteriorNuevo.EstadoPuesto = DaoProvider.GetDaoEstadoPuestoTrabajo().GetByFilter(emepAnterior.Valor);
                    emepAnteriorNuevo.EsPosterior = false;
                    listaEstadosPuestoAnterior.Add(emepAnteriorNuevo);
                }
              
            }

            if (model.EstadoPosteriorPtAnteriorId.HasValue && model.ModificaEstadoPosteriorPuestoAnterior)
            {
                listaEstadosPuestoAnterior.Add(new EjecucionMABEstadosPuesto()
                                                  {
                                                      EjecucionMab = entidad.EnPTanterior,
                                                      EsPosterior = true,
                                                      EstadoPuesto =
                                                          DaoProvider.GetDaoEstadoPuestoTrabajo().GetById(
                                                              model.EstadoPosteriorPtAnteriorId.Value)
                                                  });
            }

            entidad.Enpt.EstadosPuestos = listaEstadosNuevoPuesto;
            entidad.EnPTanterior.EstadosPuestos = listaEstadosPuestoAnterior;
            
            return entidad;
        }

        #endregion
    }
}