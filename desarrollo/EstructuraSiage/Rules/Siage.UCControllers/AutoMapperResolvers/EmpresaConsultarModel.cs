using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Base;
using Siage.Core.Domain;

namespace Siage.UCControllers.AutoMapperResolvers
{
    public class EmpresaConsultarModelDireccionDeNivel : ValueResolver<EmpresaBase, string>
    {
        protected override string ResolveCore(EmpresaBase source)
        {
            StringBuilder nivelEducativo = new StringBuilder();
            if (source.TipoEmpresa == TipoEmpresaEnum.DIRECCION_DE_NIVEL)
            {
                var direccionDeNivel = ((DireccionDeNivel)source);
                if (direccionDeNivel.NivelesEducativos != null
                    && direccionDeNivel.NivelesEducativos.Count > 0)
                {
                    foreach (var item in direccionDeNivel.NivelesEducativos)
                        nivelEducativo.Append(item.Nombre + ", ");
                    nivelEducativo.Remove(nivelEducativo.ToString().LastIndexOf(", "), 2);
                }
            }
            return nivelEducativo.ToString();
        }
    }

    public class EmpresaConsultarModelEscuelaCUE : ValueResolver<EmpresaBase, string>
    {
        protected override string ResolveCore(EmpresaBase source)
        {
            Escuela escuela = source as Escuela;
            if (escuela != null)
            {
                return escuela.CUE;
            }
            return string.Empty;
        }
    }

    public class EmpresaConsultarModelEscuelaRaiz : ValueResolver<EmpresaBase, bool>
    {
        protected override bool ResolveCore(EmpresaBase source)
        {
            if (source.TipoEmpresa == TipoEmpresaEnum.ESCUELA_MADRE || source.TipoEmpresa == TipoEmpresaEnum.ESCUELA_ANEXO)
            {
                Escuela escuela = source as Escuela;
                if (escuela != null)
                {
                    return escuela.EsRaiz;
                }
            }
            return false;
        }
    }

    public class EmpresaConsultarModelTipoEducacionResolver : ValueResolver<EmpresaBase, TipoEducacionEnum?>
    {
        protected override TipoEducacionEnum? ResolveCore(EmpresaBase source)
        {
            switch (source.TipoEmpresa)
            {
                case TipoEmpresaEnum.ESCUELA_MADRE:
                    {
                        Escuela escuela = source as Escuela;
                        if (escuela != null)
                            return escuela.TipoEducacion;
                    }
                    break;
                case TipoEmpresaEnum.ESCUELA_ANEXO:
                    {
                        EscuelaAnexo escuelaAnexo = source as EscuelaAnexo;
                        if (escuelaAnexo != null)
                            return escuelaAnexo.TipoEducacion;
                    }
                    break;
                case TipoEmpresaEnum.DIRECCION_DE_NIVEL:
                    {
                        DireccionDeNivel direccionDeNivel = source as DireccionDeNivel;
                        if (direccionDeNivel != null)
                            return direccionDeNivel.TipoEducacion;
                    }
                    break;
            }
            return null;
        }
    }
}
