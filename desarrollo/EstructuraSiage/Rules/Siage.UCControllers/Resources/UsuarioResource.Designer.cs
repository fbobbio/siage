﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Siage.UCControllers.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class UsuarioResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UsuarioResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Siage.UCControllers.Resources.UsuarioResource", typeof(UsuarioResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Las claves ingresadas no coinciden.
        /// </summary>
        internal static string ClavesNoCoinciden {
            get {
                return ResourceManager.GetString("ClavesNoCoinciden", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Falta ingresar datos obligatorios.
        /// </summary>
        internal static string DatosIncompletos {
            get {
                return ResourceManager.GetString("DatosIncompletos", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Se debe indicar la empresa para cargar el usuario..
        /// </summary>
        internal static string FaltaEmpresa {
            get {
                return ResourceManager.GetString("FaltaEmpresa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Se debe indicar la persona para cargar el usuario.
        /// </summary>
        internal static string FaltaPersona {
            get {
                return ResourceManager.GetString("FaltaPersona", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La dirección de e-mail ingresada no es valida.
        /// </summary>
        internal static string FormatoEmail {
            get {
                return ResourceManager.GetString("FormatoEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La clave nueva no tiene la longitud mínima de 4 caracteres.
        /// </summary>
        internal static string LongitudClave {
            get {
                return ResourceManager.GetString("LongitudClave", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ya existe en el sistema una cuenta de usuario asociada a esa dirección de e-mail.
        /// </summary>
        internal static string MailUsuarioExiste {
            get {
                return ResourceManager.GetString("MailUsuarioExiste", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No existe en el sistema un usuario asociado con esa dirección de correo electrónico.
        /// </summary>
        internal static string MailUsuarioInexistente {
            get {
                return ResourceManager.GetString("MailUsuarioInexistente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El nombre de usuario ingresado ya existe en el sistema.
        /// </summary>
        internal static string NombreUsuarioExistente {
            get {
                return ResourceManager.GetString("NombreUsuarioExistente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La cuenta de usuario con la que intenta acceder no corresponde a un usuario Administrador.
        /// </summary>
        internal static string PerfilAdministradorRequerido {
            get {
                return ResourceManager.GetString("PerfilAdministradorRequerido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El usuario o la clave de acceso son incorrectas.
        /// </summary>
        internal static string UsuarioClaveIncorrecta {
            get {
                return ResourceManager.GetString("UsuarioClaveIncorrecta", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La cuenta de usuario no está habilitada. Contacte con el administrador del sistema.
        /// </summary>
        internal static string UsuarioInactivo {
            get {
                return ResourceManager.GetString("UsuarioInactivo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La cuenta de usuario no ha sido validada. Verifique el e-mail  que le ha sido enviado para activar la cuenta.
        /// </summary>
        internal static string UsuarioNoComprobado {
            get {
                return ResourceManager.GetString("UsuarioNoComprobado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El nombre de usuario ingresado no se encuentra disponible..
        /// </summary>
        internal static string UsuarioNombreExistente {
            get {
                return ResourceManager.GetString("UsuarioNombreExistente", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to El rol de usuario que desea registrar ya existe para esta persona..
        /// </summary>
        internal static string UsuarioRepetido {
            get {
                return ResourceManager.GetString("UsuarioRepetido", resourceCulture);
            }
        }
    }
}