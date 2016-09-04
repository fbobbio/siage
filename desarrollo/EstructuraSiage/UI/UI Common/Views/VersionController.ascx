<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div id="numeroVersion">Versión Alpha 1.04.39.55</div>
<% 
    var ConString = ConfigurationManager.ConnectionStrings["ORACLE-SIAGE"].ConnectionString;
    string[] ConStringArray = ConString.Split(';');
    var nombreBaseDatos = ConStringArray[ConStringArray.Length-2].Split('=')[1];
 %>
<div id="numeroVersion">Base de datos <%: nombreBaseDatos  %></div>