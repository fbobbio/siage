Vinculo = {};
Vinculo.Prefijo = "";
Vinculo.EstadoVinculo;
Vinculo.init = function (data) {
    $("#btnAddVinculo").hide();
    $("#btnCleanVinculo").hide();
    Vinculo.Prefijo = data;
    var grillaVinculos = $("#listaVinculos").jqGrid({
        datatype: "vinculoFamiliar",
        colNames: ['Id', 'Tipo Documento', 'Nro. Documento', 'Nombre', 'Apellido', 'Sexo', 'Tipo Vínculo_Id', 'Tipo Vínculo', 'Ocupacion', 'Telefono', 'Vive', 'Permiso Retiro'],
        colModel: [
                    { name: 'Id', index: 'Id', hidden: true },
                    { name: 'Pariente.TipoDocumento', index: 'TipoDocumento', hidden: false },
                    { name: 'Pariente.NumeroDocumento', index: 'NroDocumento', hidden: false },
                    { name: 'Pariente.Nombre', index: 'Nombre', hidden: false },
                    { name: 'Pariente.Apellido', index: 'Apellido', hidden: false },
                    { name: 'Pariente.IdSexo', index: 'IdSexo', hidden: true },
                    { name: 'VinculoId', index: 'VinculoId', hidden: true },
                    { name: 'VinculoNombre', index: 'VinculoNombre', hidden: false },
                    { name: 'Ocupacion', index: 'Ocupacion', hidden: true },
                    { name: 'Telefono', index: 'Telefono', hidden: true },
                    { name: 'Vive', index: 'Vive', hidden: true },
                    { name: 'PermisoRetiro', index: 'PermisoRetiro', hidden: true }
                ],
        sortname: "NroDocumento",
        sortorder: "asc",
        toppager: true,
        rowNum: 30,
        viewrecords: true,
        autowidth: true,
        height: "100%",
        caption: "Vínculo Familiar",
        loadtext: 'Cargando, espere por favor',
        emptyrecords: 'No hay datos para mostrar',
        onSelectRow: function (id) {
            //Configuracion.DetalleSeleccionado = grillaDetalles.getRowData(id)
        }
    });

    $("#listaVinculos_toppager_center", "#listaVinculos_toppager").remove();
    $(".ui-paging-info", "#listaVinculos_toppager").remove();
    $("#listaVinculos_toppager_left").append('<table cellspacing="0" cellpadding="0" border="0" style="float: left; table-layout: auto;" class="ui-pg-table navtable"><tbody><tr></tr></tbody></table>');

    //Creo el boton agregar de la grilla
    $("#listaVinculos").navButtonAdd("#listaVinculos_toppager_left",
            {
                position: "first",
                caption: "Agregar",
                title: "Agregar",
                buttonicon: "ui-icon-plus",
                onClickButton: function () {
                    Vinculo.EstadoVinculo = "Registrar";
                    $("#divPersonaFisicaVinculo").show();
                    Vinculo.OcultarBotones();
                    $("#btnCleanVinculo").show();
                }
            });

    //Creo el boton editar de la grilla
    $("#listaVinculos").navButtonAdd("#listaVinculos_toppager_left",
            {
                position: "last",
                caption: "Editar",
                title: "Editar",
                buttonicon: "ui-icon-pencil",
                onClickButton: function () {


                    var seleccion = $("#listaVinculos").getRowData(parseInt($("#listaVinculos").getGridParam("selrow")));
                    if (seleccion != null) {
                        Vinculo.EstadoVinculo = "Editar";
                        Vinculo.OcultarBotones();
                        $("#btnCleanVinculo").show();
                        var dniEstudiante;
                        var tipoDniEstudiante;
                        var sexo;
                        //Aqui buscamos la persona fisica del pariente que se tiene que mostrar en el control de persona
                        for (var item in seleccion) {
                            if (item == "Pariente.TipoDocumento") {
                                $("#Vinculos_cmbTipoDocumento").val(seleccion[item]);
                                tipoDniEstudiante = seleccion[item];
                            }
                            if (item == "Pariente.NumeroDocumento") {
                                $("#Vinculos_txtNumeroDocumento").val(seleccion[item]);
                                dniEstudiante = seleccion[item];
                            }
                            if (item == "Pariente.IdSexo") {
                                $("#Vinculos_cmbSexo").val(seleccion[item]);
                                sexo = seleccion[item];
                            }
                        }
                        $("#Vinculos_btnBuscarPF").click();
                        //Variable que contiene todos los datos temporales de la grilla
                        var registro = $("#listaVinculos").data("vinculoFamiliar") || [];
                        //Busco en los temporales el registro que corresponda con el dni del registro seleccionado
                        //y cargo los campos de vinculo familiar
                        for (var i = 0; i < registro.length; i++) {
                            if (registro[i].Pariente.NumeroDocumento == dniEstudiante) {
                                $("#divCamposVinculo #Vinculos_Id").val(registro[i].Id);
                                $("#" + Vinculo.Prefijo + "VinculoId").val(registro[i].VinculoId);
                                $("#" + Vinculo.Prefijo + "Ocupacion").val(registro[i].Ocupacion);
                                $("#" + Vinculo.Prefijo + "Telefono").val(registro[i].Telefono);
                                $("#" + Vinculo.Prefijo + "Vive").attr('checked', registro[i].Vive);
                                $("#" + Vinculo.Prefijo + "PermisoRetiro").attr('checked', registro[i].PermisoRetiro);
                            }
                        }
                    }
                    else {
                        Mensaje.Error.texto = "Seleccione un vinculo para editar.";
                        Mensaje.Error.mostrar();
                    }
                }
            });

    $("#listaVinculos").navButtonAdd("#listaVinculos_toppager_left",
             {
                 position: "last",
                 caption: "Eliminar",
                 title: "Eliminar",
                 buttonicon: "ui-icon-trash",
                 onClickButton: function () {
                     Vinculo.QuitarVinculoClick();
                 }
             });

    //Quito la propiedad de requeridos a los campos requeridos de vinculo familiar para que no lo valide el ModelState, ya que esto sera usado como control
    //y se podra optar donde se usa si cargar o no los vinculos
    $("#divCamposVinculo :input").removeClass("val-Required");
    $("#divPersonaFisicaVinculo :input").removeClass("val-Required");
    $("#divCamposVinculo").hide();

    $("#divVinculos").width(700);
    $('#listaVinculos').setGridWidth(650, true);
    $("#divPersonaFisicaVinculo").hide();

    $("#Vinculos_btnBuscarPF").click(function () {
        $("#Vinculos_divFormularioPF").one("ajaxStop", function () {
            if ($("#Vinculos_NumeroDocumento").val() != "") {
                $("#divPersonaFisicaVinculo").show();
                $("#divCamposVinculo").show();
                $("#btnAddVinculo").show();
            }
        });
    });

    $("#Vinculos_btnNuevoPF").click(function () {
        $("#divCamposVinculo").show();
        $("#btnAddVinculo").show();
    });
}

