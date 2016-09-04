using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siage.Base.Dto
{
    public class DtoVinculoEmpresaReporte
    {
        public virtual int IdVinculoEmpresa { get; set; }
        public virtual int IdEmpresa { get; set; }
        public virtual string IdentificadorEmpresa { get; set; }
        public virtual string NombreEmpresa { get; set; }

        //Datos planos del domicilio empresa
        public virtual string DeptoProvincialEmpresa { get; set; }
        public virtual int? IdDeptoProvincialEmpresa { get; set; }
        public virtual string LocalidadEmpresa { get; set; }
        public virtual int? IdLocalidadEmpresa { get; set; }
        public virtual string BarrioEmpresa { get; set; }
        public virtual int? IdBarrioEmpresa { get; set; }
        public virtual string CalleEmpresa { get; set; }
        public virtual int? IdCalleEmpresa { get; set; }
        public virtual string AlturaEmpresa { get; set; }

        public virtual int IdEdificio { get; set; }
        public virtual string IdentificadorEdificio { get; set; }
        public virtual int? IdTipoEdificio { get; set; }
        public virtual string TipoEdificio { get; set; }
        public virtual int? IdFuncionEdificio { get; set; }
        public virtual string FuncionEdificio { get; set; }
        public virtual string SuperficieTotalEdificio { get; set; }

        //Datos planos del domicilio edificio
        public virtual string DeptoProvincialEdificio { get; set; }
        public virtual int? IdDeptoProvincialEdificio { get; set; }
        public virtual string LocalidadEdificio { get; set; }
        public virtual int? IdLocalidadEdificio { get; set; }
        public virtual string BarrioEdificio { get; set; }
        public virtual int? IdBarrioEdificio { get; set; }
        public virtual string CalleEdificio { get; set; }
        public virtual int? IdCalleEdificio { get; set; }
        public virtual string AlturaEdificio { get; set; }

        public virtual string DomicilioCompletoEmpresa
        {
            get
            {
                StringBuilder cadena = new StringBuilder();

                if (!string.IsNullOrEmpty(DeptoProvincialEmpresa))
                    cadena.Append("Depto. prov.: " + DeptoProvincialEmpresa + " - ");
                if (!string.IsNullOrEmpty(LocalidadEmpresa))
                    cadena.Append("Localidad: " + LocalidadEmpresa + " - ");
                if (!string.IsNullOrEmpty(BarrioEmpresa))
                    cadena.Append("Barrio: " + BarrioEmpresa + " - ");
                if (!string.IsNullOrEmpty(CalleEmpresa))
                    cadena.Append("Calle: " + CalleEmpresa + " - ");
                if (!string.IsNullOrEmpty(AlturaEmpresa))
                    cadena.Append("Altura: " + AlturaEmpresa);

                return cadena.ToString();
            }
        }

        public virtual string DomicilioCompletoEdificio
        {
            get
            {
                StringBuilder cadena = new StringBuilder();

                if (!string.IsNullOrEmpty(DeptoProvincialEdificio))
                    cadena.Append("Depto. prov.: " + DeptoProvincialEdificio + " - ");
                if (!string.IsNullOrEmpty(LocalidadEdificio))
                    cadena.Append("Localidad: " + LocalidadEdificio + " - ");
                if (!string.IsNullOrEmpty(BarrioEdificio))
                    cadena.Append("Barrio: " + BarrioEdificio + " - ");
                if (!string.IsNullOrEmpty(CalleEdificio))
                    cadena.Append("Calle: " + CalleEdificio + " - ");
                if (!string.IsNullOrEmpty(AlturaEdificio))
                    cadena.Append("Altura: " + AlturaEdificio);

                return cadena.ToString();
            }
        }
    }
}
