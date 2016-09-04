using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Siage.Services.Core.Models;
using Siage.Base;
using Siage.Core.Domain;
using Siage.Base.Dto;


namespace Siage.Services.Core.InterfacesUC
{
   public interface ICronogramaEscolarRules
   {
       List<CronogramaEscolarModel> GetEventosEscolares(int idEscuela, int mes, int anio);
       string GetIdNivelEducativoByEscuela(int idEscuela);
   }
}
