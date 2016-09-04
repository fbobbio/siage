using Siage.Core.Domain;

namespace Siage.Core.DaoInterfaces
{
    /** 
    * <summary> Interfaz IDaoOrientacionSubOrientacionYEspecialidad
    *	
    * </summary>
    * <remarks>
    *		Autor: gabriel
    *		Fecha: 7/1/2011 2:54:06 PM
    * </remarks>
    */

    public interface IDaoOrientacionSubOrientacionYEspecialidad : IDao<OrientacionSubOrientacionYEspecialidad, int>
    {
        #region Constantes
        #endregion

        #region Métodos

        OrientacionSubOrientacionYEspecialidad GetByIds(int orientacionId, int suborientacionId, int especialidadId);

        #endregion
    }
}
