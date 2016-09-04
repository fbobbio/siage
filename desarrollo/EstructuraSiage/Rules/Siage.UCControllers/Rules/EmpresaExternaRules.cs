using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.ServiceLocation;
using Siage.Core.DaoInterfaces;
using Siage.Core.Domain;
using Siage.Data.DAO;
using Siage.Services.Core.InterfacesUC;
using Siage.Services.Core.Models;
using Siage.Base;
using AutoMapper;
using Siage.UCControllers.Resources;
using Siage.Base.Dto;


namespace Siage.UCControllers.Rules
{

    public class EmpresaExternaRules : IEmpresaExternaRules
    {
        #region Atributos

        private IDaoProvider _daoProvider;
        private IDaoEmpresaExterna _daoEmpresaExterna;
        private IDaoTipoCalle _daoTipoCalle;

        #endregion

        #region Propiedades
        private IDaoProvider DaoProvider
        {
            get
            {
                if (_daoProvider == null)
                {
                    _daoProvider = ServiceLocator.Current.GetInstance<IDaoProvider>();
                }
                return _daoProvider;
            }
        }
        private IDaoTipoCalle DaoTipoCalle
        {
            get
            {
                if (_daoTipoCalle == null)
                {
                    _daoTipoCalle = DaoProvider.GetDaoTipoCalle();
                }
                return _daoTipoCalle;
            }
        }

        private IDaoEmpresaExterna DaoEmpresaExterna
        {
            get
            {
                if (_daoEmpresaExterna == null)
                {
                    _daoEmpresaExterna = DaoProvider.GetDaoEmpresaExterna();//ServiceLocator.Current.GetInstance<IDaoEmpresaExterna>();
                }
                return _daoEmpresaExterna;
            }

        }
        #endregion

        #region IRules
        public EmpresaExternaModel GetEmpresaExternaById(int id)
        {
            var empresaEntidad = DaoEmpresaExterna.GetById(id);

            var empresaModel = Mapper.Map<EmpresaExterna, EmpresaExternaModel>(empresaEntidad);
            if (empresaEntidad.PersonaJuridica != null)
                empresaModel.ReferenteEmpresa = empresaModel.Referente;
            return empresaModel;
        }

        public List<DtoEmpresaExternaConsultaModel> GetFiltroBusquedaEmpresaExterna(string nombre, string razonSocial, string cuil, string cuit, TipoEmpresaExternaEnum? tipoEmpresa, bool estado)
        {
            var nuevoCuil = RecortarCuilCuit(cuil);
            var nuevoCuit = RecortarCuilCuit(cuit);

            var lista = DaoEmpresaExterna.GetByFiltros(nombre, razonSocial, nuevoCuil, nuevoCuit, tipoEmpresa, estado);
           return Mapper.Map<List<DtoEmpresaExternaConsulta>, List<DtoEmpresaExternaConsultaModel>>(lista);
        }

        public bool VerificarExistenciaEmpresaXpersona(string cuil, string cuit)
        {
            var nuevoCuil = !string.IsNullOrEmpty(cuil) ? RecortarCuilCuit(cuil) : string.Empty;
            var nuevoCuit = !string.IsNullOrEmpty(cuit) ? RecortarCuilCuit(cuit) : string.Empty;
            var existe = DaoEmpresaExterna.GetByFiltrosVerificarPersonaAsociadaAempresa(nuevoCuil, nuevoCuit);

            return existe;
        }

        public int GetIdPersona(string cuil, string cuit)
        {
            var nuevoCuil = string.IsNullOrEmpty(cuil)!=true ? RecortarCuilCuit(cuil) : string.Empty;
            var nuevoCuit = !string.IsNullOrEmpty(cuit) ? RecortarCuilCuit(cuit) : string.Empty;
            var documentoPf = !string.IsNullOrEmpty(nuevoCuil) ? nuevoCuil.Substring(2,8) : string.Empty;

            if (!string.IsNullOrEmpty(cuil))
            {
            
            if(documentoPf[0]=='0')
            {
                documentoPf = documentoPf.Substring(1,7);
            }
            }
            var idPersona = DaoEmpresaExterna.VerificarPersonaYgetByPersonaId(documentoPf, nuevoCuit);
            
            return idPersona;
        }

