using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siage.Base
{
    public class ReflectionUtil
    {
        /// <summary>
        /// Compara dos objetos del mismo tipo y retorna true si los objetos son iguales, o false si el objeto nuevo ha sido modificado
        /// </summary>
        /// <typeparam name="TAssembly">Tipo de dato que se compara (solo sirve para comparar si pertenecen al mismo Assembly)</typeparam>
        /// <param name="objetoViejo"></param>
        /// <param name="objetoNuevo"></param>
        /// <returns>Retorna true si los objetos son iguales</returns>
        public static bool CompararObjetos<TAssembly>(object objetoViejo, object objetoNuevo)
        {
            var propiedades = objetoViejo.GetType().GetProperties();
            foreach (var prop in propiedades)
            {
                var valorViejo = prop.GetValue(objetoViejo, null);

                var propNueva = objetoNuevo.GetType().GetProperty(prop.Name);
                if(propNueva == null)
                    continue;

                var valorNuevo = propNueva.GetValue(objetoNuevo, null);

                if (valorNuevo != null && valorViejo != null)
                {
                    if (prop.PropertyType.Assembly == typeof(TAssembly).Assembly) 
                    {
                        if (!CompararObjetos<TAssembly>(valorViejo, valorNuevo))
                            return false;
                    }
                    else if (!valorViejo.Equals(valorNuevo)) 
                    {
                        return false;
                    }
                }
                else if(!(valorNuevo == null && valorViejo == null))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
