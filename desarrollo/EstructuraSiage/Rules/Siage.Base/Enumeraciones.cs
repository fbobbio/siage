using System.ComponentModel;

namespace Siage.Base
{
    public enum PortalEnum
    {
        ESCUELA = 1,
        HOGAR,
        MINISTERIO
    }

    public enum ConstantesSession
    {
        USUARIO = 1,
        EMPRESA
    }

    public enum EstadoPlanEstudioEnum
    {
        PROVISORIO = 1,
        VIGENTE,
        NO_VIGENTE
    }

    public enum DiaSemanaEnum
    {
        LUNES = 1,
        MARTES,
        MIERCOLES,
        JUEVES,
        VIERNES,
        SABADO
    }

    public enum EstadoInscripcionEnum
    {
        INSCRIPTO = 1,
        NO_APROBADO,
        APROBADO,
        CERRADA,
        ANULADA,
        LIBRE_SANCIONES,
        LIBRE_ASISTENCIAS
    }

    public enum EstadoDiagramacionCursoEnum
    {
        HABILITADA = 1,
        EN_PROCESO_DE_CIERRE,
        CERRADA
    }

    public enum EstadoTipoMovimientoInstrumentoLegalEnum
    {
        ACTIVO = 1,
        INACTIVO
    }

    public enum EntidadAlquiladaEnum
    {
        PARCELA = 1,
        EDIFICIO,
        BIEN,
        DEPOSITO
    }

    public enum FactorRiesgoAmbientalEnum
    {
        ZONA_INUNDABLE = 1,
        ZONA_SISMICA,
        ZONA_DE_ALUDES_DERRUMBES,
        ZONA_DE_INFLUENCIA_VOLCANICA,
        NAPAS_CONTAMINANTES_LOCALIZADO_A_MENOS_DE_100_MTS_DE_RUTAS_TRANSITADAS,
        LOCALIZADO_A_MENOS_DE_500_MTS_DE_BASURALES,
        LOCALIZADO_A_MENOS_DE_500_MTS_DE_MATADEROS,
        LOCALIZADO_A_MENOS_DE_500_MTS_DE_DEPOSITOS_DE_SUSTANCIAS_INFLAMABLES_O_EXPLOSIVOS,
        LOCALIZADO_A_MENOS_DE_500_MTS_DE_FABRICAS_U_OTRO_FOCO_CONTAMINANTE,
        LOCALIZADO_A_MENOS_DE_100_MTS_DE_TENDIDOS_DE_ALTA_TENSION,
        LOCALIZADO_A_MENOS_DE_2000_MTS_DE_AEROPUESTOS_U_OTRO_GENERADOR_DE_RUIDOS_INTENSIVOS,
        OTROS
    }

    public enum TipoActoAdministrativoEnum
    {
        ACTA = 1,
        DECRETO,
        RESOLUCION,
        MEMORANDUM,
        FORMULARIO_24,
        LEY,
        NOTA,
        RESOLUCION_CJPR,
        ACUERDO,
        EXPEDIENTE,
        RESOLUCION_DIRECCION_GENERAL,
        RESOLUCION_REGIONAL,
        RESOLUCION_INSPECCION_GENERAL,
        PENDIENTE_INSTRUMENTO_LEGAL,
        RESOLUCION_MINISTERIAL,
        RESOLUCION_DIPE,
        RESOLUCION_DEMIS,
        RESOLUCION_DRE,
        CONVENIO
    }

    public enum CategoriaEscuelaEnum
    {
        UNO = 1,
        DOS,
        TRES,
        CUATRO
    }

    public enum CondicionIvaEnum
    {
        UNO=1,
        DOS=2
    }
    public enum EstadoCargoMinimoEnum
    {
        VIGENTE = 1,
        NO_VIGENTE
    }

    //Tipo Empresa queda como enumeración
    public enum TipoEmpresaEnum
    {
        MINISTERIO = 1,
        SECRETARIA,
        SUBSECRETARIA,
        APOYO_ADMINISTRATIVO,
        DIRECCION_DE_INFRAESTRUCTURA,
        DIRECCION_DE_RECURSOS_HUMANOS,
        DIRECCION_DE_SISTEMAS,
        DIRECCION_DE_TESORERIA,
        DIRECCION_DE_NIVEL,//9
        INSPECCION,
        ESCUELA_MADRE,
        ESCUELA_ANEXO
    }

    public enum TipoEmpresaExternaEnum
    {
        PRESTADOR_DE_SERVICIO_PUBLICO_PRIVADO = 1,
        COOPERATIVA,
        COOPERADORA,
        MINISTERIO_DE_OTRAS_PROVINCIAS,
        PAICOR,
        OBRAS_SOCIALES_DE_MAESTRAS_INTEGRADORAS,
        SISTEMA_PREGASE,
        MINISTERIO_OTROS_DE_CÓRDOBA,
        PRESTADOR_DE_SERVICIO_MEDICO
    }
    //TODO VANESA: falta cargar y verificar si es una enumeración o tabla
    public enum MotivoEnum
    {
        CIERRE = 1,
    }

    public enum MotivoBajaContratoEnum
    {
        CIERRE = 1,
    }

    public enum MotivoRescisionContratoEnum
    {
        CIERRE = 1,
    }

    public enum VisualizacionEmpresaEnum
    {
        A = 1,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L
    }

    public enum TipoAsistenciaEnum
    {
        AUSENTE = 1,
        AUSENTE_JUSTIFICADO,
        LLEGADA_TARDE,
        PRESENTE,
        INASISTENCIA_NO_COMPUTABLE
    }

    //TODO VANE: Quitar esta enumeracion, es una CLASE (Ale)
    public enum TipoSancionEnum
    {
        AMONESTACIÓN = 1,
        EXPULSIÓN,
        SUSPENSIÓN,
        APERCIBIMIENTO
    }