        public PersonaFisicaModel TomarPersonaFisicaById(int id)
        {
            var personaFisica = DaoProvider.GetDaoPersonaFisica().GetById(id);

            return Mapper.Map<PersonaFisica,PersonaFisicaModel>(personaFisica);
        }

        public List<CondicionIvaModel> GetCondicionIvaAll()
        {
            var listaCondicionIva = DaoProvider.GetDaoCondicionIva().GetAll();
            return Mapper.Map<List<CondicionIva>, List<CondicionIvaModel>>(listaCondicionIva);
        }
        
        public EmpresaExternaModel EmpresaExternaSave(EmpresaExternaModel modelo)
        {
            ValidarExistenciaEmpresaExterna(modelo.Nombre);
            ValidarReferenteEmpresa(modelo);
            #region Guardar Persona

            if (modelo.Referente != null)
            {
                var reglaPersona = new PersonaFisicaRules();
                var persona = reglaPersona.PersonaFisicaSaveOUpdate(modelo.Referente);
                modelo.Referente = persona;
            }

            if (modelo.PersonaJuridica != null)
            {
                var reglaPJ = new PersonaJuridicaRules();
                var personaJuridica = reglaPJ.PersonaJuridicaSaveOUpdate(modelo.PersonaJuridica);
                modelo.PersonaJuridica = personaJuridica;


                if (modelo.ReferenteEmpresa != null)
                {
                    var reglaPersonaReferente = new PersonaFisicaRules();
                    var personaReferente = reglaPersonaReferente.PersonaFisicaSaveOUpdate(modelo.ReferenteEmpresa);
                    modelo.ReferenteEmpresa = personaReferente;
                }
                else
                {
                    throw new BaseException("Tiene que ingresar una persona referente a la Empresa");
                }
            }

            #endregion
            var condicionIva = modelo.CondicionIva!=null? DaoProvider.GetDaoCondicionIva().GetById(modelo.CondicionIva):null;
            var empresaEntidad = Mapper.Map<EmpresaExternaModel, EmpresaExterna>(modelo);
            CargarComunicaciones(empresaEntidad, modelo);
            empresaEntidad.CondicionIva = condicionIva;
            empresaEntidad.FechaAlta = DateTime.Now;
            empresaEntidad.Activo = true;
            var empresamodelo = DaoEmpresaExterna.Save(empresaEntidad);
            var modeloDevuelto = Mapper.Map<EmpresaExterna, EmpresaExternaModel>(empresamodelo);
            return modeloDevuelto;
        }

       

        public void ValidarExistenciaEmpresaExterna(string nombre)
        {
            var existeEmpresa = DaoEmpresaExterna.GetByFiltrosExisteEmpresa(nombre);

            if(existeEmpresa)
            {
                throw new ApplicationException("Existe una Empresa con el mismo nombre, ingrese un nuevo nombre");
            }

        }

        public void DomicilioSave(DomicilioModel modelo, int idEmpresa)
        {
            if (modelo == null)
                throw new BaseException("Debe asignar un domicilio a la empresa externa.");
            var entidad = Mapper.Map<DomicilioModel, Domicilio>(modelo);
            if (entidad == null)
                entidad = new Domicilio();
            if (modelo.IdCalle > 0)
            {
                var calle = DaoProvider.GetDaoCalle().GetById(modelo.IdCalle);
                entidad.Calle = calle;
                entidad.TipoCalle = calle.TipoCalle;
            }
            entidad.Altura = modelo.Altura;
            entidad.Piso = modelo.Piso;
            entidad.Departamento = modelo.Departamento;
            entidad.Torre = modelo.Torre;

            var localidad = DaoProvider.GetDaoLocalidad().GetById(modelo.IdLocalidad);
            entidad.Localidad = localidad;
            entidad.Provincia = localidad.Provincia;
            entidad.DepartamentoProvincial = localidad.DepartamentoProvincial;
            entidad.Barrio = DaoProvider.GetDaoBarrio().GetById(modelo.IdBarrio);
            entidad.Origen = OrigenEnum.T_DO_EMP_EXTERNA;
            entidad.EntidadId = idEmpresa.ToString();
            DaoProvider.GetDaoDomicilio().Save(entidad);
        }

