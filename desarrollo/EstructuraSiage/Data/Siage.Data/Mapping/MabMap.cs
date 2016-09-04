using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate;
using Siage.Core.Domain;
using Siage.Data.CustomTypes;

namespace Siage.Data.Mapping
{
    public class MabMap: ClassMap<Mab> 
    {
        
        public  MabMap ()

        {
            Id(x => x.Id, "ID_SEQ_MAB").GeneratedBy.Native("SEQ_DO_MABS");

            Map(x => x.FechaConfeccion, "FEC_CONFECCION");
            Map(x => x.FechaDesde, "FEC_DESDE");
            Map(x => x.FechaHasta, "FEC_HASTA");
            References<TipoNovedad>(x => x.TipoNovedad,"ID_SEQ_TIPO_NOVEDAD").Not.Nullable();
            References<Agente>(x => x.AgenteMab, "ID_AGENTE_MAB").Not.Nullable();
            References<Asignacion>(x => x.AsignacionAgenteOrigen, "ID_SEQ_ASIG_AGENTE_ORIGEN");
            References<AsignacionInstrumentoLegal>(x => x.ActoAdministrativo, "ID_SEQ_ASIGNACION_INSTR_LEG").Cascade.SaveUpdate();
            References<Agente>(x => x.AgenteAutorizado, "ID_AGENTE_AUTORIZADO");
            References<CodigoMovimientoMab>(x => x.CodigoMovimiento, "ID_SEQ_CODIGO_MOV_MAB").Not.Nullable();//NO APARECE EN TABLA !!!
            References<PuestoDeTrabajo>(x => x.PuestoDeTrabajo, "ID_SEQ_PUESTO_TRABAJO").Not.Nullable();
            References<Asignacion>(x => x.Asignacion, "ID_SEQ_ASIGNACION").Not.Nullable();//NO APARECE EN TABLA !!!
            References<SituacionDeRevista>(x => x.SituacionDeRevista, "ID_SEQ_SITUACION_REVISTA").Not.Nullable();
            Map(x => x.Observaciones, "OBSERVACIONES").Length(100);
            Map(x => x.CodigoBarra, "CODIGO_BARRA").Length(60).Not.Nullable();
            Map(x => x.FechaAutorizacionCargo, "FECHA_AUTORIZ_CARGO").Not.Nullable();
            References<Asignacion >(x => x.PuestoDeTrabajoAnterior, "ID_SEQ_ASIGN_PUE_TRAB_ANTERIOR");
            References<ModalidadMab>(x => x.Modalidad, "ID_SEQ_MODALIDAD_MABS");
            References<Usuario>(x => x.UsuarioAlta, "ID_SEQ_USUARIO_ALTA").Not.Nullable();
            References<Usuario>(x => x.UsuarioModif, "ID_SEQ_USUARIO_MODIFICACION");
            Map(x => x.FechaUltModif, "FEC_ULTIMA_MODIFICACION");
            
            Table("T_DO_MABS");
            Cache.ReadWrite();



   

}
    }
      

  
}