Vinculo.postCargarModeloOriginal = Abmc.postCargarModelo;
Abmc.postCargarModelo = function (estado, id) {
    if (Vinculo.postCargarModeloOriginal)
        Vinculo.postCargarModeloOriginal(estado, id);

    switch (estado) {
        case 'Editar':
            //Aca lleno la grilla con los datos que vienen del controlador
            //CargarGrillaVinculos(id);
            break;
        case 'Eliminar':
            //CargarGrillaVinculos(id);
            //Configuracion.OcultarBotones();
            break;
        case 'Ver':
            Vinculo.OcultarBotones();
            //CargarGrillaVinculos(id);
            //Configuracion.OcultarBotones();
            break;
        default:
    };
};

function CargarGrillaVinculos(id) {
    $("#listaVinculos").setGridParam({
        url: $.getUrl("/VinculoFamiliarController/GetVinculosByPersonaFisica"),
        datatype: 'json',
        postData: { id: id }
    });
    $('#listaVinculos').trigger('reloadGrid');
}

Vinculo.CancelarVinculo = function (estado) {
    $("#divCamposVinculo").hide();
    Vinculo.LimpiarCampos();
    Vinculo.MostrarBotones();
}

Vinculo.LimpiarCampos = function () {
    $("#Vinculos_Ocupacion").val("");
    $("#Vinculos_Telefono").val("");
    $("#Vinculos_Vive").attr("checked", false);
    $("#Vinculos_PermisoRetiro").attr("checked", false);
}

Vinculo.OcultarBotones = function () {
    $("#listaVinculos_toppager_left td[title='Agregar']").hide();
    $("#listaVinculos_toppager_left td[title='Editar']").hide();
    $("#listaVinculos_toppager_left td[title='Eliminar']").hide();
};