        public EmpresaExternaModel TomarIdDeEmpresa(int id)
        {
            var empresaExterna = DaoEmpresaExterna.GetById(id);

            return Mapper.Map<EmpresaExterna, EmpresaExternaModel>(empresaExterna);
        }

        public EmpresaExternaModel EmpresaExternaUpdateOrSave(EmpresaExternaModel modelo)
        {
            ValidarEmpresaExternaSave(modelo);
            var empresaExEntidad = DaoEmpresaExterna.GetById(modelo.Id);
            if(modelo.CondicionIva!=null)
            {
                var condicionIva = DaoProvider.GetDaoCondicionIva().GetById(modelo.CondicionIva);
                empresaExEntidad.CondicionIva = condicionIva;
            }else
            {
                empresaExEntidad.CondicionIva = null;
            }

            empresaExEntidad.Descripcion = modelo.Descripcion;
            empresaExEntidad.NumeroAnses = modelo.NumeroAnses;
            empresaExEntidad.NumeroIngBrutos = modelo.NumeroIngBrutos;
            empresaExEntidad.Sucursal = modelo.Sucursal;
            empresaExEntidad.Observaciones = modelo.Observaciones;
            if (modelo.FechaCreacion != null) empresaExEntidad.FechaCreacion = modelo.FechaCreacion.Value;
            CargarComunicaciones(empresaExEntidad, modelo);
            empresaExEntidad.FechaUltimaActivacion = DateTime.Today;
            var empresaExternaRecuperada = DaoEmpresaExterna.SaveOrUpdate(empresaExEntidad);
            var empresaModel = Mapper.Map<EmpresaExterna, EmpresaExternaModel>(empresaExternaRecuperada);
            return empresaModel;
        }

        public void DeleteEmpresaExterna(EmpresaExternaModel modelo)
        {
            var empresaExternaEntidad = DaoEmpresaExterna.GetById(modelo.Id);
            if (DaoEmpresaExterna.VerificarExistenciaEnPuestoTrabajo(empresaExternaEntidad.Id))
            {
                throw new BaseException("La empresa externa no puede ser eliminada, esta vinculada a un puesto de trabajo Activo");
            }
            if (string.IsNullOrEmpty(modelo.MotivoBaja))
            {
                throw new BaseException("Tiene que ingresar el motivo de la baja");
            }
            empresaExternaEntidad.MotivoBaja = modelo.MotivoBaja;
            empresaExternaEntidad.FechaBaja = DateTime.Now;
            empresaExternaEntidad.Activo = false;
            DaoEmpresaExterna.SaveOrUpdate(empresaExternaEntidad);
        }

        public EmpresaExternaModel ReactivarEmpresaExterna(EmpresaExternaModel modelo)
        {
            var empresa = DaoEmpresaExterna.GetById(modelo.Id);
            if (empresa.Activo)
            {
                throw new BaseException("La empresa externa seleccionada ya se encuentra activa.");
            }
            empresa.Activo = true;
            empresa.FechaUltimaActivacion = DateTime.Now;
            var empresaExtEntidad = DaoEmpresaExterna.SaveOrUpdate(empresa);
            return Mapper.Map<EmpresaExterna, EmpresaExternaModel>(empresaExtEntidad);
        }
        #endregion

        #region Soporte
        public void ValidarReferenteEmpresa(EmpresaExternaModel modelo)
        {
           if(modelo.PersonaJuridica!=null && modelo.ReferenteEmpresa==null)
           {
               throw new BaseException("No se asigno un referente de la empresa");
           }
        }