    public enum MotivoClaustroEnum
    {
        APLICAR_CANTIDAD_MAXIMA_DE_AMONESTACIONES = 1,
        EXPULSION_DEL_ESTUDIANTE,
        SUSPENSION_DEL_ESTUDIANTE
    }

    public enum DivisionEnum
    {
        A = 1,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J
    }

    //TODO VANE. Esta enum es la clase TipoNota, porque ademas de nombre tiene una lista de NivelesEducativos
    public enum TipoNotaEnum
    {
        EVALUACION = 1,
        ETAPA,
        EXAMEN,
        FINAL,
        PARCIAL        
    }

    public enum TipoNotaLocaEnum
    {
        [Description("D")]
        TAREAS_PASIVAS = 1,
        [Description("D")]
        CAMBIO_DE_ÁMBITO_LABORAL,
        [Description("D")]
        TRASLADO_PROVISORIO,
        [Description("D")]
        PASE_A_COMISIÓN,
        [Description("E")]
        EVALUACIÓN,
        [Description("E")]
        ETAPA,
        [Description("E")]
        EXAMEN,
        [Description("E")]
        FINAL,
        [Description("E")]
        PARCIAL,
        [Description("I")]
        TRÁMITE,
        [Description("I")]
        INTIMACIÓN
    }

    public enum TipoIngresoEnum
    {
        DIRECTO = 1,
        INDIRECTO
    }

    public enum TipoExamenEnum
    {
        COLOQUIO = 1,
        COMPLEMENTARIO,
        PREVIA_REGULAR,
        PREVIA_LIBRE,
        EQUIVALENCIA,
        LIBRE
    }

    public enum TipoCondicionInasistenciaEnum
    {
        NECESIDAD_REPOSO_EMBARAZO = 1,
        ALUMBRAMIENTO,
        FRANQUICIA_AMAMANTAMIENTO,
        REPRESENTACION_DEPORTIVA,
        CELEBRACION_DEL_AÑO_NUEVO_JUDIO,
        CREDOS_RECONOCIDOS_POR_EL_MINISTERIO
    }

    //public enum TipoAsignaturaEnum
    //{
    //    OBLIGATORIA = 1,
    //    DEFINICION_INSTITUCIONAL
    //}

    public enum TipoBecaEnum
    {
        NACIONAL = 1,
        PROVINCIAL,
        MUNICIPAL
    }

    public enum TipoInscripcionEnum
    {
        CURSADO_LIBRE = 1,
        CURSADO_NORMAL
    }
    public enum VinculoEnum
    {
        JEFE_A_FAMILIA = 1,
        NIETO_A,
        SOBRINO_A,
        HERMANO_A,
        ADOPCION,
        HIJO_INCAPACITADO,
        EN_GUARDIA,
        PADRE_MADRE,
        OTROS_FAMILIARES,
        CONYUGE,
        PRENATAL,
        HIJO_A,
        TIO_A,
        TUTOR_A,
        DESCONOCIDO,
        BISNIETO_A,
        CUÑADO_A,
        PRIMO_A,
        SUEGRO_A,
        NUERA,
        YERNO,
        ABUELO,
        CONVIVIENTE,
        HIJO_DEL_CONYUGE,
        AMIGO,
        CONOCIDO,
        DIRECTOR_A,
        DOCENTE,
        VECINO_A
    }

    public enum CondicionCorrelatividadEnum
    {
        PARA_CURSAR = 1,
        PARA_RENDIR
    }

    public enum EstadoAsignaturaCorrelativaEnum
    {
        REGULAR = 1,
        APROBADA
    }

    public enum ProcesoEnum
    {
        PRE_INSCRIPCION = 1,
        INSCRIPCION,
        PASE,
        EXAMEN,
        SORTEO, //5
        PRUEBA_DE_APTITUD,
        MATRICULACION,
        RECESO,
        PERIODO_DE_CLASES,
        ETAPA1, //10
        ETAPA2,
        ETAPA3,
        INSCRIPCION_SEMESTRE1,
        INSCRIPCION_SEMESTRE2 //14
    }

    public enum TipoHoraEnum
    {
        HORA_CATEDRA = 1,
        RECREO
    }

    public enum PaqueteParametroEnum
    {
        ESTUDIANTES = 1,
        PLAN_DE_ESTUDIOS,
        DOCENTES,
        PLANTA_FUNCIONAL,
        INFRAESTRUCTURA,
        COSTOS,
        REGLAS_DE_NEGOCIO
    }

    public enum AmbitoAplicacionEnum
    {
        NACIONAL = 1,
        PROVINCIAL,
        MUNICIPAL
    }

    public enum EstadoCalendarioEnum
    {
        ACTIVO = 1,
        DE_BAJA
    }

    public enum TipoFechaEnum
    {
        DIA_HABIL = 1,
        DIA_NO_HABIL
    }

    public enum TipoErrorImportacionEnum
    {
        ALUMNO_EXISTENTE = 1,
        ESCUELA_INEXISTENTE,
        PROVINCIA_DE_RESIDENCIA_INEXISTENTE,
        VALOR_NO_ADMITIDO_EN_SEXO,
        TIPO_DE_DOCUMENTO_INCORRECTO,
        VALOR_NULO_EN_ATRIBUTO_CON_DATO_REQUERIDO,
        FORMATO_INCORRECTO_DE_FECHA,
        LOCALIDAD_DE_RESIDENCIA_INEXISTENTE,
        VALOR_INCORRECTO_EN_GRADO_AÑO,
        VALOR_INCORRECTO_EN_TURNO,
        VALOR_INCORRECTO_EN_DIVISION,
        VALOR_NULO_EN_CARRERA, _CUANDO_LA_ESCUELA_ES_DE_NIVEL_SUPERIOR
    }

    public enum CondicionAsignaturaEnum
    {
        ADEUDA = 1,
        REGULAR,
        REGULAR_EN_CURSO,
        PREVIO_LIBRE,
        PREVIO_REGULAR,
        EQUIVALENCIA_RECONOCIDA,
        EQUIVALENCIA_A_RENDIR,
        LIBRE,
        PROMOCIONADO
    }

