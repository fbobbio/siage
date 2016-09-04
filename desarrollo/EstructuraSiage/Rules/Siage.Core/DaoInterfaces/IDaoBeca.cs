using Siage.Core.Domain;
using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDaoBeca : IDao<Beca, int>
    {
        List<Beca> GetByEstudiante(string dniEstudiante);
    }
}