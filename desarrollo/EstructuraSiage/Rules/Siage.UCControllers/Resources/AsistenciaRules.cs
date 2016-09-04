using System;
using System.Collections.Generic;
using System.Linq;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using Siage.Core.DaoInterfaces;
using Microsoft.Practices.ServiceLocation;
using AutoMapper;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.UCControllers.Rules
{
    public class AsistenciaRules : IAsistenciaRules
    {
        #region Atributos

        private IDaoProvider _daoProvider;
        private IDaoInscripcion _daoInscripcion;
        private IDaoInasistencia _daoInasistencia;
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

        private IDaoInscripcion DaoInscripcion
        {
            get
            {
                if (_daoInscripcion == null)
                {
                    _daoInscripcion = DaoProvider.GetDaoInscripcion();
                }
                return _daoInscripcion;
            }
        }

        private IDaoInasistencia DaoInasistencia
        {
            get
            {
                if (_daoInasistencia == null)
                {
                    _daoInasistencia = DaoProvider.GetDaoInasistencia();
                }
                return _daoInasistencia;
            }
        }

        #endregion

        #region IAsistenciaRules Members

        public List<InasistenciaRegistrarModel> GetAsistenciasByFiltros(int idEscuela, DateTime Fecha, int Turno, int Grado, DivisionEnum Division, int? Asignatura)
        {
            //Todo Vanesa: la asignatura especial no se puede buscar, le pregunte a Martin Pollioto.
            var lista = DaoInasistencia.GetByDiagramacionCurso(idEscuela, Fecha, Turno, Grado, Division);
            if (lista!= null)
                return Mapper.Map<List<Inasistencia>, List<InasistenciaRegistrarModel>>(lista);
            return null;
            //Si no tiene cargada la asistencia tengo que buscar los alumnos inscriptos
        }
        public InasistenciaRegistrarModel AsistenciaSave(InasistenciaRegistrarModel modelo)
        {
            if (modelo == null)
                throw new ApplicationException(Resources.Asistencia.Modelo);

            //3. El sistema verifica que la fecha sea menor o igual a la actual y que esté dentro del período de clases habilitado por el calendario escolar y es así. 
            //10. El sistema registra la asistencia por estudiante inscripto, para la fecha ingresada y grado/año/turno/división seleccionada.
            return modelo;
        }

        public EstudianteModel VerificarInasistencias()
        {
//            11.A. El usuario logueado es de nivel medio.
//11.A.1. Para cada estudiante ausente, el sistema verifica si posee condición especial de inasistencia y no posee.
//   11.A.1.A. El estudiante posee condición especial de inasistencia.
//   11.A.1.A.2. Fin del caso de uso.
//11.A.2. El sistema contabiliza el número de inasistencias 
//TotalInasistencias=InasistenciasJustificadas+ InasistenciasNoJustificadas. 
//        11.A.3. El sistema verifica si el estudiante no supera el límite de inasistencias Nivel Medio (Ver observaciones) por parámetro y es así.
//11.A.3.A. El estudiante supera el límite de inasistencias establecido en el parámetro.
//11.A.3.A.1. El sistema informa la situación.
            return null;
        }


        public List<AsignaturaModel> GetAsignaturasEspecialesByDiagramacionCurso(int idGradoAnio, int idTurno, int idDivision, int idEscuela)
        {
//            pero lo hagamos mas fácil...vos tenes en la vista una diagramación curso (4to C Mañana) vas con ese ID a la clase ConfiguracionAsignaturaEspecial y ves si existe dicha diagramación en alguna instancia de esa clase
//de ser así recuperas la unidadAcademica asociada y tomas la asignatura


//Vanesa: y como consigo la asign? por detalle subgrupo o por detalle asignatura? son excluyentes
//en el caso de Asig. Especial cual es el camino?


//Martin: por detalle de asignatura corazón
            return null;
        }
        #endregion
    }
}
