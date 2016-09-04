using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Siage.Base
{
    public static class JSONSerializationHelper
    {
        public static string SerializeDataContract(object data)
        {
            return SerializeDataContract(data, string.Empty);
        }

        /// <summary>
        /// Sirve para controlar que no se produzcan bucles infinitos en métodos recursivos
        /// </summary>
        private static int _profundidadMaximaNavegacion = 3;

        public static string SerializeDataContract(object data, string excludedProperties)
        {
            var ms = new MemoryStream();

            if (data.GetType().Name.Contains("IList") || data.GetType().Name.Contains("List`1"))
            {
                return ProcesarLista(data, excludedProperties, 0, null, data.GetType());
            }
            return ProcesarObjeto(data, excludedProperties, 0, null, null);
            //var ser = new DataContractJsonSerializer(data.GetType());
            //ser.WriteObject(ms, data);
            //var json = Encoding.Default.GetString(ms.ToArray());
            //return json;
        }

        #region Métodos de procesamiento de Listas/Objetos/Propiedades

        //Excluse las propiedades de un OBJETO
        private static string ProcesarObjeto(object data, string excludedProperties, int profundidadActual, Type claseActual, Type clasePadre)
        {
            if (data == null)
                return string.Empty;
            else if (EsPrimitivo(data.GetType()))
            {
                return data.ToString();
            }

            string[] propiedadesExcluidas = excludedProperties.Split(',');
            profundidadActual++;

            if (claseActual == null)
                claseActual = data.GetType();

            StringBuilder resultado = new StringBuilder();
            resultado.AppendLine("{");
            //resultado.Append(String.Format("{0}=", claseActual.Name));
            foreach (var propiedad in claseActual.GetProperties())
            {
                Type claseActualProcesada = propiedad.PropertyType;

                if (claseActualProcesada == clasePadre)
                {
                    resultado.Append(ExcluirPropiedad(data, propiedad, "REFERENCIA_A_CLASE_PADRE"));
                }

                // si es byte la excluye de una.
                else if (propiedad.PropertyType == typeof(byte[]) || propiedad.PropertyType == typeof(Byte[]))
                {
                    resultado.Append(ExcluirPropiedad(data, propiedad));
                }
                //Excluye propiedades por nombre
                else if (propiedadesExcluidas.Contains(propiedad.Name))
                    resultado.Append(ExcluirPropiedad(data, propiedad));

                else if (EsPrimitivo(propiedad.PropertyType))
                {
                    resultado.Append(String.Format("{0}:'{1}';", propiedad.Name, propiedad.GetValue(data, null)));
                }
                else if (profundidadActual > _profundidadMaximaNavegacion)
                {
                    continue;
                }
                //Procesa las propiedades que corresponden con una Lista de objetos
                else if ((claseActualProcesada.Name.Contains("IList") || claseActualProcesada.Name.Contains("List`1")))
                {
                    var dataInterna = propiedad.GetValue(data, null);
                    resultado.Append(ProcesarLista(dataInterna, excludedProperties, profundidadActual, propiedad, data.GetType()));
                }

                //Procesa las propiedades que corresponden con un objeto 
                else if (EsPropiedadProcesable(propiedad))
                {
                    var dataInterna = propiedad.GetValue(data, null);
                    var valor = String.Format("{0}:{1};", propiedad.Name, ProcesarObjeto(dataInterna, excludedProperties, profundidadActual, claseActualProcesada, data.GetType()));
                    resultado.Append(valor);
                }
            }

            profundidadActual--;
            resultado.AppendLine("}");
            return resultado.ToString();
        }

        private static bool EsPrimitivo(Type dataType)
        {
            var type = Type.GetTypeCode(dataType);
            foreach (TypeCode enumValue in Enum.GetValues(typeof(TypeCode)))
            {
                if (enumValue == type && enumValue != TypeCode.Object)
                {
                    return true;
                }
            }
            return false;
        }

        //Procesa una propiedad que es una LISTA DE OBJETOS
        private static string ProcesarLista(object data, string excludedProperties, int profundidadActual, PropertyInfo propiedad, Type clasePadre)
        {
            if (data == null)
                return string.Empty;

            Type tipoGenerico;
            StringBuilder resultado = new StringBuilder();
            if (propiedad != null)
            {
                tipoGenerico = GetTipoGenerico(propiedad);
                resultado.Append(String.Format("{0}:[", propiedad.Name));
            }
            else
            {
                tipoGenerico = GetTipoGenerico(data);
            }

            foreach (var item in (IList)data)
                resultado.Append(ProcesarObjeto(item, excludedProperties, profundidadActual, tipoGenerico, clasePadre));
            resultado.Append("]");
            return resultado.ToString();
        }

        //Reglas de exclusion de propiedades por tipo de dato
        private static string ExcluirPropiedad(object data, PropertyInfo propiedad, string mensaje)
        {
            if (data == null)
                return string.Empty;
            return String.Format("{0}:{1};", propiedad.Name, mensaje);
        }
        private static string ExcluirPropiedad(object data, PropertyInfo propiedad)
        {
            return ExcluirPropiedad(data, propiedad, "PROPIEDAD_SIN_LOGUEO_PERMITIDO");
        }

        #endregion

        #region Métodos de Soporte

        /// <summary>
        /// Obtiene el tipo generico de una lista. Ej. List<Persona> devuelve Type=Persona
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static Type GetTipoGenerico(PropertyInfo pi)
        {
            Type type = pi.PropertyType;
            if (type.IsGenericType && type.GetGenericTypeDefinition()
                    == typeof(List<>))
                return type.GetGenericArguments()[0]; // use this...
            return null;

        }

        /// <summary>
        /// Obtiene el tipo generico de una lista. Ej. List<Persona> devuelve Type=Persona
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        private static Type GetTipoGenerico(object pi)
        {
            Type type = pi.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition()
                    == typeof(List<>))
                return type.GetGenericArguments()[0]; // use this...
            return null;

        }

        /// <summary>
        /// Define algunas excepciones para propiedades que no deben ser procesadas como una clase
        /// sino como un elemento primitivo en el arbol
        /// </summary>
        /// <param name="propiedad"></param>
        /// <returns></returns>
        private static bool EsPropiedadProcesable(PropertyInfo propiedad)
        {
            //Detectar propiedad por nombre y poner breakpoint
            /*
            if (propiedad.Name.Contains("nombreDePropiedad"))
                propiedad.Name.ToString();
            */

            if (propiedad.PropertyType.IsValueType) //Afecta a los tipos Nullable, ya que extienden de ValueType
                return false;
            if (propiedad.PropertyType.IsPrimitive)
                return false;
            if (propiedad.PropertyType.Name.CompareTo("String") == 0)
                return false;
            if (propiedad.PropertyType.Name.CompareTo("Byte[]") == 0)
                return false;
            if (propiedad.PropertyType.Name.CompareTo("byte[]") == 0)
                return false;
            if (propiedad.PropertyType.Name.CompareTo("string") == 0)
                return false;
            if (propiedad.PropertyType.Name.CompareTo("DateTime") == 0)
                return false;
            if (propiedad.PropertyType.BaseType != null && propiedad.PropertyType.BaseType.Name.CompareTo("Enum") == 0)
                return false;
            return true;
        }

        #endregion
    }
}