Vinculo.MostrarBotones = function () {
    $("#listaVinculos_toppager_left td[title='Agregar']").show();
    $("#listaVinculos_toppager_left td[title='Editar']").show();
    $("#listaVinculos_toppager_left td[title='Eliminar']").show();
}
Vinculo.AgregarVinculo = function () {

    if ($("#" + Vinculo.Prefijo + "VinculoId").val() == "" || $("#" + Vinculo.Prefijo + "Ocupacion").val() == "") {
        Mensaje.Error.texto = "Los datos Vinculo y Ocupacion son requeridos. Ingrese por favor.";
        Mensaje.Error.mostrar();
        return;
    }

    var Entidad = {};
    Entidad.Pariente = {};
    Entidad.Id = 0;
    if ($("#divCamposVinculo #Vinculos_Id").val() != 0 || $("#divCamposVinculo #Vinculos_Id").val() != "") {
        Entidad.Id = $("#divCamposVinculo #Vinculos_Id").val();
    }
    Entidad.Pariente.Id = 0;
    if ($("#" + Vinculo.Prefijo + "Id").val() != 0) {
        Entidad.Pariente.Id = $("#" + Vinculo.Prefijo + "Id").val();
    }

    Entidad.Pariente.Nombre = $("#" + Vinculo.Prefijo + "Nombre").val();
    Entidad.Pariente.Apellido = $("#" + Vinculo.Prefijo + "Apellido").val();
    Entidad.Pariente.TipoDocumento = $("#" + Vinculo.Prefijo + "TipoDocumento").val();
    Entidad.Pariente.NumeroDocumento = $("#" + Vinculo.Prefijo + "NumeroDocumento").val();
    Entidad.Pariente.FechaNacimiento = $("#" + Vinculo.Prefijo + "FechaNacimiento").val();
    Entidad.Pariente.EstadoCivil = $("#" + Vinculo.Prefijo + "EstadoCivil").val();
    Entidad.Pariente.Observaciones = $("#" + Vinculo.Prefijo + "Observaciones").val();
    Entidad.Pariente.OrganismoEmisorDocumento = $("#" + Vinculo.Prefijo + "OrganismoEmisorDocumento").val();
    //Entidad.Pariente.Clase = $("#" + Vinculo.Prefijo + "Clase").val();
    Entidad.Pariente.Sexo = $("#" + Vinculo.Prefijo + "Sexo").val();
    Entidad.Pariente.IdSexo = $("#" + Vinculo.Prefijo + "Sexo").val();
    Entidad.Pariente.IdPaisEmisorDocumento = $("#" + Vinculo.Prefijo + "IdPaisEmisorDocumento").val();
    Entidad.Pariente.IdPaisNacionalidad = $("#" + Vinculo.Prefijo + "IdPaisNacionalidad").val();
    Entidad.Pariente.IdPaisOrigen = $("#" + Vinculo.Prefijo + "IdPaisOrigen").val();
    Entidad.Pariente.ProvinciaNacimiento = $("#" + Vinculo.Prefijo + "ProvinciaNacimiento").val();
    Entidad.Pariente.DepartamentoProvincialNacimiento = $("#" + Vinculo.Prefijo + "DepartamentoProvincialNacimiento").val();
    Entidad.Pariente.LocalidadNacimiento = $("#" + Vinculo.Prefijo + "LocalidadNacimiento").val();
    Entidad.VinculoId = $("#" + Vinculo.Prefijo + "VinculoId").val();
    Entidad.VinculoNombre = $("#" + Vinculo.Prefijo + "VinculoId option:selected").text();
    Entidad.Ocupacion = $("#" + Vinculo.Prefijo + "Ocupacion").val();
    Entidad.Telefono = $("#" + Vinculo.Prefijo + "Telefono").val();
    Entidad.Vive = $("#" + Vinculo.Prefijo + "Vive").is(":checked");
    Entidad.PermisoRetiro = $("#" + Vinculo.Prefijo + "PermisoRetiro").is(":checked");

    var temp = $("#listaVinculos").data("vinculoFamiliar") || [];
    var datos = temp;
    if (Vinculo.EstadoVinculo === "Editar")
        for (var i = 0; i < datos.length; i++) {
            if (datos[i].Id == Entidad.Id) {
                datos[i] = Entidad;
            }
        }
    else {
        datos[datos.length] = Entidad;
    }

    temp = datos;
    $("#listaVinculos").data("vinculoFamiliar", temp);
    $("#listaVinculos").clearGridData();
    for (var item = 0; item < datos.length; item++) {
        $("#listaVinculos").addRowData(-(item + 1), datos[item], "last");
    }

    if (Vinculo.EstadoVinculo === "Editar") {
        Vinculo.CancelarVinculo();
    }
    else {
        //Limpia el formulario de vinculo
        Vinculo.LimpiarCampos();
        $("#divCamposVinculo").hide();
        $("#Vinculos_btnLimpiarPF").click();
    }

    $("#Vinculos_NumeroDocumento").val("");

    if (Vinculo.EstadoVinculo == "Editar") {
        Vinculo.CancelarVinculo();
    }
}

