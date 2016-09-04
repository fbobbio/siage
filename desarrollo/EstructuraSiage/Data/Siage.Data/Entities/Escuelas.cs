using System;
using Siage.Core.Domain;
using Siage.Base;

namespace Siage.Data.MockEntities
{
    public class Escuelas
    {
        private static Escuela _escuelaMadre3;
        public static Escuela EscuelaMadre3
        {
            get
            {
                if(_escuelaMadre3 == null)
                    _escuelaMadre3 = 
                new Escuela
                {
                    Id = 3,
                    Nombre = "ESCUELA_MADRE3",
                    CodigoEmpresa = "EM1",
                    FechaAlta = new DateTime(2011, 1, 1),
                    FechaInicioActividad = new DateTime(2011, 1, 2),
                    TipoEmpresa = TipoEmpresaEnum.ESCUELA_MADRE,
                    //EstadoEmpresa = EstadoEmpresaEnum.ACTIVA,
                    CUE = "CUE1",
                    EsRaiz = true,
                    //Nivel = 1,
                    NumeroEscuela = 1,
                    Religioso = false,
                    //TipoEscuela = TipoEscuelaEnum.ESCUELA_NORMAL,
                    //ZonaDesfavorable = false,
                    TipoCategoria = CategoriaEscuelaEnum.UNO,   
                    EscuelaPlan = EscuelasPlanes.Lista
                };
                return _escuelaMadre3;
            }
        }

        private static Escuela _escuelaMadre4;
        public static Escuela EscuelaMadre4
        {
            get
            {
                if(_escuelaMadre4 == null)
                    _escuelaMadre4 = new Escuela
                {
                    Id = 2,
                    Nombre = "ESCUELA_MADRE4",
                    CodigoEmpresa = "EM2",
                    FechaAlta = new DateTime(2011, 2, 2),
                    FechaInicioActividad = new DateTime(2011, 2, 3),
                    TipoEmpresa = TipoEmpresaEnum.ESCUELA_MADRE,
                    //EstadoEmpresa = EstadoEmpresaEnum.ACTIVA,
                    CUE = "CUE2",
                    EsRaiz = true,
                    //Nivel = NivelEducativoEnum.INICIAL,
                    NumeroEscuela = 1,
                    Religioso = false,
                    //TipoEscuela = TipoEscuelaEnum.ESCUELA_NORMAL,
                    //ZonaDesfavorable = false,
                    TipoCategoria = CategoriaEscuelaEnum.DOS,
                    EscuelaPlan = EscuelasPlanes.Lista
                };
                return _escuelaMadre4;
            }
        }
    }
}
