﻿namespace Siage.Base.Dto
{
  public  class DtoEmpresaExternaConsulta
    {

      public virtual int Id { get; set; }
      public virtual string Nombre { get; set; }
      public virtual string RazonSocial { get; set; }
      public virtual string Cuil { get; set; }
      public virtual string Cuit { get; set; }
      public virtual bool Activo { get; set; }
      public virtual TipoEmpresaExternaEnum TipoEmpresa { get; set; }
      public virtual string BarrioNuevo { get; set; }
      public virtual string NombreCalle { get; set; }
      public virtual int? Altura { get; set; }

    }
}
