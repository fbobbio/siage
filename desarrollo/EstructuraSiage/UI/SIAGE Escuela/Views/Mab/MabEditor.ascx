<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.MabModel>" %>
<%@ Import Namespace="Siage.Core.Domain" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Ver";
    int estadoId = (estadoText != "Ver") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Ver;

    var tipoNovedad = Html.CreateSelectList<TipoNovedadModel>(Siage.Base.ViewDataKey.TIPO_NOVEDAD_MAB.ToString(), "Id",
                                                              "Tipo", Model.TipoNovedadId);
    var situacionDeRevista =
        Html.CreateSelectList<SituacionDeRevistaModel>(Siage.Base.ViewDataKey.SITUACION_REVISTA.ToString(),
                                                       Model.SituacionRevistaId);
    var modalidad = Html.CreateSelectList<ModalidadMabModel>(Siage.Base.ViewDataKey.MODALIDAD_MAB.ToString(), "Id",
                                                             "Descripcion", Model.ModalidadId);

    var codigoNovedad =
        Html.CreateSelectList<CodigoMovimientoMabModel>(Siage.Base.ViewDataKey.CODIGO_MOVIMIENTO_MAB.ToString(), "Id",
                                                        "CodigoYDescripcion", Model.CodigoDeNovedadId);