    public enum TipoCursadoEnum
    {
        ANUAL = 1,
        SEMESTRAL
    }

    public enum EstadoEmpresaEnum
    {
        GENERADA = 1,
        ACTIVA,
        AUTORIZADA,
        EN_PROCESO_DE_CIERRE,
        CERRADA,
        CERRADA_SIN_VISADO,
        EN_PROCESO_DE_REACTIVACION,
        RECHAZADA,
        EN_PROCESO_DE_CIERRE_AUTORIZADO_NOTIFICADO,
    }

    public enum TipoCooperadoraEnum
    {
        CON_PERSONERIA_JURIDICA = 1,
        SIN_PERSONERIA_JURIDICA,
        NO_TIENE
    }

    public enum AmbitoEscuelaEnum
    {
        URBANO = 1,
        RURAL
    }

    public enum DependenciaEnum
    {
        NACIONAL = 1,
        PROVINCIAL,
        MUNICIPAL
    }

    public enum TipoEducacionEnum
    {
        COMUN = 1,
        ESPECIAL,
        ADULTO,
        ARTISTICA,
        UADM
    }

    //public enum ModalidadJornadaEnum
    //{
    //    EXTENDIDA = 1,
    //    AMPLIADA
    //}

    public enum TipoGestionEnum
    {
        GESTION_ADMINISTRATIVA = 1,
        GESTION_EDUCATIVA,
        ESCUELA
    }

    public enum EnteAutorizaEnum
    {
        DIRECCION_DE_NIVEL = 1,
        APOYO_ADMINISTRATIVO,
        INSPECCION
    }

    public enum EstadoAdHonoremEnum
    {
        ACTIVO = 1,
        INACTIVO
    }

    public enum EstadoAgenteEspecialEnum
    {
        ACTIVO = 1,
        INACTIVO
    }

    public enum EstadoAusentismoEnum
    {
        PENDIENTE_DE_JUSTIFICAR = 1,
        JUSTIFICADA,
        INJUSTIFICADA,
        ELIMINADA
    }

    public enum EstadoPrevisionAusenciaEnum
    {
        ACTIVA = 1,
        CERRADA,
        ELIMINADA,
        AUTORIZADA,
        NO_AUTORIZADA
    }

    public enum EstadoTramiteEnum
    {
        GENERADO = 1,
        VISADO,
        CANCELADO,
        NOTIFICADO,
        ELIMINADO,
        AUTORIZADO,
        NO_AUTORIZADO
    }

    public enum EstadoVacanteEnum
    {
        DISPONIBLE = 1,
        PARA_APROBAR,
        EN_PROCESO_DE_AUTORIZACION,
        RECHAZADO,
        A_PUBLICAR,
        PUBLICADA,
        OTORGADA,
        CUBIERTA,
        NO_CUBIERTA,
        CUBIERTA_ADHONOREM,
        RESERVADA,
        EN_PROCESO_DE_DISTRIBUCION
    }

    //public enum TipoMovimientoActoAdministrativoEnum
    //{
    //    ALTA_EMPRESA = 1,
    //    BAJA_EMPRESA,
    //    MODIFICACION_EMPRESA,
    //    REACTIVACION_EMPRESA,
    //    BAJA_DIVISION_TIPO_CARGO,
    //    ALTA_DIVISION_TIPO_CARGO,
    //    MODIFICACION_TIPO_CARGO,
    //    ALTA_PUESTO_TRABAJO,
    //    NOTA,
    //    BAJA_TIPO_CARGO,
    //    ALTA_TIPO_CARGO
    //}

    public enum ResultadoActoAdministrativoEnum
    {
        APROBADO = 1,
        RECHAZADO,
        CANCELADO
    }

    public enum AccionEnteEnum
    {
        VISADO = 1,
        CONTROL,
        AUTORIZACION
    }

    public enum AccionEnteControlEnum
    {
        CONTROL = 1,
        AUTORIZACION
    }

    public enum ResponsableActoAdministrativoEnum
    {
        MINISTERIO = 1,
        DIRECCION
    }

    public enum RolCargoEnum
    {
        FRENTE_CURSO = 1,
        DIRECTIVO,
        NO_FRENTE_DE_CURSO
    }

    //TODO cargar enumeracion
    public enum ClasePersonaEnum
    {
        ALUMNO = 1,
        ACTOR
    }

    public enum TipoInspeccionEnum
    {
        //GENERAL = 1,
        //INTERMEDIA,
        //ZONAL
        GENERAL = 1,
        ZONAL,
        OTRA
    }

    public enum TipoDocumentacionEnum
    {
        TITULO = 1,
        APTO_PSICOFISICO,
        CONSTANCIA_DE_SERVICIO,
        CERTIFICADO_DE_ESCOLARIDAD,
        CERTIFICADO_DE_CAPACITACION,
        CARTA_MEDICA,
        CERTIFICADO_DE_ASISTENCIA,
        NOTA_DE_TRAMITE,
        EXPEDIENTE,
        RESOLUCION,
        DOCUMENTO,
        RECIBO_SUELDO,
        MAB,
        FORM_DISPONIBILIDAD,
        SOLICITUD_DISPONIBILIDAD,
        DECLARACION_DISPONIBILIDAD,
        DECLARACION_JURADA,
        RESUMEN_REUBICACION,
        CAMBIO_CATEDRA_PERMUTA,
        SOLICITUD_CAMBIO_CATEDRA,
        RESUMEN_PERMUTA_CAMBIO,
        SOLICITUD_PERMUTA,
        SOLICITUD_CONCENTRACION,
        LOM
    }

    public enum TipoActividadEspecialEnum
    {
        REUNION_DE_PERSONAL = 1,
        REUNION_DE_AREA,
        TALLER_DOCENTE,
        ACTO_ACADEMICO
    }

