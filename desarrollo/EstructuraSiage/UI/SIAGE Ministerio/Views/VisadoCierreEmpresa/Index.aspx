<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Siage.Services.Core.Models.EmpresaVisarModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% if (Model == null)
       { %>
            <%: Html.ActionLink("Consultar Empresa", "Index", "Empresa")%>
            <%: Html.ActionLink("Volver", "Index", "Home") %>
    <% }
       else
       { %>
       

        <%
        using (Html.BeginForm())
        {
        %>
  <div id="divConsulta">
         <% Html.RenderPartial("ConsultarEmpresaEditor"); %>
  </div>
   <div id="divVista">
            <fieldset>
                <legend>Visado de Empresa</legend>
                <%: Html.HiddenFor(model => model.Id)%>

                <%-------------------------------------- INICIO AREA EDITABLE --------------------------------------%>
                <%: Html.Label("Codigo Empresa")%> <%: Html.TextBox("CodigoEmpresa", Model.CodigoEmpresa, new { @Disabled = "disabled" })%>
                <%: Html.Label("CUE")%> <%: Html.TextBox("CUE", Model.CUE, new { @Disabled = "disabled" })%>
                <%: Html.Label("Nombre")%> <%: Html.TextBox("Nombre", Model.Nombre, new { @Disabled = "disabled" })%>
                <%: Html.Label("Tipo Educacion")%> <%: Html.DropDownListEnum("TipoEducativo", typeof(Siage.Base.TipoEducacionEnum), new { @Disabled = "disabled" })%>
                <%: Html.Label("Nivel Educativo")%> <%: Html.DropDownListEnum("NivelEducativo", typeof(Siage.Base.NivelEducativoEnum), new { @Disabled = "disabled" })%>
                <%: Html.Label("Tipo Empresa")%> <%: Html.DropDownListEnum("TipoEmpresa", typeof(Siage.Base.TipoEmpresaEnum), new { @Disabled = "disabled" })%>
                
                <%---------------------------------------- FIN AREA EDITABLE ---------------------------------------%>

                <%: Html.Label("Accion")%> <%: Html.RadioButton("Accion", "Autorizar", true)%> <%: Html.RadioButton("Accion", "Rechazar")%>

                <div id="AreaObservaciones"><%: Html.TextArea("Observaciones")%></div>

                <%: Html.ValidationSummary("La operacion no pudo completarse")%>
                <%: Html.BtnAceptarCancelar("Home/Index.aspx")%>
            </fieldset>

  </div>
        <%
        }
        %>

    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        $(document).ready(
            function () {
                $("#Observaciones").hide();
//                if (<%:(int)ViewData[AjaxAbmc.EstadoId]%> == <%: (int) EstadoABMC.Consultar %>) 
//                {

//                    $("#divConsulta").show();

//                    $("#divVista").hide();
//                }
//                else {
//                    $("#divConsulta").hide();
//                    $("#divVista").show();
//                }
            });

        $("#Accion").change(
            function () {
                if ($("#Accion").val() == "Rechazar") {
                    $("#AreaObservaciones").show();
                }
                else {
                    $("#AreaObservaciones").hide();
                }
            });
    </script>
</asp:Content>