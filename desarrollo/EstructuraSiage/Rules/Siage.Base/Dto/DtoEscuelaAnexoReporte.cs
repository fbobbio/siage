using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ValidationAttributes;

namespace Siage.Base.Dto
{
    public class DtoEscuelaAnexoReporte
    {
        public virtual int Id { get; set; }
        public virtual string CUE { get; set; }
        public virtual string CUEAnexo { get; set; }
        public virtual string CodigoEmpresa { get; set; }
        public virtual string NombreEmpresa { get; set; }
        public virtual DateTime FechaInicioActividad { get; set; }
        public virtual EstadoEmpresaEnum Estado { get; set; }
        public virtual string TipoCategoria { get; set; } //Lo cambie por string porque solo muestro el dato
        public virtual string CodigoOrdenPago { get; set; }
        public virtual string DescripcionOrdenDePago { get; set; }
        public virtual bool EsRaiz { get; set; }
        public virtual string CodigoProgPresupuestario { get; set; }
        public virtual string DescripcionProgPresupuestario { get; set; }
        public virtual TipoEducacionEnum? TipoEducacion { get; set; }
        public virtual string NivelEducativo { get; set; }
        public virtual int? IdNivelEducativo { get; set; }
        public virtual string Ambito { get; set; } //Lo cambie por string porque solo muestro el dato
        public virtual DependenciaEnum? Dependencia { get; set; }
        public virtual int? IdZonaDesfavorable { get; set; }
        public virtual string NombreZonaDesfavorable { get; set; }
        public virtual string Telefono { get; set; }
        public virtual bool Privado { get; set; }

        public virtual string CodigoEscuelaMadre { get; set; }
        public virtual string NombreEscuelaMadre { get; set; }

        //Datos planos del domicilio empresa
        public virtual string DeptoProvincialEmpresa { get; set; }
        public virtual int? IdDeptoProvincialEmpresa { get; set; }
        public virtual string LocalidadEmpresa { get; set; }
        public virtual int? IdLocalidadEmpresa { get; set; }
        public virtual string BarrioEmpresa { get; set; }        
        public virtual string CalleEmpresa { get; set; }
        public virtual string AlturaEmpresa { get; set; }

        public virtual string DomicilioCompleto
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

        //public virtual string TipoCategoriaString
        //{
        //    get { return Util.GetEnumClearName(TipoCategoria); }
        //}

        public virtual string EstadoString
        {
            get { return Util.GetEnumClearName(Estado); }
        }

        public virtual string TipoEducacionString
        {
            get
            {
                return TipoEducacion != null ? Util.GetEnumClearName(TipoEducacion) : string.Empty;
            }
        }

        public virtual string EsRaizString
        {
            get { return EsRaiz == true ? NoSiEnum.SI.ToString() : NoSiEnum.NO.ToString(); }
        }

        //public virtual string AmbitoString
        //{
        //    get
        //    {
        //        return Ambito != null ? Util.GetEnumClearName(Ambito) : string.Empty;
        //    }
        //}

        public virtual string DependenciaString
        {
            get
            {
                return Dependencia != null ? Util.GetEnumClearName(Dependencia) : string.Empty;
            }
        }

        public virtual string CUEYCUEAnexo
        {
            get
            {
                var cadena = CUE;
                if (!string.IsNullOrEmpty(CUE) && !string.IsNullOrEmpty(CUEAnexo))
                    cadena = cadena + "-" + CUEAnexo;
                return cadena;
            }
        }

        public virtual string ProgPresupuestarioString
        {
            get
            {
                var cadena = CodigoProgPresupuestario;
                if (!string.IsNullOrEmpty(DescripcionProgPresupuestario.Trim()))
                    cadena = cadena + " / " + DescripcionProgPresupuestario;
                return cadena;
            }
        }

        public virtual string PublicaPrivadaString
        {
            get
            {
                return Privado ? "Privada" : "Pública";
            }
        }

        public virtual string DatosEscuelaMadre
        {
            get
            {
                var cadena = string.Empty;
                if(!string.IsNullOrEmpty(CodigoEscuelaMadre))
                    cadena = "Código: " + CodigoEscuelaMadre;
                if (!string.IsNullOrEmpty(NombreEscuelaMadre))
                    cadena += " Nombre: " + NombreEscuelaMadre;
                return cadena;
            }
        }

    }
}
