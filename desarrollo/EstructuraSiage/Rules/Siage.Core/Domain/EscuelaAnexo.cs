using System.Collections.Generic;
using System.Linq;
using Siage.Base;

namespace Siage.Core.Domain
{
    public class EscuelaAnexo : EmpresaBase 
    {
        public EscuelaAnexo()
        {
            TipoEmpresa = TipoEmpresaEnum.ESCUELA_ANEXO;
            PeriodosLectivo = new List<PeriodoLectivo>();
            NivelesEducativo = new List<NivelEducativoPorTipoEducacion>();
        }

        public virtual int NumeroEscuela { get; set; }
        public virtual int NumeroAnexo { get; set; }
        public virtual bool Religioso { get; set; }
        public virtual bool Arancelado { get; set; }
        public virtual bool Albergue { get; set; }
        public virtual string CUE { get; set; }
        public virtual int CUEAnexo { get; set; }
        public virtual string HorarioDeFuncionamiento { get; set; }
        public virtual string Colectivos { get; set; }
        public virtual bool EstructuraDefinitiva { get; set; }
        public virtual bool ContextoDeEncierro { get; set; }
        public virtual bool Hospitalaria { get; set; }
        public virtual bool Privado { get; set; }
        public virtual Escuela EscuelaMadre { get; set; }
        public virtual bool EsRaiz { get; set; }
        public virtual IList<EscuelaPlan> EscuelaPlan { get; set; }
        public virtual Escuela EscuelaRaiz { get; set; }
        public virtual EscuelaPrivada EscuelaPrivada { get; set; }
        public virtual TipoCooperadoraEnum? TipoCooperadora { get; set; }
        public virtual CategoriaEscuelaEnum TipoCategoria { get; set; }
        public virtual ZonaDesfavorable ZonaDesfavorable { get; set; }
        public virtual AmbitoEscuelaEnum? Ambito { get; set; }
        public virtual DependenciaEnum? Dependencia { get; set; }
        public virtual TipoEscuela TipoEscuela { get; set; }
        public virtual string CodigoInspeccion { get; set; }

        public virtual ModalidadJornada ModalidadJornada { get; set; }
        public virtual TipoJornada TipoJornada { get; set; }
        //public virtual IList<EscuelaPlan> PlanesEscuela { get; set; } Se borra porque está repetida la lista y no está en el mapper
        //Antes era lista (PeriodosLectivos). Verificar
        public virtual IList<PeriodoLectivo> PeriodosLectivo { get; set; }

        public virtual IList<NivelEducativoPorTipoEducacion> NivelesEducativo { get; set; }
        public virtual void AddTurnoPorEscuela(TurnoPorEscuela  turno)
        {
            turno.Escuela  = this;
            TurnosXEscuela.Add(turno); 
        }

        #region NoMapeados
        public virtual List<Turno> Turnos
        {
            get
            {
                if (TurnosXEscuela != null)
                {
                    List<Turno> _turnos = (from x in TurnosXEscuela
                                           select x.Turno).ToList<Turno>();
                    return _turnos;
                }
                else
                {
                    return null;
                }
            }

        }

        public virtual TipoEducacionEnum TipoEducacion
        {
            //como es el mismo valor en toda la lista, me quedo con el primero
            get { return (TipoEducacionEnum)NivelesEducativo[0].TipoEducacion  ; }
        }

        public virtual NivelEducativo NivelEducativo
        {
            //como es el mismo valor en toda la lista, me quedo con el primero
            get { return NivelesEducativo[0].NivelEducativo ; }
        }

        #endregion
    }
}
