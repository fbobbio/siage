using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Siage.Base;
using System.ComponentModel;
using ValidationAttributes;

namespace Siage.Services.Core.Models
{
      [ConditionalRequired("TipoGestion", "ESCUELA", "TipoCooperadora", "Ambito")]
      [ConditionalRequired("TipoEmpresa", "DIRECCION_DE_NIVEL", "Sigla")]
    public class EmpresaRegistrarModel
    {
        public int Id { get; set; }
        [Required, DisplayName("Código empresa"),StringLength(9)]
        public virtual string CodigoEmpresa { get; set; }

        [Required, StringLength(50, ErrorMessage = "El campo nombre no puede superar los 50 dígitos.")]
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        //lo quitaron del diagrama(vanesa)
        //public virtual DateTime? FechaNotificacion { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaUltimaModificacion { get; set; }

        [DisplayName("Fecha de cierre")]
        public virtual DateTime? FechaCierre { get; set; }
        [DisplayName("Usuario que solicito cierre")]
        public virtual string UsuarioCierre { get; set; }

        public virtual DateTime? FechaBaja { get; set; }
        public virtual string Observaciones { get; set; }

        [DisplayName("Teléfono"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&/='´¨+*~^`-_¡!¿?(){}[]<>"), StringLength(12)]
        public virtual string Telefono { get; set; }

        [Required, DisplayName("Fecha inicio de actividades")]
        public virtual DateTime FechaInicioActividades { get; set; }
        [Required, DisplayName("Orden de pago")]
        public virtual int? OrdenDePagoId { get; set; }
        [Required, DisplayName("Programa presupuestario")]
        public virtual int? ProgramaPresupuestarioId { get; set; }

        [DisplayName("Tipo empresa")]
        public TipoEmpresaEnum TipoEmpresa { get; set; }
        [Required]
        [DisplayName("Tipo gestión")]
        public virtual TipoGestionEnum? TipoGestion { get; set; }
        //[Required]
        public virtual int? EmpresaInspeccionId { get; set; }
        //[Required]
        public virtual int? EmpresaInspeccionSupervisoraId { get; set; }

        [Required]
        public virtual int AgenteAltaId { get; set; }
        public virtual int EmpresaRegistro { get; set; }
        public virtual int EmpresaPadreOrganigramaId { get; set; }
        /*public virtual OrdenDePagoModel OrdenDePago { get; set; }
        public virtual DomicilioModel Domicilio { get; set; }
        public virtual ServicioEntidadModel Servicio { get; set; }*/
        public virtual int? AgenteUltimaModificacionId { get; set; }
        public virtual IList<EdificioModel> Edificio { get; set; }
        public virtual long? DomicilioId { get; set; }
        public virtual string CalleNuevoDomicilio { get; set; }
        public virtual string AlturaNuevoDomicilio { get; set; }
        public virtual EstadoEmpresaEnum? EstadoEmpresa { get; set; }
        public virtual IList<HistorialEstadoEmpresaModel> HistorialEstadoEmpresa { get; set; }

        /*  ESCUELA */
        public virtual int? EscuelaMadreId { get; set; }
        public virtual int? EscuelaRaizId { get; set; }

        [DisplayName("Número escuela")]
        public virtual int? NumeroEscuela { get; set; }

        [DisplayName("Es privada")]
        public virtual bool Privado { get; set; }
        public virtual bool Religioso { get; set; }
        public virtual bool Arancelado { get; set; }
        public virtual bool Albergue { get; set; }
        [ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>", ErrorMessage = "Se permiten solos números para el campo 'CUE'")]
        public virtual string CUE { get; set; }
        [DisplayName("CUE anexo"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>", ErrorMessage = "Se permiten solos números para el campo 'CUE anexo'")]
        public virtual int? CUEAnexo { get; set; }
        [DisplayName("Horario de funcionamiento")]
        public virtual string HorarioDeFuncionamiento { get; set; }
        public virtual string Colectivos { get; set; }
        //public virtual EscuelaPrivadaModel EscuelaPrivada { get; set; }
        [DisplayName("Tipo cooperadora (*)")]
        public virtual TipoCooperadoraEnum? TipoCooperadora { get; set; }
        [DisplayName("Categoría (*)")]
        public virtual CategoriaEscuelaEnum? CategoriaEscuela { get; set; }
        //[DisplayName("Zona desfavorable")]
        //public virtual bool ZonaDesfavorable { get; set; }
        [DisplayName("Zona desfavorable (*)")]
        public virtual int? ZonaDesfavorableId { get; set; }
        [DisplayName("Ámbito (*)")]
        public virtual AmbitoEscuelaEnum? Ambito { get; set; }
        [DisplayName("Dependencia (*)")]
        public virtual DependenciaEnum? Dependencia { get; set; }        
        [DisplayName("Modalidad jornada")]
        public virtual int? ModalidadJornada { get; set; }
        [DisplayName("Tipo jornada")]
        public virtual int? TipoJornada { get; set; }
        [DisplayName("Tipo inspección de escuela")]
        public virtual TipoInspeccionEnum? TipoInspeccionDeEscuela { get; set; }
        [DisplayName("Es inspección principal")]
        public virtual bool EsInspeccionPrincipal { get; set; }
        [DisplayName("Contexto encierro")]
        public virtual bool ContextoDeEncierro { get; set; }
        [DisplayName("Es hospitalaria")]
        public virtual bool EsHospitalaria { get; set; }
        [DisplayName("Es raíz")]
        public virtual bool EsRaiz { get; set; }
        [DisplayName("Número anexo (*)"), ValidationAttributes.CaracteresEspeciales("|°¬#$%&='´¨+*~^`-_¡!¿?(){}[]<>",ErrorMessage="Se permiten solos números para el campo 'Número Anexo'")]
        public virtual int? NumeroAnexo { get; set; }
        public virtual List<PeriodoLectivoModel> PeriodosLectivos { get; set; }
        [DisplayName("Período lectivo (*)")]
        public virtual int? PeriodoLectivoId { get; set; }
        [DisplayName("Turno")]
        public virtual List<TurnoModel> Turnos { get; set; }
        [DisplayName("Turno (*)")]
        public virtual int? TurnoId { get; set; }
        [DisplayName("Tipo escuela")]
        public virtual int? TipoEscuela { get; set; }
        //public virtual int? TipoEscuela2 { get; set; }
        [DisplayName("Código de inspección (*)")]
        public virtual string CodigoInspeccion { get; set; }

        /*  ESCUELA PRIVADA */
        [UIHint("PersonaFisicaEditor")]
        public PersonaFisicaModel Director { get; set; }
        [UIHint("PersonaFisicaEditor")]
        public PersonaFisicaModel RepresentanteLegal { get; set; }
        //[DisplayName("Apellido director")]
        //public virtual string ApellidoDirector { get; set; }
        //[DisplayName("Nombre director")]
        //public virtual string NombreDirector { get; set; }
        //[DisplayName("Tipo documento director")]
        //public virtual int? IdTipoDocumentoDirector { get; set; }
        //[DisplayName("Número documento director")]
        //public virtual string NumeroDocumentoDirector { get; set; }
        //[DisplayName("Sexo director")]
        //public virtual string SexoDirector { get; set; }
        //[DisplayName("Sexo representante legal")]
        //public virtual string SexoRepresentanteLegal { get; set; }
        //[DisplayName("Apellido representante legal")]
        //public virtual string ApellidoRepresentanteLegal { get; set; }
        //[DisplayName("Nombre representante legal")]
        //public virtual string NombreRepresentanteLegal { get; set; }
        //[DisplayName("Tipo documento representante legal")]
        //public virtual int? IdTipoDocumentoRepresentanteLegal { get; set; }
        //[DisplayName("Número documento representante legal")]
        //public virtual string NumeroDocumentoRepresentanteLegal { get; set; }
        [DisplayName("Porcentaje aporte estado (*)")]
        public virtual float? PorcentajeAporteEstado { get; set; }
        [DisplayName("Sucursal bancaria (*)")]
        public virtual int? SucursalBancariaId { get; set; }
        [DisplayName("Número cuenta bancaria (*)"), StringLength(30)]
        public virtual string NumeroCuentaBancaria { get; set; }
        [DisplayName("Obra social")]
        public virtual int? ObraSocialId { get; set; }
        [UIHint("AsignacionInstrumentoLegalEditor")]
        public virtual AsignacionInstrumentoLegalModel AsignacionInstrumentoLegalZonaDesfavorable { get; set; }
        [DisplayName("Fecha notificación asignación instrumento legal")]
        public virtual DateTime? FecNotificacionAsignacionILZD { get; set; }
        /** La estructura escolar que se pueden haber cargado */
        protected IList<DiagramacionCursoRegistrarModel> _estructuraEscolar;
        [UIHint("EstructuraEscuelaRegistrarEditor")]
        public virtual IList<DiagramacionCursoRegistrarModel> EstructuraEscolar
        {
            get { return _estructuraEscolar; }
            set
            {
                _estructuraEscolar = value;
            }
        }

        public virtual bool EstructuraDefinitiva { get; set; }

        /** Los planes de estudio que se pueden haber cargado*/
        public List<EscuelaPlanModel> PlanDeEstudio { get; set; }

        /*  INSPECCION  */
        [DisplayName("Tipo inspección (*)")]
        public virtual TipoInspeccionEnum? TipoInspeccionEnum { get; set; }
        [DisplayName("Tipo inspección")]
        public virtual int? TipoInspeccion { get; set; }
        [DisplayName("Tipo inspección intermedia")]
        public virtual int TipoInspeccionIntermediaId { get; set; }
        [DisplayName("Inspección superior")]
        public virtual int? IdInspeccionSuperior { get; set; }
        [DisplayName("Asignación escuela")]
        public virtual List<AsignacionInspeccionEscuelaModel> AsignacionEscuela { get; set; }

        /*  DIRECCION DE NIVEL  */
        [DisplayName("Sigla (*)"), StringLength(14)]
        public virtual string Sigla { get; set; }
        //[DisplayName("Tipo escuela")]
        //public virtual int? TipoEscuela { get; set; }

        /* ATRIBUTOS Q SE REPITEN EN VARIOS TIPOS DE EMPRESA*/
        [DisplayName("Nivel educativo (*)")]
        public virtual int? NivelEducativoId { get; set; }
        [DisplayName("Tipo educación (*)")]
        public virtual TipoEducacionEnum? TipoEducacion { get; set; }

        public virtual List<NivelEducativoPorTipoEducacionModel> NivelEducativoPorTipoEducacion { get; set; }
        public virtual List<TipoEscuelaModel> TiposEscuelas { get; set; }

        /* LISTA DE ATRIBUTOS QUE NECESITEN INSTRUMENTOS LEGALES */
        [UIHint("AsignacionInstrumentoLegalEditor")]
        //public virtual Dictionary<string, AsignacionInstrumentoLegalModel> InstrumentosLegales { get; set; }
        public virtual AsignacionInstrumentoLegalModel InstrumentosLegales { get; set; }
        public virtual List<AsignacionInstrumentoLegalModel> AsignacionesIntrumentosLegales { get; set; }
        [DisplayName("Fecha notificación asignación instrumento legal")]
        public virtual DateTime? FecNotificacionAsignacionIL { get; set; }

        /*  ULTIMAS PROPIEDADES AGREGADAS*/
        [DisplayName("Fecha notificación")]
        public virtual DateTime? FechaNotificacion { get; set; }
        public virtual int? TipoDireccionDeNivelId { get; set; }
        public virtual string EmpresaPadreCodigo { get; set; }
        public virtual string DireccionDeNivelCodigo { get; set; }
        public virtual List<VinculoEmpresaEdificioModel> VinculoEmpresaEdificio { get; set; }

        public virtual string EmpresaPadreCod { get; set; }
        public virtual string EmpresaPadreNombre { get; set; }

        public virtual string EscuelaRaizCod { get; set; }
        public virtual string EscuelaRaizNombre { get; set; }

        public virtual string EscuelaMadreCod { get; set; }
        public virtual string EscuelaMadreNombre { get; set; }

        public virtual string EmpresaInspeccionCod { get; set; }
        public virtual string EmpresaInspeccionNombre { get; set; }

        public virtual string EmpresaSupervisoraCod { get; set; }
        public virtual string EmpresaSupervisoraNombre { get; set; }

        [DisplayName("Código")]
        public virtual string CodigoEmpresaQueRegistro { get; set; }
        [DisplayName("Nombre")]
        public virtual string NombreEmpresaQueRegistro { get; set; }

    }
}
