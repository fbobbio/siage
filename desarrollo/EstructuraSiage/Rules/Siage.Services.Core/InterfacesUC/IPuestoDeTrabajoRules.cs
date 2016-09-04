using System;
using System.Collections.Generic;
using Siage.Base.Dto;
using Siage.Services.Core.Models;
using Siage.Base;

namespace Siage.Services.Core.InterfacesUC
{
    public interface IPuestoDeTrabajoRules
    {
        PuestoDeTrabajoModel GetPuestoDeTrabajoById(int id);
        PuestoDeTrabajoDetalleModel GetDetallePuestoDeTrabajo(int id);
        PuestoDeTrabajoProvisorioVerModel GetPuestoDeTrabajoProvisorioById(int id);
        PuestoDeTrabajoExternoModel GetPuestoDeTrabajoExternoById(int id);

        PuestoDeTrabajoModel GetPuestoDeTrabajoByIdUnidadAcademica(int idUnidadAcademica);

        List<DtoPuestoDeTrabajoControlConsulta> GetByFiltroBasico(int? codigoTipoCargo,
                                                                  string nombreTipoCargo,
                                                                  string codigoAgrupamiento,
                                                                  string codigoNivelCargo,
                                                                  EstadoPuestoDeTrabajoEnum? estadoPT,
                                                                  string nombreAsignatura,
                                                                  string tipoDocumento,
                                                                  string numeroDocumento,
                                                                  string tipoAgente,
                                                                  string codigoPosicionPN, int? idAgente, int? idEmpresa,
                                                                  int? idSituacionRevista, bool esPuestoActualMab,
                                                                  bool estadoEmpresaNoCerrada);

        List<DtoPuestoDeTrabajoControlConsulta> GetByFiltroAvanzado(string CUE,
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
                                                                    int? idAgente, int? idEmpresa,
                                                                    bool esPuestoActualMab, bool estadoEmpresaNoCerrada);


        PuestoDeTrabajoExternoModel PuestoDeTrabajoExternoUpdate(PuestoDeTrabajoExternoModel model);
        PuestoDeTrabajoExternoModel PuestoDeTrabajoExternoSave(PuestoDeTrabajoExternoModel model);
        
        List<AsignacionModel> GetAsignacionesByPuestoDeTrabajo(int puesto);

        List<PuestoDeTrabajoExternoModel> GetPuestoExternoByFiltros(int? fltEmpresa, DateTime? fltFechaDesdeInicio,
                    DateTime? fltFechaHastaInicio, DateTime? fltFechaDesdeFin,
                    DateTime? fltFechaHastaFin, int? fltAgente, EstadoPuestoDeTrabajoEnum? fltEstado);
        PuestoDeTrabajoProvisorioModel PuestoDeTrabajoProvisorioTareaPasivaSave(PuestoDeTrabajoProvisorioModel model);
        PuestoDeTrabajoProvisorioModel PuestoDeTrabajoProvisorioItineranteSave(PuestoDeTrabajoProvisorioModel model);
        PuestoDeTrabajoProvisorioModel PuestoDeTrabajoProvisorioOtroMinisterioSave(PuestoDeTrabajoProvisorioModel model);
        PuestoDeTrabajoProvisorioModel PuestoDeTrabajoProvisorioMaestraIntSave(PuestoDeTrabajoProvisorioModel model);
        PuestoDeTrabajoProvisorioModel PuestoDeTrabajoProvisorioDelete(PuestoDeTrabajoProvisorioModel model);
        List<PuestoDeTrabajoModel> GetPTVacantesByEmpresa(int empresaId);
        List<PuestoDeTrabajoConsultaModel> GetPTByAgente(int agenteId);
        EstadoPuestoDeTrabajoEnum GetEstadoPuestoById(int id);
    }
}

