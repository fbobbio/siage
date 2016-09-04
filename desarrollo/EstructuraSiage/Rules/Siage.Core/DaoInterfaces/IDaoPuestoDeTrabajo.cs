using Siage.Base.Dto;
using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoPuestoDeTrabajo : IDao<PuestoDeTrabajo, int>
    {
        List<DtoPuestoDeTrabajoControlConsulta> FiltroBasico(int? codigoTipoCargo,
                                                             string nombreTipoCargo,
                                                             string codigoAgrupamiento,
                                                             string codigoNivelCargo,
                                                             EstadoPuestoDeTrabajoEnum? estadoPT,
                                                             string nombreAsignatura,
                                                             string tipoDocumento,
                                                             string numeroDocumento,
                                                             string tipoAgente,
                                                             string codigoPosicionPN,
                                                             int? idAgente, int? idEmpresa, int? idSituacionRevista,
                                                             bool esPuestoActualMab,
                                                             bool estadoEmpresaNoCerrada);

        List<DtoPuestoDeTrabajoControlConsulta> FiltroAvanzado(string CUE,
                                                               string codigoEmpresa,
                                                               string nombreEmpresa,
                                                               EstadoEmpresaEnum? estadoEmpresa,
                                                               TipoEmpresaEnum? tipoEmpresa,
                                                               NoSiEnum? escuela,
                                                               int? nivelEducativo,
                                                               string nombreProgPresup,
                                                               NoSiEnum? presupuestado,
                                                               NoSiEnum? liquidado,
                                                               string tipoPuesto,
                                                               EstadoAsignacionEnum? estadoAsignacion,
                                                               int? situacionRevista,
                                                               string tipoInspeccion,
                                                               NoSiEnum? itinerante,
                                                               string codigoAsignatura,
                                                               NoSiEnum? materiaEspecial,
                                                               int? gradoAño,
                                                               DivisionEnum? division,
                                                               int? turno,
                                                               string nombreCarrera,
                                                               int? idAgente, int? idEmpresa, bool esPuestoActualMab,
                                                               bool estadoEmpresaNoCerrada);

        List<Asignacion> GetAsignacionesDePuestoTrabajoByEstado(int idPuesto, EstadoAsignacionEnum estado);       
        List<Asignacion> GetAsignacionesByPuestoDeTrabajoAgenteYEstado(int idPuesto, int idAgente, EstadoAsignacionEnum estado);        
        List<Asignacion> GetAsignacionesByPuestoDeTrabajoAgente(int idAgente, int idPuesto);
        List<Asignacion> GetAsignacionesByPuestoDeTrabajo(int idPuesto);
        List<DtoPuestoDeTrabajoConsulta> GetPuestoTrabajoActivoByAgente(int idAgente);        
        List<PuestoDeTrabajo> GetPuestoExternoByFiltros(int? fltEmpresa, DateTime? fltFechaDesdeInicio,
                    DateTime? fltFechaHastaInicio, DateTime? fltFechaDesdeFin,
                    DateTime? fltFechaHastaFin, int? fltAgente, EstadoPuestoDeTrabajoEnum? fltEstado);

        PuestoDeTrabajo GetPuestoDeTrabajoByIdUnidadAcademica(int idUnidadAcademica);        
        Asignacion GetUltimaAsignacionByPuestoDeTrabajoAgente(int idAgente, int idPuesto);
        Asignacion GetAsignacionRecienteByPuestoDeTrabajoYAgente(int idPuesto, int idAgente);
        Asignacion GetAsignacionAgenteUltimaAsignacionInactiva(int idPuesto);
        Agente GetAgenteUltimaAsignacionInactiva(int idPuesto);
        List<Agente> GetDirectoresEmpresa(int idEmpresa);
        Asignacion GetUltimaAsignacionxEstado(int idPuesto, EstadoAsignacionEnum estado);
        Agente GetAgenteByPuestoDeTrabajo(int idPuesto, int idEmpresa);
        List<DtoPuestoDeTrabajoConsulta> GetPuestoTrabajoActivoByAgenteYEmpresa(int idAgente, int idEmpresa);
        bool ValidarExistenciaCodigoPN(string codigo, int pt);
        bool VerificarTodosEstadosPuestosDeEmpresaSean(int idEmpresa, List<EstadoPuestoDeTrabajoEnum> estadosList);
        List<PuestoDeTrabajo> GetPTVacantesByEmpresa(int empresaId);
        EstadoPuestoDeTrabajoEnum GetEstadoPuestoById(int id);
    }
}