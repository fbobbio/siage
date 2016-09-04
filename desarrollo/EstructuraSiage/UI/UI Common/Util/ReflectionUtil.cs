using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAGE.UI_Common.Util
{
    public class ReflectionUtil
    {
        public static TEntity GetById<TEntity>(object rule, int id)
        {
            string nombreEntidad = typeof(TEntity).Name.Replace("Model", string.Empty);
            string nombreMetodo = String.Format("Get{0}ById", nombreEntidad);

            var metodo = rule.GetType().GetMethod(nombreMetodo);
            var valor = metodo.Invoke(rule, new object[] { id });

            return (TEntity)valor;
        }
    }
}