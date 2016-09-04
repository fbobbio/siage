using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Siage.Base;
using System.ComponentModel.DataAnnotations;

namespace Siage.Services.Core.Models
{
    /** 
    * <summary> Clase RegistrarMABDeAltaModel
    *	
    * </summary>
    * <remarks>
    *		Autor: fede
    *		Fecha: 6/10/2011 11:48:43 AM
    * </remarks>
    */
    public class MabModel
    {
        #region Constantes
        #endregion

        #region Atributos

            #region AtributosMABsComunes
            #endregion

            #region AtributosMABsDeAlta

            //Faltan los decorators para saber que atributos usar
            
            public virtual int Id { get; set; }
            [DisplayName("Fecha actual")]
            public virtual DateTime FechaActual { get; set; }
            [DisplayName("Tipo novedad"),Required]
            public virtual int? TipoNovedadId { get; set; }

            [DisplayName("Nombre empresa")]
            public virtual string EmpresaNombre { get; set; }
            [DisplayName("Código empresa")]
            public virtual string EmpresaCodigo { get; set; }

            public virtual AsignacionModel AsignacionAgente { get; set; }
            [UIHint("AsignacionInstrumentoLegalEditor")]
            public virtual AsignacionInstrumentoLegalModel AsignacionInstrumentoLegalAgente { get; set; }

            [DisplayName("Ingresar datos acto administrativo:")]
            public virtual bool CargarInstrumentoLegalCheck { get; set; }

            [DisplayName("Modalidad Mab")]
            public virtual int? ModalidadId { get; set; }
            [Required]
            [DisplayName("Situación de revista")]
            public virtual int? SituacionRevistaId { get; set; }

            public virtual string CodigoCargo { get; set; }
        
            public virtual AsignacionModel AsignacionAgenteReemplazado { get; set; }
            //TODO: Falta agregar el motivo de inasistencia y la descripcion, no está la tabla tipo motivo inasistencia

            [DisplayName("Fecha desde")]
            public virtual DateTime? FechaNovedadDesde { get; set; }
            [DisplayName("Fecha hasta")]
            public virtual DateTime? FechaNovedadHasta { get; set; }
            
            [DisplayName("Código de novedad")]
            public virtual int CodigoDeNovedadId { get; set; }

            public virtual SucursalBancariaModel SucursalBancaria { get; set; }

            [DisplayName("Registrar datos del cargo")]
            public virtual bool RegistrarCargoAnterior { get; set; }
            [DisplayName("Es cargo de empresa del ministerio")]
            public virtual bool EsCargoDeEmpresaMinisterio { get; set; }

            public virtual int? IdPuestoAnteriorDeMinisterio { get; set; }
            [DisplayName("Observaciones del cargo anterior (Nombre empresa, cargo, situación de revista, plan de estudio, materia, etc.)")]
            public virtual string ObservacionesCargoAnterior { get; set; }
            [DisplayName("Observaciones generales")]
            public virtual string ObservacionesMab { get; set; }
            [DisplayName("Imprimir Mab:")]
            public virtual bool DeseaImprimir { get; set; }

            /** Estos datos son los que se van a mostrar en la grilla del consultar.*/
            [DisplayName("Materia")]
            public virtual string Materia { get; set; }
            [DisplayName("Turno")]
            public virtual string Turno { get; set; }
            [DisplayName("Grado/Año")]
            public virtual string GradoAno { get; set; }
            [DisplayName("Sección/División")]
            public virtual string SeccionDivision { get; set; }
            [DisplayName("Horas")]
            public virtual int Horas { get; set; }

            public virtual bool PuestoTrabajoActualMovimiento { get; set; }

            #endregion

        #endregion

        #region Constructores
        #endregion

        #region Métodos Públicos
        #endregion

        #region Métodos Privados
        #endregion

        #region Overrides

        public String ToString()
        {
            return base.ToString();
        }

        public int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