    public enum TipoAusenciaEnum
    {
        RAZONES_PARTICULARES = 1,
        ESTUDIO,
        CAPACITACION,
        VIAJE,
        ENFERMEDAD_DE_3ROS_A_CARGO,
        RAZONES_CONFESIONALES,
        RAZONES_GREMIALES,
        FENOMENOS_GREMIALES,
        FENOMENOS_METEOROLOGICOS,
        DONACION_DE_SANGRE,
        OBLIGACIONES_CIVICO_MILITARES,
        VIDA_EN_LA_NATURALEZA,
        PROGRAMAS_DEPORTIVOS,
        FESTIVIDAD_RELIGIOSA
    }

    public enum TipoNovedadEnum
    {
        ALTA = 1,
        BAJA,
        MOVIMIENTO,
        AUSENTISMO
    }

    public enum TipoAdquisicionEnum
    {
        DONADO = 1,
        COMPRADO,
        EN_PRESTAMO_COMODATO,
        ALQUILADO,
        OTRA_SITUACION
    }

    public enum SituacionLegalEnum
    {
        FISCAL = 1,
        PROFISCAL
    }

    public enum EstadoPredioEnum
    {
        CONSTRUIDO = 1,
        CON_MALEZA,
        CLAUSURADO,
        OTRO
    }

    public enum EstadoEdificioEnum
    {
        EN_USO = 1,
        LIBRE_Y_HABILITADO,
        INHABILITADO,
        OTRO
    }

    public enum EstadoTipoCargoEnum
    {
        VIGENTE = 1,
        NO_VIGENTE,
        EN_CIERRE
    }

    public enum TipoEdificioEnum
    {
        ESCOLAR = 1,
        NO_ESCOLAR
    }

    public enum EstadoCasaHabitacionEnum
    {
        EN_USO = 1,
        LIBRE_Y_HABILITADO,
        INHABILITADO,
        OTRO
    }

    public enum EstadoLocalEnum
    {
        EN_USO = 1,
        LIBRE,
        INHABILITADO,
        EN_CONSTRUCCION,
        OTRO
    }

    public enum PlantaEnum
    {
        SUBSUELO = 1,
        PLANTA_BAJA,
        PRIMER_PISO,
        SEGUNDO_PISO,
        TERCER_PISO,
        CUARTO_PISO,
        QUINTO_PISO,
        SEXTO_PISO,
        SEPTIMO_PISO,
        OCTAVO_PISO,
        NOVENO_PISO,
        DECIMO_PISO,
        DECIMO_PRIMER_PISO,
        DECIMO_SEGUNDO_PISO,
        DECIMO_TERCERO_PISO,
        DECIMO_CUARTO_PISO,
        DECIMO_QUINTO_PISO,
        ALTILLO
    }

    //TODO cargar enumeracion
    public enum EstadoDepositoEnum
    {
        ACTIVO = 1,
        INACTIVO
    }

    public enum EstadoServicioEntidadEnum
    {
        INACTIVO = 1,
        INHABILITADO,
        ACTIVO
    }

    public enum ContratoTipoBienEnum
    {
        EDIFICIO = 1,
        PREDIO,
        CASA_HABITACION,
        DEPOSITO
    }

    public enum TipoContratoEnum
    {
        ALQUILER = 1,
        COMODATO
    }

    public enum EstadoCarreraEnum
    {
        CREADA = 1,
        VIGENTE,
        NO_VIGENTE
    }

    public enum TipoRolUsuarioEnum
    {
        AGENTE = 1,
        ALUMNO
    }

    public enum EmisorInstrumentoLegalEnum
    {
        DIRECCION_GENERAL = 1,
        DIRECCION_REGIONAL,
        INSPECCION_GENERAL,
        MINISTERIO,
        DIPE,
        DEMIS,
        DRE,
        GOBIERNO_NACIONAL,
        GOBIERNO_PROVINCIAL,
        GOBIERNO_MUNICIPAL
    }

    public enum EstadoAsignacionInspeccionEscuelaEnum
    {
        PROVISORIO = 1,
        VIGENTE ,
        NO_VIGENTE
    }

    public enum AccionVisadoActivacionEmpresaEnum
    {
        VISADA = 1,
        AUTORIZAR,
        RECHAZAR
    }

    public enum TipoMovimientoInstrumentoLegalEnum
    {
        ALTA_PREDIO = 1,
        BAJA_PREDIO,
        PRORROGA_CONTRATO,
        ALTA_CONTRATO,
        RESCISION_CONTRATO,
        DIVISION_PREDIO, //5
        UNION_PREDIO,
        AMPLIACION_PREDIO,
        REDUCCION_PREDIO,
        UNION_EDIFICIO,
        DIVISION_EDIFICIO, //10
        DONADO,
        COMPRADO,
        EN_PRESTAMO_O_COMODATO,
        ALQUILADO,
        OTRA_SITUACION, //15
        ALTA_EMPRESA,
        CIERRE_EMPRESA,
        MODIFICACION_EMPRESA,
        REAPERTURA_EMPRESA,
        ALTA_DIVISION, //20
        BAJA_DIVISION,
        MODIFICACION_DIVISION,
        ALTA_TIPO_CARGO,
        BAJA_TIPO_CARGO,
        REACTIVACION_DE_TIPO_DE_CARGO, //25
        ALTA_PUESTO_TRABAJO,
        NOTA,
        ALTA_PLAN_DE_ESTUDIO,
        MODIFICACION_PLAN_DE_ESTUDIO,
        RESOLUCIONES_DE_EQUIVALENCIAS, //30        
        ALTA_SISTEMA_DE_NOTAS,
        MODIFICACION_SISTEMA_DE_NOTAS,
        ALTA_CALENDARIO_ESCOLAR,
        MODIFICACION_CALENDARIO_ESCOLAR,
        ALTA_ASIGNATURA_DEFINIDA_POR_UNA_ESCUELA, //35
        MODIFICACION_ASIGNATURA_DEFINIDA_POR_UNA_ESCUELA,
        ALTA_PROGRAMA,
        MAB_ALTA,
        MAB_MOVIMIENTO,
        MAB_BAJA, //40     
        MAB_AUSENTISMO,
        MAB_HORAS_INSTITUCIONALES,
        ALTA_ASIGNACION_PLAN_DE_ESTUDIO_A_ESCUELA,
        MODIFICACION_ASIGNACION_PLAN_DE_ESTUDIO_A_ESCUELA,
        BAJA_ASIGNACION_PLAN_DE_ESTUDIO_A_ESCUELA, //45
        ESCUELA_ZONA_DESFAVORABLE
    }

