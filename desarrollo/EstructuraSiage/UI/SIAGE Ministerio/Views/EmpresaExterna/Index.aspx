<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AjaxAbmc.Master" Inherits="System.Web.Mvc.ViewPage<Siage.Services.Core.Models.EmpresaExternaModel>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TituloContent" runat="server"> Empresa Externa
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FiltrosBusquedaContent" runat="server">
    <p><%: Html.Label("Nombre Empresa:") %> <%: Html.TextBox("filtroNombreEmpresa") %></p>
    <p><%: Html.Label("Razon Social:") %> <%: Html.TextBox("filtroNombreRazonSocial") %></p>
    <p><%: Html.Label("Cuil:") %> <%: Html.TextBox("filtroCuil") %></p>
    <p><%: Html.Label("Cuit:") %> <%: Html.TextBox("filtroCuit") %></p>
    <p><%: Html.Label("Tipo de Empresa :") %> <%: Html.DropDownListEnum("filtroTipoEmpresa", typeof(Siage.Base.TipoEmpresaExternaEnum))%></p>
    <p><%: Html.Label("Incluir dados de baja:")%><input id="chkBusquedaEmpresaExternasEliminadas" type="checkbox" /></p>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/jquery.cascadingDropDown.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.personaFisica.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.personaJuridica.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.domicilio.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/siage.EmpresaExterna.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            EmpresaExterna.init();
        });
   </script>
</asp:Content>

                                                                