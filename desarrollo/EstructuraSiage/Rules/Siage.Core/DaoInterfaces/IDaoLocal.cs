using Siage.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; using Siage.Base;
using Siage.Core.Domain;


namespace Siage.Core.DaoInterfaces
{
    public interface IDaoLocal : IDao<Local, int>
    {
        List<Local> GetByFiltros(string nombre, int? idTipoLocal, int? numeroLocal, int? idEdificio, bool? bCasaHabitacion, bool bInlucirDadosDeBaja = false);
        /// <summary>
        /// Valida que no exista un local con el mismo nombre y número que el local actual.
        /// </summary>
        /// <param name="nombre">Nombre del local</param>
        /// <param name="numero">Número del local</param>
        /// <param name="idLocal">Id del local actual</param>
        /// <returns>True: no existe un local registrado con el mismo nombre y número. False: existe un local registrado con el mismo nombre y número.</returns>
        bool VerificarDatosLocal(string nombre, int? numero,int idLocal);
         //<summary>
         //Valida que el local actual no sea el último local asociado a la casa habitación del edificio al que pertenece.
         //</summary>
         //<param name="idLocal">Id del local actual</param>
         //<param name="idEdificioLocal">Id del edificio al cual pertenece el local</param>
         //<returns>True: no es el último local asociado a la casa habitación. False: es el último local asociado a la casa habitación.</returns>
        bool VerificarCasaHabitacion(int idLocal,int idEdificioLocal);
    }
}