    public enum TipoMovimientoDetalleAsignacionEnum
    {
        MAB = 1,
        LICENCIA,
        PUESTO_PROVISORIO,
        RETORNO_A_ACTIVIDADES
    }

    public enum TipoPuestoProvisorioEnum
    {
        ALTA_DOCENTE_INTEGRADOR = 1,
        PASE_DE_OTRAS_JURISDICCIONES_O_MINISTERIOS,
        COMPLETA_HORAS,
        TAREAS_PASIVAS,
    }

    //public enum AgrupamientoCargoEnum
    //{
    //    DOCENTE = 1,
    //    NO_DOCENTE
    //}

    //public enum NivelCargoEnum
    //{
    //    PRIMERA = 1,
    //    SEGUNDA
    //}

    //  Para obtener el valor actual de un parametro, usar ParametroRules, el metodo GetValorParametroVigente.
    public enum ParametroEnum
    {
        CANTIDAD_ASIGNATURAS_ADEUDADAS = 1,
        CANTIDAD_DE_AUSENCIAS_ACUMULADAS_JUSTIFICADAS_POR_RAZONES_PERSONALES_MENSUALES_PARA_DOCENTES,
        CANTIDAD_DE_AUSENCIAS_ACUMULADAS_JUSTIFICADAS_POR_RAZONES_PERSONALES_ANUALES_PARA_DOCENTES,
        CANTIDAD_DE_AUSENCIAS_ACUMULADAS_JUSTIFICADAS_POR_RAZONES_PERSONALES_MENSUALES_PARA_EMPLEADOS_PÚBLICOS,
        CANTIDAD_DE_AUSENCIAS_ACUMULADAS_JUSTIFICADAS_POR_RAZONES_PERSONALES_ANUALES_PARA_EMPLEADOS_PÚBLICOS,
        FALTA_UNO, //TODO: ESTA ESTÁ DE RELLENO, SACAR!!
        CANTIDAD_DE_RETRASOS_JUSTIFICADOS_POR_EMPRESA_POR_AÑO_LECTIVO,
        CANTIDAD_MÁXIMA_DE_AMONESTACIONES,//8
        CANTIDAD_MÁXIMA_DE_HORAS_SEMANALES_MEDIA,
        CANTIDAD_MÁXIMA_DE_HORAS_SEMANALES_PRIMARIO, //10
        CANTIDAD_MÁXIMA_DE_HORAS_SEMANALES_SUPERIOR,
        CANTIDAD_MÍNIMA_DE_HORAS_SEMANALES_MEDIA,
        CANTIDAD_MÍNIMA_DE_HORAS_SEMANALES_PRIMARIO,
        CANTIDAD_MÍNIMA_DE_HORAS_SEMANALES_SUPERIOR,
        CIERRE_DE_EMPRESA_CON_AUTORIZACIÓN, //15
        DESVINCULACIÓN_DE_EDIFICIO_EN_CIERRE_DE_EMPRESA,
        EMAIL_ACTIVACIÓN_DE_EMPRESA,
        EMAIL_AUTORIZACIÓN_REACTIVACIÓN_DE_EMPRESA,
        EMAIL_AUTORIZACIÓN_SOLICITUD_DE_CREACIÓN_PUESTO_DE_TRABAJO,
        EMAIL_AUTORIZACIÓN_SUPERIOR_DE_AMPLIACIÓN_ESTRUCTURA_DE_EMPRESA, //20
        EMAIL_CIERRE_DE_EMPRESA,
        EMAIL_VIGENCIA_TIPO_DE_CARGO,
        ESTRUCTURA_ESCOLAR_EN_CREACIÓN_EMPRESA,
        ESTRUCTURA_ESCOLAR_EN_REACTIVACIÓN_EMPRESA,
        GENERAR_PAQUETE_NO_PRESUPUESTADO_EN_AMPLIACIÓN, //25
        GENERAR_PAQUETE_NO_PRESUPUESTADO_EN_CAMBIO_DE_PLAN_DE_ESTUDIOS,
        GENERAR_PAQUETE_NO_PRESUPUESTADO_EN_CREACIÓN_DE_EMPRESA,
        INASISTENCIAS_PRIMERA_REINCORPORACIÓN,
        INASISTENCIAS_SEGUNDA_REINCORPORACIÓN,
        INASISTENCIAS_TERCERA_REINCORPORACIÓN, //30
        INGRESO_DE_FECHAS_EN_CONFECCIÓN_DE_MAB,
        INSTRUMENTO_LEGAL_EN_CREACIÓN_EMPRESA,
        INSTRUMENTO_LEGAL_EN_REACTIVACIÓN_EMPRESA,
        JERARQUÍA_DE_INSPECCIÓN_IGUAL_A_ORGANIGRAMA,
        MODIFICAR_NOMBRE_SUGERIDO_DE_EMPRESA, //35
        PAQUETE_PRESUPUESTADO_EN_CREACIÓN_EMPRESA,
        PLAN_ESTUDIO_EN_CREACIÓN_EMPRESA,
        PLAN_ESTUDIO_EN_REACTIVACIÓN_DE_EMPRESA,
        PORCENTAJE_DOCENTES_PRESENTES_POR_EXPULSIÓN,
        PORCENTAJE_DOCENTES_PRESENTES_POR_MÁXIMO_DE_AMONESTACIONES, //40
        PORCENTAJE_DOCENTES_PRESENTES_POR_SUSPENSIÓN,
        SOLO_VISADO_EN_CREACIÓN_O_REACTIVACIÓN_DE_EMPRESA,
        TIEMPO_CANCELACIÓN_DE_TRÁMITE_AGENTE,
        TIEMPO_PERMITIDO_PARA_ELIMINACIÓN_DE_UN_RETRASO,
        TIEMPO_SOLICITAR_AUTORIZACIÓN_SUPERIOR, //45
        VENCIMIENTO_DE_REGULARIDAD_POR_AÑOS,
        VENCIMIENTO_DE_REGULARIDAD_POR_TURNO,
        CANTIDAD_DE_HORAS_PERMITIDAS_PARA_INCREMENTAR,
        CANTIDAD_DE_HORAS_PARA_MODIFICACIÓN_DE_PEDIDO_DE_LICENCIA,
        VALIDA_PREINSCRIPCIÓN, //50
        VALIDA_SITUACIÓN_ACADÉMICA_ANTERIOR,
        RESPETA_RÉGIMEN_DE_CORRELATIVIDADES,
        CANTIDAD_DÍAS_ALERTA_VENCIMIENTO_CONTRATO_ALQUILER_COMODATO,
        CANTIDAD_DE_DÍAS_ANTERIORES_PERMITIDOS_PARA_INGRESO_DE_SUSPENSIÓN_DE_ACTIVIDAD,
        CANTIDAD_DE_INASISTENCIAS_A_COMPUTAR_POR_AUSENCIA_A_ACTIVIDAD_ESPECIAL_CON_MOTIVO_PARO, //55
        CANTIDAD_DE_INASISTENCIAS_A_COMPUTAR_POR_AUSENCIA_A_ACTIVIDAD_ESPECIAL_CON_MOTIVO_PEDIDO_DE_LICENCIA,
        CANTIDAD_DE_INASISTENCIAS_A_COMPUTAR_POR_AUSENCIA_A_ACTIVIDAD_ESPECIAL_CON_MOTIVO_CARPETA_MEDICA,
        CANTIDAD_DE_INASISTENCIAS_A_COMPUTAR_POR_AUSENCIA_A_ACTIVIDAD_ESPECIAL_CON_MOTIVO_RAZONES_PARTICULARES,
        CANTIDAD_DE_INASISTENCIAS_A_COMPUTAR_POR_AUSENCIA_A_ACTIVIDAD_ESPECIAL_CON_MOTIVO_ACCIDENTE_LABORAL,
        CANTIDAD_DE_DIAS_PERMITIDOS_PARA_REGISTRAR_ASISTENCIA_DE_DOCENTE_A_ACTIVIDAD_ESPECIAL, //60
        CANTIDAD_MÁXIMA_DE_MINUTOS_PERMITIDOS_POR_RETRASO,
        INASISTENCIA_ASIGNATURA_ESPECIAL,
        INASISTENCIA_LLEGADA_TARDE,
        CANTIDAD_DE_INASISTENCIAS_ACUMULADAS_INJUSTIFICADAS_POR_RAZONES_PERSONALES_ANUALES_PARA_DOCENTES_CON_CARGO,
        CANTIDAD_DE_INASISTENCIAS_ACUMULADAS_INJUSTIFICADAS_POR_RAZONES_PERSONALES_ANUALES_PARA_NO_DOCENTES_CON_PUESTO_EN_ESCUELA, //65
        CANTIDAD_DE_OBLIGACIONES_AUSENTE_ACUMULADAS_INJUSTIFICADAS_POR_RAZONES_PERSONALES_ANUALES_PARA_DOCENTES_CON_HORAS_CÁTEDRA
    }