Vinculo.CancelarVinculo = function (estado) {
    $("#Vinculos_btnLimpiarPF").click();
    Vinculo.MostrarBotones();
    Vinculo.LimpiarCampos();
    $("#divCamposVinculo").hide();
    $("#divPersonaFisicaVinculo").hide();
    $("#btnCleanVinculo").hide();
    $("#btnAddVinculo").hide();
}

//Elimina un vinculo de la grilla
Vinculo.QuitarVinculoClick = function () {
    var gr = jQuery("#listaVinculos").jqGrid('getGridParam', 'selrow');
    if (gr != null) {
        //Variable que contiene el registro seleccionado de la grilla de vinculos        
        var seleccion = $("#listaVinculos").getRowData(parseInt($("#listaVinculos").getGridParam("selrow")));
        var dniEstudiante;
        for (var item in seleccion) {
            if (item == "Pariente.NumeroDocumento") {
                dniEstudiante = seleccion[item];
            }
        }
        //Variable que contiene todos los datos temporales de la grilla
        var registro = $("#listaVinculos").data("vinculoFamiliar") || [];
        //Variable en la que se va a guardar todos los datos temporales menos el que se elimina
        var temp = [];
        //Comparo los datos temporales que estan en la grilla y elimino el que este seleccionado
        for (var i = 0; i < registro.length; i++) {
            if (registro[i].Pariente.NumeroDocumento != dniEstudiante) {
                temp[temp.length] = registro[i];
            }
        }
        //Borro visualmente de la grilla el seleccionado
        jQuery("#listaVinculos").delRowData(gr);
        //Cargo en el data de la grilla los nuevos datos temporales sin el que se elimino
        $("#listaVinculos").data("vinculoFamiliar", temp);
    }
    else {
        Mensaje.Error.texto = "Seleccione un vinculo para eliminar.";
        Mensaje.Error.mostrar();
    }
};

function CargarVinculoFamiliarByPersona(data) {
    var grilla = $("#listaVinculos");
    grilla.clearGridData();

    var temp = [];
    for (var i = 0; i < data.rows.length; i++) {
        var Entidad = {};
        Entidad.Pariente = {};
        Entidad.Id = data.rows[i].Id;
        Entidad.Pariente.Id = data.rows[i].Pariente.Id;
        Entidad.Pariente.Nombre = data.rows[i].Pariente.Nombre;
        Entidad.Pariente.Apellido = data.rows[i].Pariente.Apellido;
        Entidad.Pariente.TipoDocumento = data.rows[i].Pariente.TipoDocumento;
        Entidad.Pariente.NumeroDocumento = data.rows[i].Pariente.NumeroDocumento;
        Entidad.Pariente.FechaNacimiento = data.rows[i].Pariente.FechaNacimiento;
        Entidad.Pariente.EstadoCivil = data.rows[i].Pariente.EstadoCivil;
        Entidad.Pariente.Observaciones = data.rows[i].Pariente.Observaciones;
        Entidad.Pariente.OrganismoEmisorDocumento = data.rows[i].Pariente.OrganismoEmisorDocumento;
        Entidad.Pariente.IdSexo = data.rows[i].Pariente.IdSexo
        Entidad.Pariente.Sexo = data.rows[i].Pariente.Sexo;
        Entidad.Pariente.IdPaisEmisorDocumento = data.rows[i].Pariente.IdPaisEmisorDocumento;
        Entidad.Pariente.IdPaisNacionalidad = data.rows[i].Pariente.IdPaisNacionalidad;
        Entidad.Pariente.IdPaisOrigen = data.rows[i].Pariente.IdPaisOrigen;
        Entidad.Pariente.ProvinciaNacimiento = data.rows[i].Pariente.ProvinciaNacimiento;
        Entidad.Pariente.DepartamentoProvincialNacimiento = data.rows[i].Pariente.DepartamentoProvincialNacimiento;
        Entidad.Pariente.LocalidadNacimiento = data.rows[i].Pariente.LocalidadNacimiento;
        Entidad.VinculoId = data.rows[i].VinculoId;
        Entidad.VinculoNombre = data.rows[i].VinculoNombre;
        Entidad.Ocupacion = data.rows[i].Ocupacion;
        Entidad.Telefono = data.rows[i].Telefono;
        Entidad.Vive = data.rows[i].Vive;
        Entidad.PermisoRetiro = data.rows[i].PermisoRetiro;

        var id = temp.length;
        temp[id] = Entidad;
        grilla.addRowData(-id, Entidad, "last");
    }
    grilla.data("vinculoFamiliar", temp);
}