%>
<fieldset>
    <legend><%: estadoText %> MAB</legend>
    <%: Html.HiddenFor(model => model.Id)%>

    <div id="divMab">

        <%-- Cabecera Mab --%>
        <div id="divHeadMab">
            <p> <%: Html.Label("Fecha actual:") %> <%: Html.TextBox("FechaActual") %> </p>
            <div id="divTipoNovedad">
                <fieldset>
                    <legend>Tipo de novedad</legend>
                    <p> <%: Html.AbmcSelectControlFor(model => model.TipoNovedadId, estadoId, Abmc.SelectControl.DropDownList, tipoNovedad)%>                    
                        <%:Html.BtnGenerico(Botones.ButtonType.button, "Seleccionar tipo de novedad", string.Empty, string.Empty, "btnSeleccionarNovedad")%> </p>
                </fieldset>
            </div>
        </div>
        <%-- Cabecera Mab --%>
        
        <%-- Cuerpo Mab --%>
        <div id="divBodyMab" style="display:none">
            <div id="divEmpresa">
                <fieldset>
                    <legend>Datos de Empresa</legend>
                    <p><%: Html.Label("Código empresa:") %> <%: Html.TextBox("CodigoEmpresa") %></p>
                    <p><%: Html.Label("Nombre empresa:") %> <%: Html.TextBox("NombreEmpresa") %></p>
                </fieldset>
            </div>
            <div id="divDatosAgente">
                <fieldset>
                    <legend>Datos agente</legend>
                    <%-- Falta mostrar los datos del domicilio del agente seleccionado. (COCO)
                         Ver como mostrar los datos del instrumento legal asociado al agente. --%>
                    <div id="divConsultarAgenteMab">
                        <% Html.RenderPartial("ConsultarAgenteEditor", new AgenteModel()); %>
                    </div>
                    <p class="botones">
                        <%:Html.BtnGenerico(Botones.ButtonType.button, "Modificar Domicilio", string.Empty, string.Empty, "btnModificarDomicilioAgente")%>
                    </p>
                </fieldset>
            </div>

            <div id="divTipoMovimiento" style="display:none">
            <fieldset>
                <legend>Movimiento</legend>
                <p>
                    <label>Puesto de trabajo actual</label>
                    <%: Html.RadioButton("radioBtn", "Puesto de trabajo actual", false, new { id = "rdbPuestoActual" })%></p>
                <p>
                    <label>Nuevo puesto de trabajo</label>
                    <%: Html.RadioButton("radioBtn", "Nuevo puesto de trabajo", false, new { id = "rdbPuestoNuevo" })%></p>
                <p class="botones">
                    <%:Html.BtnGenerico(Botones.ButtonType.button, "Seleccionar movimiento", string.Empty, string.Empty, "btnSeleccionarMovimiento")%></p>
            </fieldset>
            </div>
            <%--<div id="divVacantes">
                <fieldset>
                    <legend>Vacantes</legend>
                </fieldset>
            </div>--%> 
            <%-- TODO: Esto no entra para fase 2. --%>

            
            <div id="divOcultarPorMovimiento">
            <div id="divPuestoDeTrabajo">
                <fieldset>
                    <legend>Puesto de trabajo</legend>

                    <div id="divConsultarPuestoDeTrabajo">
                        <% Html.RenderPartial("ConsultarPuestoDeTrabajoEditor"); %>
                    </div>

                    <!-- SE SACA ESTO PORQUE LA ASIGNACIÓN PUEDE SER SÓLO 1
                    <div id="divListaAsignacionesRelacionadasAPuestoYAgente" style="display:none;">
                        <div id="divListaAsignaciones" style="display:none;">
                            <table id="listAsignaciones" cellpadding="0" cellspacing="0" width="50%"></table>
                            <div id="pagerListAsignaciones"></div>
                        </div>
                        <div id="divDatosAsignacionSeleccionada" style="display:none;">
                            <p> < %: Html.Label("Asignacion seleccionada: ") %> < %: Html.TextBox("txtAsignacionSeleccionada") %> </p>
                        </div>
                    </div> -->
                    
                    <div id="divModalidadPuestoTrabajo" style="display:none">
                        <%-- Seleccionar la modalidad si el puesto de trabajo está relacionado 
                             con un plan de estudio. --%>
                        <%: Html.AbmcSelectControlFor(model => model.ModalidadId, estadoId, Abmc.SelectControl.DropDownList, modalidad)%>
                    </div>

                    <%--<div id="divAgentePuestoDeTrabajoSeleccionadoMabAusentismo" style="display:none;">
                        <% Html.RenderPartial("ConsultarPuestoDeTrabajoEditor"); %>
                    </div>--%>

                    <div id="divHorarioPuestoTrabajo">
                        <%-- Mostrar los horarios del puesto de trabajo seleccionado. 
                             Pueden ser horas cátedras si está asociado a un plan de estudio
                             o simplemente las horas del puesto de trabajo normal. --%>
                    </div>

                    <div id="divPuestoTrabajoProlongacionItinerante">
                        <%-- De acuerdo al tipo de trabajo, si es prolongacion o itinerante
                             mostrar el código de empresa y la cantidad de horas, si no 
                             mostrar los datos del agente. --%>
                    </div>

                </fieldset>
            </div>

            <div id="divDatosAgenteReemplazado" style="display:none;">
                <fieldset>
                    <legend>Agente a quién reemplaza</legend>
                    <div id="divDatosABuscarAgenteReemplazado">
                        <% Html.RenderPartial("ConsultarAgenteEditor", new AgenteModel()); %>
                    </div>                    
                </fieldset>
            </div>
            <div id="divLabelAgenteReemplazadoInexistente" style="display:none;">
                <p><%: Html.Label("No existe agente a reemplazar en el puesto de trabajo seleccionado")%></p>
            </div>

            <div id="divCodigoNovedad">
                <%: Html.AbmcSelectControlFor(model => model.CodigoDeNovedadId, estadoId, Abmc.SelectControl.DropDownList, codigoNovedad) %>
            </div>
        
            <div id="divSituacionDeRevista">
                <%: Html.AbmcSelectControlFor(model => model.SituacionRevistaId, estadoId, Abmc.SelectControl.DropDownList, situacionDeRevista) %>
            </div>             

            <div id="divFechasDeNovedad">
                <fieldset>
                    <legend>Fechas de novedad</legend>
                    <div id="divSeleccionarFechasnovedad">
                        <%: Html.AbmcTextControlFor(model => model.FechaNovedadDesde, estadoId, Abmc.TextControl.Calendar) %>
                        <%: Html.AbmcTextControlFor(model => model.FechaNovedadHasta, estadoId, Abmc.TextControl.Calendar) %>
                    </div>
                </fieldset>
            </div>

            <div id="divInstrumentoLegalMab">
                <fieldset>
                    <legend>Acto administrativo</legend>
                <%-- Llamar al caso de uso instrumento legal. --%>
                <div id="divInstrumentoLegalPendiente">
                    <p><%: Html.Label("Tipo 13 - Pendiente de instrumento legal") %> </p>
                </div>
                <p><%: Html.Label("Ingresar datos acto administrativo:") %> <%: Html.CheckBox("CargarInstrumentoLegalCheck") %></p>
                <div id="divCargarInstrumentoLegal" style="display:none">
                    <p> <% Html.RenderPartial("AsignacionInstrumentoLegalEditor", new AsignacionInstrumentoLegalModel());%> </p>
                </div>
                </fieldset>
            </div>            

            <div id="divSucursales" style="display:none">
                <fieldset>
                    <legend>Sucursal bancaria</legend>
                    <div id="divExisteSucursalBancaria">
                        <p><%: Html.Label("Sucursal bancaria:") %> <%: Html.TextBox("SucursalBancariaText") %></p>    
                    </div>
                    <div id="divSeleccionSucursal">
                        <p><%: Html.Label("Seleccionar sucursal bancaria:") %> <%: Html.CheckBox("SeleccionarSucursalBancaria") %></p><br /><br />
                        <div id="divGrillaSucursalBancaria">
                            <table id="listSucursales" cellpadding="0" cellspacing="0" width="50%"></table>
                            <div id="pagerSelect"></div>
                        </div>
                        <div id="divCodigoSucursal">
                            <p><%: Html.Label("Ingresar código sucursal bancaria:") %> <%: Html.TextBox("CodigoSucursalBancaria") %> <%:Html.BtnGenerico(Botones.ButtonType.button, "Cargar Sucursal", string.Empty, string.Empty, "btnCargarSucursal")%></p>
                        </div>
                    </div>
                </fieldset>
            </div>

            <div id="divDatosCargoAnterior">
                <fieldset>
                    <legend>Registrar datos del cargo anterior</legend>
                    <p> <%: Html.AbmcCheckControlFor(model => model.RegistrarCargoAnterior, estadoId, Abmc.CheckControl.CheckBox)%></p>
                    <div id="divSeleccionEmpresaMinisterio" style="display:none">
                        <p> <br /><%: Html.AbmcCheckControlFor(model => model.EsCargoDeEmpresaMinisterio, estadoId, Abmc.CheckControl.CheckBox) %> </p>
                        <div id="divEsEmpresaDelMinisterio" style="display:none">
                           <p> <% Html.RenderPartial("ConsultarPuestoDeTrabajoEditor"); %> </p>
                        </div>
                        <div id="divNoEsEmpresaDelMinisterio" >
                            <p> <%: Html.AbmcTextControlFor(model => model.ObservacionesCargoAnterior, estadoId, Abmc.TextControl.TextArea) %> </p>
                        </div>
                    </div>
                </fieldset>
            </div>

            <%-- Las observaciones se van a concatenar y se van a guardar en un solo campo en 
                 la base de datos. (Observaciones = ObservacionesCargoAnterior + Observaciones) --%>

            <div id="divObservaciones">
                <%: Html.AbmcTextControlFor(model => model.ObservacionesMab, estadoId, Abmc.TextControl.TextArea)%>
            </div>
            <div id="divImprimir">
                <p> <%: Html.Label("Imprimir Mab:")%> <%: Html.CheckBox("DeseaImprimir")%></p>                
            </div>
            </div>
        </div>
        <%-- Cuerpo Mab --%>

    </div>

    <p><%: Html.AjaxAbmcBotones(estadoId)%></p>

</fieldset>

<script type="text/javascript">
    $(document).ready(function () {
        Mab.init("<%: estadoText %>");
    });
</script>