        public string RecortarCuilCuit(string valor)
        {
            var nuevoValor = "";
           
            if (!string.IsNullOrEmpty(valor))
            {

                for (var i = 0; i < valor.Length; i++)
                {
                    if (valor[i].ToString() != "-")
                    {
                        nuevoValor = nuevoValor + valor[i];
                    }

                }
            }
            return nuevoValor;
        }

        public string TomarDocumentoPf(string valor)
        {
            var nuevoValor = "";
            if (!string.IsNullOrEmpty(valor))
            {

                for (var i = 0; i < valor.Length; i++)
                {
                    if (i != 0 || i != 1 || i != 10 )
                    {
                        if (valor[i].ToString()!="-")
                        {
                            nuevoValor = nuevoValor + valor[i];
                        }
                        

                    }

                }
            }
            return nuevoValor;
        }

        public void ValidarEmpresaExternaSave(EmpresaExternaModel modelo)
        {
            if (!modelo.FechaCreacion.HasValue)
                throw new BaseException("Debe ingresar un valor en fecha creación");
            if (modelo.FechaCreacion.HasValue && modelo.FechaCreacion.Value < DateTime.FromOADate(2))
                throw new BaseException("Valor ingresado en fecha creación tiene un formato invalido");
        }

        public void CargarComunicaciones(EmpresaExterna empresaExEntidad, EmpresaExternaModel modelo)
        {
            if (!string.IsNullOrEmpty(modelo.Telefono))
            {
                empresaExEntidad.AddComunicacion(modelo.Telefono, TipoComunicacionEnum.TELEFONO_PRINCIPAL);
            }
            if (!string.IsNullOrEmpty(modelo.Fax))
            {
                empresaExEntidad.AddComunicacion(modelo.Fax, TipoComunicacionEnum.FAX_PRINCIPAL);
            }
            if (!string.IsNullOrEmpty(modelo.Email))
            {
                empresaExEntidad.AddComunicacion(modelo.Email, TipoComunicacionEnum.DIRECCION_DE_CORREO);
            }
        }

        public void ModificarComunicaciones(EmpresaExterna empresaExEntidad, EmpresaExternaModel modelo)
        {
            for (int j = 0; j < empresaExEntidad.Comunicaciones.Count; j++)
            {
                var tipoCom = empresaExEntidad.Comunicaciones[j].TipoComunicacion;

                if (tipoCom == TipoComunicacionEnum.TELEFONO_PRINCIPAL)
                {
                    if (string.IsNullOrEmpty(modelo.Telefono))
                    {
                        empresaExEntidad.Comunicaciones.RemoveAt(j);
                    }
                    else
                    {
                        empresaExEntidad.Comunicaciones[j].Valor = modelo.Telefono;
                    }
                }

                if (tipoCom == TipoComunicacionEnum.FAX_PRINCIPAL)
                {
                    if (string.IsNullOrEmpty(modelo.Fax))
                    {
                        empresaExEntidad.Comunicaciones.RemoveAt(j);
                    }
                    else
                    {
                        empresaExEntidad.Comunicaciones[j].Valor = modelo.Fax;
                    }
                }

                if (tipoCom == TipoComunicacionEnum.DIRECCION_DE_CORREO)
                {
                    if (string.IsNullOrEmpty(modelo.Email))
                    {
                        empresaExEntidad.Comunicaciones.RemoveAt(j);
                    }
                    else
                    {
                        empresaExEntidad.Comunicaciones[j].Valor = modelo.Email;
                    }
                }
            }
        }


        #endregion

        public bool ConsultarExistenciaCuil(int id)
        {
            var persona = DaoProvider.GetDaoPersonaFisica().GetById(id);

            if(string.IsNullOrEmpty(persona.CUIL))
            {
                return false;
            }

            return true;
        }


        public bool ConsultarExistenciaCuit(int id)
        {
           var persona = DaoProvider.GetDaoPersonaJuridica().GetById(id);

            if(string.IsNullOrEmpty(persona.CUIT))
            {
                return false;
            }

            return true;
        
        }
    }
}

