using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using Siage.Base;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoHistorialDocumento : IDao<HistorialDocumento, int>
    {

        List<HistorialDocumento> GetByEstudiante(string dniEstudiante);
        List<HistorialDocumento> GetHistorialDocumentoByEstudianteYProceso(int idEstudiante, ProcesoEnum proceso, int? idEscuela);
    }
}

