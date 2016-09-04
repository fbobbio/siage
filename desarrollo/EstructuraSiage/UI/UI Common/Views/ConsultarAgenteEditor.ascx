<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.AgenteModel>" %>
<%@ Import Namespace="Siage.Services.Core.Models" %>

<%
    var tiposDocumento = Html.CreateSelectList<TipoDocumentoModel>(Siage.Base.ViewDataKey.TIPO_DOCUMENTO.ToString());
    var departamentoProvincial = Html.CreateSelectList<DepartamentoProvincialModel>(Siage.Base.ViewDataKey.DEPARTAMENTO_PROVINCIAL.ToString());
    var localidad = Html.CreateSelectList<LocalidadModel>(Siage.Base.ViewDataKey.LOCALIDAD.ToString());
    var nivelEducativo = Html.CreateSelectList<NivelEducativoModel>(Siage.Base.ViewDataKey.NIVEL_EDUCATIVO.ToString());
    var direccionNivel = Html.CreateSelectList<DireccionDeNivelComboModel>(Siage.Base.ViewDataKey.DIRECCION_NIVEL.ToString());
    var situacionRevista = Html.CreateSelectList<SituacionDeRevistaModel>(Siage.Base.ViewDataKey.SITUACION_REVISTA.ToString());
    var tipoCargo = Html.CreateSelectList<TipoCargoModel>(Siage.Base.ViewDataKey.TIPO_CARGO.ToString());
    var asignatura = Html.CreateSelectList<AsignaturaModel>(Siage.Base.ViewDataKey.ASIGNATURA.ToString());
    var titulo = Html.CreateSelectList<TituloModel>(Siage.Base.ViewDataKey.TITULO.ToString());
%>

<div id="divConsulta">
    <fieldset>
        <legend>Agente</legend>

        <div id="divFiltroBasico">
            <p><%: Html.Label("Tipo documento: ") %><%: Html.DropDownList("FiltroTipoDocumento", tiposDocumento, Constantes.Seleccione)%></p>
            <p><%: Html.Label("Número documento: ") %><%: Html.TextBox("FiltroNumeroDocumento") %></p>            
            <p><%: Html.Label("Sexo: ") %><%:Html.DropDownListEnum("FiltroSexo", typeof(Siage.Base.SexoEnum)) %></p>
            <p><%: Html.Label("Apellido: ") %><%: Html.TextBox("FiltroApellido")%></p>
            <p><%: Html.Label("Nombre: ") %><%: Html.TextBox("FiltroNombre")%></p>
        </div>
            
        <p><%: Html.Label("Búsqueda avanzada: ") %><%: Html.CheckBox("chkBusquedaAvanzada") %></p>
            
        <div id="divFiltroAvanzado">
            <fieldset>
                <legend>Filtro avanzado</legend>

                <p><%: Html.Label("Departamento: ")%><%: Html.DropDownList("FiltroDepartamentoProvincial", departamentoProvincial, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Localidad: ") %><%: Html.DropDownList("FiltroLocalidad", localidad, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Número legajo Siage: ") %><%: Html.TextBox("FiltroNroLegajoSiage") %></p>
                <p><%: Html.Label("Número legajo media: ") %><%: Html.TextBox("FiltroNroLegajoMedia") %></p>
                <p><%: Html.Label("Número legajo inicial: ") %><%: Html.TextBox("FiltroNroLegajoInicial") %></p>
                <p><%: Html.Label("Nombre empresa: ") %><%: Html.TextBox("FiltroNombreEmpresa") %></p>
                <p><%: Html.Label("Nivel educativo: ")%><%: Html.DropDownList("FiltroNivelEducativo", nivelEducativo, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Dirección de nivel: ")%><%: Html.DropDownList("FiltroDireccionNivel", direccionNivel, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Situación revista: ")%><%: Html.DropDownList("FiltroSituacionRevista", situacionRevista, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Tipo de cargo: ")%><%: Html.DropDownList("FiltroTipoCargo", tipoCargo, Constantes.Seleccione)%></p>
                <p><%: Html.Label("Asignatura: ")%><%: Html.DropDownList("FiltroAsignatura", asignatura, Constantes.Seleccione)%></p>

                <fieldset>
                    <legend>Ingreso</legend>
                    <p><%: Html.Label("Fecha desde: ") %><%: Html.DateTimeTextBox("FiltroFechaDesdeAlta") %></p>
                    <p><%: Html.Label("Fecha hasta: ") %><%: Html.DateTimeTextBox("FiltroFechaHastaAlta") %></p>
                </fieldset>

                <fieldset>
                    <legend>Baja</legend>
                    <p><%: Html.Label("Fecha desde: ") %><%: Html.DateTimeTextBox("FiltroFechaDesdeBaja") %></p>
                    <p><%: Html.Label("Fecha hasta: ") %><%: Html.DateTimeTextBox("FiltroFechaHastaBaja") %></p>
                </fieldset>

                <p><%: Html.Label("Título: ")%><%: Html.DropDownList("FiltroTitulo", titulo, Constantes.Seleccione) %></p>
                <p><%: Html.Label("Tipo agente: ")%><%:Html.DropDownListEnum("FiltroTipoAgente", typeof(Siage.Base.TipoAgenteEnum)) %></p>
            </fieldset>
        </div>

        <p class="botones">
            <button id="btnBuscar" type="button" >Consultar</button>
            <button id="btnLimpiar" type="button" >Limpiar</button>
        </p>
    </fieldset>

    <table id="seleccionAgenteList" cellpadding="0" cellspacing="0" ></table>
    <div id="pagerSelect" ></div>
</div>

<div id="divVista" style="display:none;" ></div>