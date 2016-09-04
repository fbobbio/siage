<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Siage.Services.Core.Models.EmpresaRegistrarModel>" %>

<%
    string vista = (string)ViewData[Constantes.VistaEmpresa] ?? "RegistrarEmpresaEditor";
    string estadoText = (string)ViewData[AjaxAbmc.EstadoText] ?? "Consultar";
    int estadoId = (estadoText != "Consultar") ? (int)ViewData[AjaxAbmc.EstadoId] : (int)EstadoABMC.Consultar;
    string usuarioLogueado = (string)ViewData[Constantes.TipoUsuarioLogueado];
    
    
    
    using (Html.BeginForm())
    {
%>


<%//TODO: ver en dnd van cada uno de los hidden %>
                   
            

            
            



            


<% } %>