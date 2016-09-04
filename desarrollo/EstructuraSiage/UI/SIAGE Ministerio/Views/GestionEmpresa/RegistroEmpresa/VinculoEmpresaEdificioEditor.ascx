<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Siage.Base" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? EstadoABMC.Registrar.ToString();
    var funcionEdificio = Html.CreateSelectList<FuncionEdificioModel>(Siage.Base.ViewDataKey.FUNCION_EDIFICIO.ToString());
    var departamentoProvincial = Html.CreateSelectList<DepartamentoProvincialModel>(Siage.Base.ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString());
    var localidad = Html.CreateSelectList<LocalidadModel>(Siage.Base.ViewDataKey.LOCALIDAD.ToString());
%>

<div id="divVincularEdificioAEmpresa" style="display: none">
    <div id="divGrillaEdificios" style="clear: both;">         
                   
        <fieldset>
            <legend>Selección Edificio</legend>

            <div id="divConsultaEdificio">
                <div id="divFiltrosDeConsultaEdificio">
                    <p>
                        <%: Html.Label("Tipo de edificio:") %>
                        <%: Html.DropDownListEnum("TipoEdificioConsulta", typeof(TipoEdificioEnum)) %></p>
                    <p>
                        <%: Html.Label("Identificador edificio:") %>
                        <%: Html.TextBox("IdentificadorEdificioConsulta") %></p>
                    <p>
                        <%: Html.Label("Función de edificio:") %>
                        <%: Html.DropDownList("FuncionEdificioConsulta", funcionEdificio, Constantes.Seleccione)%></p>
                    <p>
                        <%: Html.Label("Identificador predio:") %>
                        <%: Html.TextBox("IdentificadorPredioConsultaEdificio")%></p>
                    <p>
                        <%: Html.Label("Descripciófun predio:") %>
                        <%: Html.TextBox("DescripcionPredioConsultaEdificio")%></p>
                    <p>
                        <%: Html.Label("Nombre casa habitación:") %>
                        <%: Html.TextBox("NombreCasaHabitacionConsulta")%></p>

                    <fieldset>
                        <legend>Domicilio</legend>
                        <p>
                            <%: Html.Label("Departamento:")%>
                            <%: Html.DropDownList("FiltroDepartamentoProvincial", departamentoProvincial, Constantes.Seleccione)%></p>
                        <p>
                            <%: Html.Label("Localidad:")%>
                            <%: Html.DropDownList("FiltroLocalidad", localidad, Constantes.Seleccione)%></p>
                        <p>
                            <%: Html.Label("Barrio:")%>
                            <%: Html.DropDownList("FiltroBarrio", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                        <p>
                            <%: Html.Label("Calle:")%>
                            <%: Html.DropDownList("FiltroCalle", Html.CreateEmptySelectList(), Constantes.Seleccione)%></p>
                        <p>
                            <%: Html.Label("Altura:")%>
                            <%: Html.TextBox("FiltroAltura") %></p>
                    </fieldset>
                </div>

                <p class="botones">
                <% if (estadoText != EstadoABMC.Ver.ToString()) { %>
                    <%: Html.BtnGenerico(Botones.ButtonType.button, "Consultar", string.Empty, string.Empty, "btnConsultarEdificio")%>
                    <%: Html.BtnGenerico(Botones.ButtonType.button, "Limpiar", string.Empty, string.Empty, "btnLimpiarConsultaEdificio")%></p>    
                <% } %>
            </div>

            <table id="listEdificios" cellpadding="0" cellspacing="0" />
            <div id="pagerDetalleEdificio" />
        </fieldset>                       
    </div>
                   
    <div id="divDatosDelVinculo">
    <fieldset>
        <legend>Datos del Vínculo</legend>
        <p>
            <%:Html.Label("Fecha Desde (*):")%>
            <%:Html.DateTimeTextBox("FechaDesdeVinculo")%></p>
        <p>
            <%:Html.Label("Observaciones:")%>
            <%:Html.TextArea("ObservacionVinculo")%></p>
    </fieldset>
    </div>
                                              
    <% if (estadoText != EstadoABMC.Ver.ToString()) { %>
        <%: Html.BtnGenerico(Botones.ButtonType.button, "Vincular a empresa", string.Empty, string.Empty,"btnVincularEdificio")%>
    <% } %>

    <div id="divGrillaVinculos" style="clear: both;">
        <table id="listVinculos" cellpadding="0" cellspacing="0" />
        <div id="pagerVinculos" />
                            
        <% if (estadoText != "Ver") { %>
            <%: Html.BtnGenerico(Botones.ButtonType.button, "Borrar", string.Empty, string.Empty, "btnBorrarVinculo")%>
        <% } %>                      
    </div>

    <div id="divDomicilio">
        <fieldset>
            <legend>Domicilio</legend>
            <div id="DivGrillaDomicilio" style="clear: both;">
                <table id="listDomicilio" cellpadding="0" cellspacing="0" />
                <div id="pagerDomicilio" />
            </div>
            <div id="divDatosGeneralesDomicilio" style="display: none;">
                <%: Html.Label("Calle:") %><%: Html.TextBox("Calle") %>
                <%: Html.Label("Altura:") %><%: Html.TextBox("Altura") %>
                <%: Html.Label("Barrio:") %><%: Html.TextBox("Barrio") %>
                <%: Html.Label("Localidad:") %><%: Html.TextBox("Localidad") %>                  
                <%: Html.Label("Departamento provincial:")%><%: Html.TextBox("DepartamentoProvincial")%>                                  
            </div>
            <%--aca deberian estar el combo de las calles y el textbox para la altura--%>
            <div id="divNuevoDomicilio" style="display: none;">
                <p>
                    <%: Html.Label("Calle:") %>
                    <select id="comboCalles">
                        <option value="">SELECCIONE</option>
                    </select>
                </p>
                <p>
                    <%: Html.Label("Altura:") %>
                    <%: Html.TextBox("AlturaNueva") %>
                </p>                
            </div>
            
            <p class="botones">
                <%: Html.BtnGenerico(Botones.ButtonType.button ,"Aceptar", string.Empty, string.Empty, "btnAceptarNuevoDomicilio") %>
                <%: Html.BtnGenerico(Botones.ButtonType.button, "Buscar domicilio", string.Empty, string.Empty, "btnVolverDomicilio")%>
            </p>

        </fieldset>
    </div>
</div>