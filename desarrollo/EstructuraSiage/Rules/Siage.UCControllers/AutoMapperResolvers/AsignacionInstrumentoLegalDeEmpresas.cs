using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Siage.Core.Domain;
using Siage.Data.DAO;
using Siage.Services.Core.Models;

namespace Siage.UCControllers.AutoMapperResolvers
{
    /** 
    * <summary> Clase AsignacionInstrumentoLegalDeEmpresas
    *	
    * </summary>
    * <remarks>
    *		Autor: Ale
    *		Fecha: 9/2/2011 4:44:31 PM
    * </remarks>
    */
    public class AsignacionInstrumentoLegalDeEmpresas : ValueResolver<EmpresaBase, List<AsignacionInstrumentoLegalModel>>
    {
        #region Constantes
        #endregion

        #region Atributos
        #endregion

        #region Constructores
        #endregion

        #region Métodos Públicos
        #endregion

        #region Métodos Privados
        #endregion

        #region Overrides

        protected override List<AsignacionInstrumentoLegalModel> ResolveCore(EmpresaBase source)
        {
            return
                Mapper.Map<List<AsignacionInstrumentoLegal>, List<AsignacionInstrumentoLegalModel>>(
                    new DaoProvider().GetDaoAsignacionInstrumentoLegal().GetByEmpresaId(source.Id));
        }

        #endregion
    }
}