    public enum TipoDatoEnum
    {
        //ENTERO = 1,
        NUMÉRICO = 1,
        DECIMAL,
        FECHA,
        TEXTO,
        BOOLEAN,
        EMAIL
    }
    public enum FuncionEdificioEnum
    {
        CASA_HABITACION = 1
        //...
    }
    public enum TipoOperacionPredioEnum
    {
        UNION = 1,
        DIVISION,
        AMPLIACION,
        REDUCCION
    }

    /// <summary>
    /// a) Planta funcional: Planta Provisoria vinculada + Planta provisoria no vinculada + Planta Presupuestaria asignada + Planta no presupuestaria liquidada
    /// b) Planta externa
    /// </summary>
    public enum TipoPuestoEnum
    {
        /// <summary>
        /// Representa los puestos de trabajo liquidados por el Ministerio de Educación y puestos de trabajo que están asignados pero no liquidados.
        /// </summary>
        ASIGNADO = 1,
        /// <summary>
        /// Representa los puestos de trabajo que pertenecen a la planta funcional pero no se liquidan por el presupuesto de esa empresa, 
        /// es decir que pertenecen a la planta presupuestaria asignada de otra escuela o que proviene de otras jurisdicciones u otros ministerios 
        /// y que no son pagados por el Ministerio de Educación.
        /// </summary>
        PROVISORIO_VINCULADO,
        /// <summary>
        /// Representa a los puestos designados para docentes integradoras.
        /// </summary>
        PROVISORIO_NO_VINCULADO,
        /// <summary>
        /// Representa los puestos de trabajo en la escuela, no docentes, no pagos por el ministerio de educación: 
        /// auxiliares, cooperativas, cooperadora, Paicor, personal de limpieza.
        /// </summary>
        EXTERNO
    }

    public enum TipoOperacionEdificioEnum
    {
        UNION = 1,
        DIVISION
    }

    public enum TipoBienStockEnum
    {
        BIENES_MUEBLES = 1,
        MATERIAL_BIBLIOGRAFICO,
        OBRAS_DE_ARTE,
        SEMOVIENTES,
        INFORMATICA,
        AUTOMOTORES,
        EMBARCACIONES,
        AERONAVES,
        BIENES_NO_INVENTARIABLES
    }

    public enum EstadoItemStockEnum
    {
        DISPONIBLE = 1,
        NO_DISPONIBLE
    }

