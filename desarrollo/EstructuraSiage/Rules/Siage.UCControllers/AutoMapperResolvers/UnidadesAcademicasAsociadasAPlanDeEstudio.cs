using AutoMapper;
using Siage.Core.Domain;
using Siage.Services.Core.Models;
using Siage.Data.DAO;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class UnidadesAcademicasAsociadasAPlanDeEstudioGradoAnio : ValueResolver<Mab, string>
    {
        protected override string ResolveCore(Mab source)
        {
            if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas == null || source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count == 0)
                return null;
            else
            {
                if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count > 0
                    && source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0] != null)
                {
                    return source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DiagramacionCurso.GradoAnio.Nombre;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class UnidadesAcademicasAsociadasAPlanDeEstudioHoras : ValueResolver<Mab, int>
    {
        protected override int ResolveCore(Mab source)
        {
            if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas == null || source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count == 0)
            {
                if (source.Asignacion.PuestoDeTrabajo.TipoCargo != null)
                {
                    return source.Asignacion.PuestoDeTrabajo.TipoCargo.CantidadHoras;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count > 0)
                {
                    if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DetalleAsignatura != null
                        && source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DetalleAsignatura.Count > 0)
                    {
                        if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DetalleAsignatura.Count > 0)
                        {
                            return
                            source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DetalleAsignatura[0].HorasSemanales;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DetalleSubGrupo != null)
                        {
                            return source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DetalleSubGrupo.HorasSemanales;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class UnidadesAcademicasAsociadasAPlanDeEstudioMaterias : ValueResolver<Mab, string>
    {
        protected override string ResolveCore(Mab source)
        {
            if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas == null || source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count == 0)
                return null;
            else
            {
                if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count == 0 ||
                    source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0] == null ||
                    source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].AsignaturaEscuela == null ||
                    source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].AsignaturaEscuela.Count == 0 || 
                    source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].AsignaturaEscuela[0] == null || 
                    source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].AsignaturaEscuela[0].CodigoAsignatura == null || 
                    source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].AsignaturaEscuela[0].CodigoAsignatura.Asignatura == null)
                    return null;
                return
                    source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].AsignaturaEscuela[0].CodigoAsignatura.
                        Asignatura.Nombre;
            }
        }
    }

    public class UnidadesAcademicasAsociadasAPlanDeEstudioSeccionDivision : ValueResolver<Mab, string>
    {
        protected override string ResolveCore(Mab source)
        {
            if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas == null || source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count == 0)
                return null;
            else
            {
                if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DiagramacionCurso != null)
                {
                    return source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DiagramacionCurso.Division.ToString();
                }
                else
                {
                    return source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].Seccion.Nombre.ToString();
                }
            }
        }
    }

    public class UnidadesAcademicasAsociadasAPlanDeEstudioTurno : ValueResolver<Mab, string>
    {
        protected override string ResolveCore(Mab source)
        {
            if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas == null || source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas.Count == 0)
                return null;
            else
            {
                if (source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DiagramacionCurso != null && source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DiagramacionCurso.Turno != null)
                {
                    return source.Asignacion.PuestoDeTrabajo.UnidadesAcademicas[0].DiagramacionCurso.Turno.Nombre.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class InstrumentoLegalEnAsignacionMab : ValueResolver<AsignacionInstrumentoLegalModel, InstrumentoLegal>
    {
        protected override InstrumentoLegal ResolveCore(AsignacionInstrumentoLegalModel source)
        {
            if (source.InstrumentoLegal == null || !source.InstrumentoLegal.Id.HasValue)
                return null;
            else
            {
                var daoProvider = new DaoProvider();
                if (source.InstrumentoLegal.Id > 0)
                    return daoProvider.GetDaoInstrumentoLegal().GetById(source.InstrumentoLegal.Id.Value);
                else
                    return new InstrumentoLegal()
                    {
                        FechaAlta = source.InstrumentoLegal.FechaAlta,
                        NroInstrumentoLegal = source.InstrumentoLegal.NroInstrumentoLegal,
                        FechaEmision = source.InstrumentoLegal.FechaEmision.Value,
                        Observaciones = source.InstrumentoLegal.Observaciones,
                        EmisorInstrumentoLegal = source.InstrumentoLegal.EmisorInstrumentoLegal.Value,
                        TipoInstrumentoLegal = source.InstrumentoLegal.IdTipoInstrumentoLegal > 0 ? daoProvider.GetDaoTipoInstrumentoLegal().GetById(source.InstrumentoLegal.IdTipoInstrumentoLegal.Value) : null,
                        Expediente = source.InstrumentoLegal.Expediente != null ? daoProvider.GetDaoExpediente().GetById(source.InstrumentoLegal.Expediente.Id.Value) : null
                    };
            }
        }

    }
}
