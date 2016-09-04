<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaRegistrarModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>
<%@ Import Namespace="Siage.Base" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
    int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;

    var nivelEducativoEscuela =
        Html.CreateSelectList<NivelEducativoModel>(Siage.Base.ViewDataKey.NIVEL_EDUCATIVO.ToString(),
                                                   Model.NivelEducativoId.HasValue ? Model.NivelEducativoId.Value : -1);
    var tipoEscuelasPermitidas =
        Html.CreateSelectList<TipoEscuelaModel>(Siage.Base.ViewDataKey.TIPO_ESCUELA_PERMITIDA_EMPRESA.ToString(),
                                                Model.TipoEscuela.HasValue ? Model.TipoEscuela.Value : -1);
    var turno = Html.CreateSelectList<TurnoModel>(Siage.Base.ViewDataKey.TURNO.ToString(),
                                                  Model.TurnoId.HasValue ? Model.TurnoId.Value : -1);
    var periodoLectivo = Html.CreateSelectList<PeriodoLectivoModel>(Siage.Base.ViewDataKey.PERIODO_LECTIVO.ToString(),
                                                                    Model.PeriodoLectivoId.HasValue
                                                                        ? Model.PeriodoLectivoId.Value
                                                                        : -1);
    var tipoJornada = Html.CreateSelectList<TipoJornadaModel>(Siage.Base.ViewDataKey.TIPO_JORNADA.ToString(),
                                                              Model.TipoJornada.HasValue ? Model.TipoJornada.Value : -1);
    var modalidadJornada =
        Html.CreateSelectList<ModalidadJornadaModel>(Siage.Base.ViewDataKey.MODALIDAD_JORNADA.ToString(),
                                                     Model.ModalidadJornada.HasValue ? Model.ModalidadJornada.Value : -1);
    var sucursalBancaria =
        Html.CreateSelectList<SucursalBancariaModel>(Siage.Base.ViewDataKey.SUCURSAL_BANCARIA.ToString(),
                                                     Model.SucursalBancariaId.HasValue
                                                         ? Model.SucursalBancariaId.Value
                                                         : -1);
    var obraSocial = Html.CreateSelectList<ObraSocialModel>(Siage.Base.ViewDataKey.OBRA_SOCIAL.ToString(),
                                                            Model.ObraSocialId.HasValue ? Model.ObraSocialId.Value : -1);
    var zonaDesfavorable =
        Html.CreateSelectList<ZonaDesfavorableModel>(Siage.Base.ViewDataKey.ZONA_DESFAVORABLE.ToString(),
                                                     Model.ZonaDesfavorableId.HasValue
                                                         ? Model.ZonaDesfavorableId.Value
                                                         : -1);
    
    using (Html.BeginForm())
    {
%>


<fieldset>
    <div id="escuelaTabs">
        <ul>
            <li id="esSolapa1"><a href="#divEmpresasRaizInspeccionMadre">Jerarquía</a></li>
            <li id="esSolapa2"><a href="#divParticularesEscuela">Generales escuela</a></li>
            <li id="esSolapa3"><a href="#divEscuelaPrivada">Privada</a></li>
            <li id="esSolapa4"><a href="#divEscuelaZonaDesfavorable">Zona Desfavorable</a></li>
        </ul>

    <div id="divEmpresasRaizInspeccionMadre">
        <div id="divModificarTipoEmpresa">
            <p class="botones">
            <!--
                < %if(false) %>  Se quita esto porque no va en la versión de producción: < %if estadoText == "Editar") %> 
                    < %:Html.BtnGenerico(Botones.ButtonType.button, "Modificar tipo empresa", string.Empty, string.Empty,"btnModificarTipoEmpresa")%>           
            -->
            </p>       
        </div>  

        <div id="divEscuelaMadreRaiz" style="display:none;">
            <%: Html.AbmcCheckControlFor(model => model.EsRaiz, estadoId, Abmc.CheckControl.CheckBox, "EscuelaMadre")%>
            <p class="EscuelaMadre botones">
                <% if (estadoText != "Ver")%>
                    <%:Html.BtnGenerico(Botones.ButtonType.button, "Buscar escuela raíz", string.Empty, string.Empty,"btnEscuelaRaiz")%>
            </p>

    
            <div id="divSeleccionEscuelaRaiz" style="display: none;">
                <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
            </div>
        </div>

        <%: Html.AbmcTextControlFor(model => model.NumeroAnexo, estadoId, Abmc.TextControl.TextBox)%>

        <div id="divBtnSeleccionarEscuelaMadre" style="display: none;">
            <p class="EscuelaAnexo botones">
                <% if(estadoText != "Ver")%>
                    <%: Html.BtnGenerico(Botones.ButtonType.button, "Buscar escuela madre", string.Empty, string.Empty, "btnEscuelaMadre") %>
            </p>
        </div>

        <div id="divSeleccionEscuelaMadre" style="display: none;">
            <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
        </div>

        <% if(estadoText != "Ver")%>
            <%:  Html.BtnGenerico(Botones.ButtonType.button, "Seleccionar empresa inspección", string.Empty, string.Empty, "btnSelecEmpresaInspeccion")%>
        <div id="divSeleccionEmpresaInspeccion" style="display: none;">
            <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
        </div>
    </div>
    <div id="divParticularesEscuela">
    <fieldset>
        <legend>Datos escuela</legend>
        <%: Html.AbmcTextControlFor(model => model.CUE, estadoId, Abmc.TextControl.TextBox)%>
        <p class="EscuelaMadreAnexo">
            <%: Html.Label("-") %>
        </p>
        <%: Html.AbmcTextControlFor(model => model.CUEAnexo, estadoId, Abmc.TextControl.TextBox) %>
        <div id="NumEscuela">
            <%: Html.AbmcTextControlFor(model => model.NumeroEscuela, estadoId, Abmc.TextControl.TextBox)%>
        </div>
        <%: Html.AbmcTextControlFor(model => model.TipoEducacion, estadoId, Abmc.TextControl.DropDownListEnum)%>        
        <%: Html.AbmcSelectControlFor(model => model.NivelEducativoId, estadoId, Abmc.SelectControl.DropDownList, nivelEducativoEscuela, Constantes.Seleccione)%>    
        <%: Html.AbmcSelectControlFor(model => model.TipoEscuela, estadoId, Abmc.SelectControl.DropDownList, tipoEscuelasPermitidas, Constantes.Seleccione)%>
        <%: Html.AbmcTextControlFor(model => model.CodigoInspeccion, estadoId, Abmc.TextControl.TextBox) %>
        <%: Html.AbmcTextControlFor(model => model.CategoriaEscuela, estadoId, Abmc.TextControl.DropDownListEnum)%>
        <%: Html.AbmcTextControlFor(model => model.Ambito, estadoId, Abmc.TextControl.DropDownListEnum)%>
        <%: Html.AbmcTextControlFor(model => model.TipoCooperadora, estadoId, Abmc.TextControl.DropDownListEnum)%>
        <%: Html.AbmcTextControlFor(model => model.Dependencia, estadoId, Abmc.TextControl.DropDownListEnum)%>
    </fieldset>
    <div id="divTurnosEscuelas">
        <fieldset>
        <legend>Turnos</legend>
        <%: Html.AbmcSelectControlFor(model => model.TurnoId, estadoId, Abmc.SelectControl.DropDownList, turno, Constantes.Seleccione)%>
        <%-- Agrego una grilla de carga rápida para seleccionar los turnos --%>
        <p class="botones">
            <% if (estadoText != "Ver"){%>
                <%:Html.BtnGenerico(Botones.ButtonType.button, "Agregar", string.Empty, string.Empty,"btnAgregarTurno")%>
                <%:Html.BtnGenerico(Botones.ButtonType.button, "Eliminar", string.Empty, string.Empty,"btnEliminarTurno")%>
            <% } %>
        </p>
        <table id="listTurnos" cellpadding="0" cellspacing="0">
        </table>
        </fieldset>
    </div>    
    
    <div id="divPeriodoLectivos">
        <fieldset>
        <legend>Período Lectivo</legend>
            <%: Html.AbmcSelectControlFor(model => model.PeriodoLectivoId, estadoId, Abmc.SelectControl.DropDownList, periodoLectivo, Constantes.Seleccione)%>
            <p class="botones">
                <input id="btnAgregarPeriodoLectivo" type="button" value="Agregar" />
                <input id="btnEliminarPeriodoLectivo" type="button" value="Eliminar" />
            </p> 
            <table id="listPeriodosLectivos"></table>
        </fieldset>
    </div>
   
    <div id="divEstructuraEscolar">
        <fieldset>
            <legend>Estructura escolar</legend>
            <p>
                <%:Html.Label("Registrar estructura escolar:")%>
                <%:Html.CheckBox("RegistrarEstructuraEscolarCheck", false)%>
            </p>
            <div id="divAreaEstructuraEscolar" style="display: none">
                <% if (Model.EstructuraEscolar != null)
                {
                    Model.EstructuraEscolar = null;
                }%>
                <%: Html.EditorFor(model => model.EstructuraEscolar) %>
            </div>
            <div id="grillaEstructura">
                <table id="listaEstructura" cellpadding="0" cellspacing="0">
                </table>
                <div id="pagerEstructura">
                </div>
            </div>
            <div id="divEstructuraDefinitiva" style="display:none;">
                <%: Html.AbmcCheckControlFor(model => model.EstructuraDefinitiva, estadoId, Abmc.CheckControl.CheckBox)%>
            </div>
        </fieldset>
    </div>
    <fieldset>
        <legend>Datos adicionales escuela</legend>
        <%--<%: Html.AbmcSelectControlFor(model => (int)model.CategoriaEscuela, estadoId, Abmc.SelectControl.DropDownList, categorias)%>--%>        
        <%: Html.AbmcCheckControlFor(model => model.Religioso, estadoId, Abmc.CheckControl.CheckBox)%>
        <%: Html.AbmcCheckControlFor(model => model.Arancelado, estadoId, Abmc.CheckControl.CheckBox)%>
        <%: Html.AbmcCheckControlFor(model => model.Albergue, estadoId, Abmc.CheckControl.CheckBox)%>        
        <%: Html.AbmcSelectControlFor(model => model.TipoJornada, estadoId, Abmc.SelectControl.DropDownList, tipoJornada, Constantes.Seleccione)%>
        <%: Html.AbmcSelectControlFor(model => model.ModalidadJornada, estadoId, Abmc.SelectControl.DropDownList, modalidadJornada, Constantes.Seleccione)%>
        <%: Html.AbmcCheckControlFor(model => model.ContextoDeEncierro, estadoId, Abmc.CheckControl.CheckBox)%>
        <%: Html.AbmcCheckControlFor(model => model.EsHospitalaria, estadoId, Abmc.CheckControl.CheckBox)%>
        <%: Html.AbmcTextControlFor(model => model.HorarioDeFuncionamiento, estadoId, Abmc.TextControl.TextBox)%>
        <%: Html.AbmcTextControlFor(model => model.Colectivos, estadoId, Abmc.TextControl.TextBox)%>
    </fieldset>
    </div>
    <div id="divEscuelaPrivada">
        <%: Html.AbmcCheckControlFor(model => model.Privado, estadoId, Abmc.CheckControl.CheckBox)%>

        <div id="divEsPrivado" style="display:none;">
            <fieldset>
                <legend>Escuela privada</legend>
                <div id="divDirector">
                    <%: Html.EditorFor(model => model.Director) %>
                </div>
                <div id="divRepresentanteLegal">
                    <%: Html.EditorFor(model => model.RepresentanteLegal) %>
                </div>
                <%: Html.AbmcSelectControlFor(model => model.SucursalBancariaId, estadoId, Abmc.SelectControl.DropDownList, sucursalBancaria, Constantes.Seleccione)%>
                <%: Html.AbmcTextControlFor(model => model.NumeroCuentaBancaria, estadoId, Abmc.TextControl.TextBox)%>
                <%: Html.AbmcTextControlFor(model => model.PorcentajeAporteEstado, estadoId, Abmc.TextControl.TextBox)%>
                <%: Html.AbmcSelectControlFor(model => model.ObraSocialId, estadoId, Abmc.SelectControl.DropDownList, obraSocial, Constantes.Seleccione)%>
            </fieldset>
        </div>
    </div>
    <div id="divEscuelaZonaDesfavorable">
        <%: Html.AbmcSelectControlFor(model => model.ZonaDesfavorableId, estadoId, Abmc.SelectControl.DropDownList, zonaDesfavorable, Constantes.Seleccione)%>
        <div id="divContenedorIL" style="display : none;">
            <div id="divInstrumentoLegalParaZonaDesfaforable">        
                <% Html.RenderPartial("AsignacionInstrumentoLegalEditor", new AsignacionInstrumentoLegalModel());%>
            </div>
            <div id="divFecNotificacionAsignacionILZD">
                <%:Html.AbmcTextControlFor(model => model.FecNotificacionAsignacionILZD, estadoId, Abmc.TextControl.Calendar)%>  
            </div>           
        </div>
    </div>
    </div>

</fieldset>
   
<% } %> 