    public enum EstadoPedidoCierreEnum
    {
        GENERADO = 1,
        APROBADO,
        RECHAZADO
    }

    public enum ZonaDesfavorableEnum
    {
        A = 1,
        B,
        C,
        D,
        E,
        F,
        G
    }

    public enum TipoSubGrupoEnum
    {
        OBLIGATORIA = 1,
        DEFINICION_INSTITUCIONAL,
        OPCIONAL
    }

    public enum EstadoVinculoEmpresaEdificioEnum
    {
        ACTIVO = 1,
        INACTIVO,
        PROVISORIO
    }

    public enum EstadoPuestoDeTrabajoEnum
    {
        GENERADO = 1,
        AUTORIZADO,
        VACANTE,
        ACTIVO,
        A_ASIGNAR_POR_CAMBIO_DE_PLAN,
        RECHAZADO,
        EN_CIERRE_CON_GOCE_DE_SUELDO,
        EN_CIERRE_CON_GOCE_DE_SUELDO_CADUCADO,
        PARA_RESERVA,
        RETENIDO,
        CERRADO
    }

    public enum EstadoSolicitudCreacionPuestoDeTrabajoEnum
    {
        GENERADA = 1,
        AUTORIZADA,
        RECHAZADA,
        CERRADA
    }

    public enum MotivoBajaConfiguracionTurnoEnum
    {
        BAJA = 1
    }

    public enum TipoAtributoEnum
    {
        PROVISORIO = 1,
    }

    public enum MotivoBajaEnum
    {
        PROVISORIO = 1,
    }

    public enum TurnoEnum
    {
        MAÑANA=1,
        TARDE,
        VESPERTINO,
        NOCHE

    }

    public enum TipoEstatutoEnum
    {
        PROVISORIO = 1,
    }

    public enum TipoSiniestroEnum
    {
        ACCIDENTE_TRABAJO = 1,
        REAGRAVACION,
        ENFERMEDAD_PROFESIONAL

    }

    public enum MotivoBajaFuncEdificioEnum
    {
        PROVISORIO = 1,
    }

    public enum MotivoRetrasoEnum
    {
        ESTADO1 = 1,
        ESTADO2
    }

    public enum MotivoTipoEstruturaEdiliciaEnum
    {
        PROVISORIO = 1,
    }

    public enum MotivoTipoLocalEnum
    {
        PROVISORIO = 1,

    }

    public enum EstadoRetrasoEnum
    {
        ACTIVO = 1,
        ELIMINADO,
        CONTABILIZADO_INASISTENCIA
    }

    public enum NivelEducativoNombreEnum
    {
        INICIAL = 1,
        PRIMARIO,
        MEDIO,
        SUPERIOR
    }

    public enum TipoComunicacionEnum
    {
        TELEFONO_PRINCIPAL = 1,
        FAX_PRINCIPAL,
        TELEFONO_SECUNDARIO,
        FAX_SECUNDARIO,
        TELEFONO_ALTERNATIVO,
        FAX_ALTERNATIVO,
        TELEFONO_CELULAR,
        TELEFONO_PARTICULAR,
        FAX_PARTICULAR,
        TELEFONO_PARA_MENSAJES,
        DIRECCION_DE_CORREO,
        PAGINA_WEB,
        CORREO_ALTERNATIVO,
        TELEFONO_LABORAL,
        TELEFONO_INTERNO
    }

    public enum OrigenEnum
    {
        T_PER_FISICA = 1,
        T_PER_JURIDICA,
        T_IN_PREDIOS,
        T_DEPOSITOS,
        T_DO_SUC_BANCARIA,
        T_EM_HIST_EMPRESA,
        T_IN_EDIFICIOS,
        T_DO_EMP_EXTERNA,
        T_EM_EMPRESAS
    }

    public enum SexoEnum
    {
        [Description("00")]
        INSTITUCION = 0,
        [Description("01")]
        MASCULINO,
        [Description("02")]
        FEMENINO,
        [Description("03")]
        INDEFINIDO
    }

    public enum NoSiEnum
    {
        NO = 0,
        SI
    }

    public enum TipoAgenteEnum
    {
        DOCENTE = 1,
        NO_DOCENTE,
        ESPECIAL
    }

    public enum TipoEmpresaFiltroBusquedaEnum
    {
        TODAS = 0,
        ESCUELA_MADRE,
        ESCUELA_ANEXO,
        INSPECCION,
        DIRECCION_DE_NIVEL,
        DIRECCION_DE_INFRAESTRUCTURA,
        DIRECCION_DE_RECURSOS_HUMANOS,
        DIRECCION_DE_SISTEMAS,
        DIRECCION_DE_TESORERIA,
        SECRETARIA,
        MINISTERIO,
        ESCUELA_MADRE_RAIZ,
        INSPECCION_QUE_PERTENECE_A_DIRECCION_DE_NIVEL_DEL_USUARIO_ACTUAL,
        INSPECCION_NO_ZONAL_QUE_PERTENECE_A_DIRECCION_DE_NIVEL_DEL_USUARIO_ACTUAL,
        INSPECCION_ZONAL,
        DIRECCION_NIVEL_USUARIO_LOGUEADO
    }

    public enum OpcionEnvioMailEnum
    {
        ACTIVACION = 0,
        PEDIDO_CIERRE,
        CIERRE,
        REACTIVACION
    }

    public enum UsoCodigoMovMabEnum
    {
        PROVISORIO = 1

    }

    public enum TipoGrupoMabEnum
    {
        ALTA = 1,
        MOVIMIENTO,
        BAJA,
        AUSENTISMO
    }

    public enum UsoCodigoMovimientoMabEnum
    {
        USO1 = 1
    }

    public enum MotivoAltaPuestoTrabajoEnum
    {
        AMPLICACION_DE_EMPRESA = 1,
        CREACION_DE_EMPRESA,
        CAMBIO_DE_PLAN_DE_ESTUDIOS
    }

    public enum TipoTituloEnum
    {
        PROVISORIO = 1,  //no definida en analisis.
    }

