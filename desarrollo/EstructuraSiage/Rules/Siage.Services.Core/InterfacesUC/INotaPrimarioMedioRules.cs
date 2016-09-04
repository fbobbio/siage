using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Base;
using Siage.Services.Core.Models;

namespace Siage.Services.Core.InterfacesUC
{
    public interface INotaPrimarioMedioRules
    {
        void Registrar(NotaPrimarioMedioModel model);
        List<InscripcionNotasModel> GetInscripciones(int idEscuela, string filtroAnio, string filtroTurno, string filtroDivision);
        List<CodigoAsignaturaModel> GetAsignaturas(int idEscuela, string filtroAnio, string filtroTurno, string filtroDivision);
        bool ValidarEtapa(EtapasEnum etapaEnum, string gradoAnio);
        SistemaNotaModel GetSistemaNotaByAsignatura(int asignatura);

        SistemaNotaModel GetSistemaNotaById(int id);

        List<InscripcionNotasModel> GenerarNotaDeEtapa(int idEscuela, string filtroAnio, string filtroTurno,
                                                       string filtroDivision, string asignatura, string filtroEtapa);

        void RegistrarNotaEtapa(NotaPrimarioMedioModel model);
    }
}