    public enum EstadoAsignacionEnum
    {
        ACTIVA = 1,
        INACTIVA,
        CERRADA,
        PENDIENTE_DE_MAB
    }

    public enum EstadoInasistenciaDocenteEnum
    {
        INJUSTIFICADA = 1,
        PENDIENTE_DE_JUSTIFICAR,
        JUSTIFICADA,
        NO_COMPUTABLE
    }

    public enum TipoMotivoInasistenciaDocenteEnum
    {
        RAZONES_PARTICULARES = 1,
        PARO,
        CARPETA_MEDICA,
        ACCIDENTE_LABORAL,
        PEDIDO_DE_LICENCIA
    }

    public enum EstadoHistorialAcademicoEnum
    {
        INSCRIPTO = 1,
        COMPLEMENTARIO,
        COLOQUIO,
        APROBADA,
        NO_APROBADO_PREVIO_LIBRE,
        NO_APROBADO_PREVIO_REGULAR
    }

    public enum EstadoSolicitudAmpliacionEstructuraEmpresa
    {
        GENERADA = 1,
        AUTORIZADA,
        RECHAZADA,
        AUTORIZADA_EN_INSTANCIA_SUPERIOR,
        EN_AUTORIZACION_DE_INSTANCIA_SUPERIOR

    }

    public enum TipoParametroIngEscolarEnum
    {
        INGRESO_PRUEBA_APTITUD = 1,
        INGRESO_SORTEO
    }

    public enum TipoSistemaNota
    {
        CUALITATIVO = 1,
        CUANTITATIVO
    }
    
    public enum EstadoPreinscripcionEnum
    {
        CREADA = 1,
        FAVORECIDO,
        NO_FAVORECIDO,
        REDIRECCIONADO,
        NO_REDIRECCIONADO,
        CERRADA,
        BAJA
    }

    public enum EstadoMatriculaEnum
    {
        PROVISORIA = 1,
        DEFINITIVA,
        BAJA,
        CERRADA
    }

    public enum EstadoPedidoLicenciaEnum
    {
        GENERADA = 1,
        VERIFICADA,
        NOTIFICADA,
        ELIMINADA,
        CON_RESOLUCION
    }

    public enum ReincorporacionEnum
    {
        PRIMERA=1,
        SEGUNDA,
        TERCERA
    }

    public enum TipoEventoEscolarEnum
    {
        FERIADO = 0,
        ACTIVIDAD_ESPECIAL,
        SUSPENSION_ACTIVIDAD
    }

    public enum MesesEnum
    {
        ENERO = 1,
        FEBRERO,
        MARZO,
        ABRIL,
        MAYO,
        JUNIO,
        JULIO,
        AGOSTO,
        SEPTIEMBRE,
        OCTUBRE,
        NOVIEMBRE,
        DICIEMBRE
    }

    public enum EtapasEnum
    {
        ETAPA_1 = 1,
        ETAPA_2,
        ETAPA_3
    }

    // Listado de keys a utilizar para la carga de ViewData/TempData.
    // Respetar el orden alfabético!!!
    public enum ViewDataKey
    {
        ACCION_VISADO,
        AGENTE,
        ASIGNATURA,
        AGRUPAMIENTO_CARGO,
        ANIO,
        CALLE,
        CALLE_DOMICILIO,
        CARRERA,
        CICLO_EDCUCATIVO,
        CICLO_LECTIVO,
        CODIGO_MOVIMIENTO_MAB,
        COMBO_VACIO,
        CONDICION_IVA,
        DEPARTAMENTO_PROVINCIAL,
        DIRECCION_NIVEL,
        ES_SUPERIOR,
        ESTADO_ASIGNACION,
        ESTADO_CIVIL,
        ESTADO_PUESTO,
        ETAPA,
        FUNCION_EDIFICIO,
        GRADO_ANIO,
        GRUPO,
        INSPECCION_INTERMEDIA,
        ID_EMPRESA_USUARIO_LOGUEADO,
        JERARQUIA_DE_INSPECCION_IGUAL_A_ORGANIGRAMA,
        LOCALIDAD,
        MODALIDAD_JORNADA,
        MODALIDAD_MAB,
        MOTIVO_BAJA,
        MOTIVO_BAJA_SANCION,
        MOTIVO_BAJA_AGENTE,
        MOTIVO_INCORPORACION,
        NIVEL_EDUCATIVO,
        NIVEL_EDUCATIVO_ALL,
        NIVEL_CARGO,
        NOMBRE_EMPRESA_HABILITADO,
        OBRA_SOCIAL,
        ORDEN_DE_PAGO,
        ORGANISMO_EMISOR,
        ORIENTACION,
        PAIS,
        PERIODO_LECTIVO,
        PROCESO,
        PROGRAMA_PRESUPUESTARIO,
        PROVINCIA,
        SEXO,
        SITUACION_REVISTA,
        SUBGRUPO,
        SUBORIENTACION,
        SUCURSAL_BANCARIA,
        TIPO_CALLE,
        TIPO_CARGO,
        TIPO_CARGO_ESPECIAL,
        TIPO_CARGO_FRENTE_CURSO,
        TIPO_DOCUMENTO,
        TIPO_EDUCACION,
        TIPO_EMPRESA,
        TIPO_ESCUELA,
        TIPO_ESCUELA_PERMITIDA_EMPRESA,
        TIPO_ESTRUCTURA_EDILICIA,
        TIPO_INSPECCION,
        TIPO_INSPECCION_INTERMEDIA,
        TIPO_INSTRUMENTO_LEGAL,
        TIPO_JORNADA,
        TIPO_LOCAL,
        TIPO_NOVEDAD_MAB,
        TIPO_VINCULO,
        TIPO_SANCION,
        TIPO_SANCION_ALL,
        TITULO,
        TURNO,
        ZONA_DESFAVORABLE,
        TIPO_EMPRESA_USUARIO_LOGUEADO
    }
}