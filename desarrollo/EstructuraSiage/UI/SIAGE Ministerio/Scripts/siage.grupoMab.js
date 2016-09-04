var GrupoMab = {};

 GrupoMab.initGrillaCodigosMovimientoMab= function () {
        $("#gridCodigosMovimientoMab").jqGrid({
            url: $.getUrl("/CodigoMovimientoMab/GetAllCodigosMovimientoMab"),
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Id', 'Codigo', 'Descripcion', 'Uso', 'GrupoMabId'],
            multiselect: true,
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 150, sortable: false, jsonmap: 'Codigo' },
                            { name: 'Descripcion', index: 'Descripcion', width: 150, sortable: false, jsonmap: 'Descripcion' },
                            { name: 'Uso', index: 'Uso', width: 150, sortable: false, jsonmap: 'Uso' },
                            { name: 'GrupoMabId', index: 'GrupoMabId', width: 150, sortable: false, jsonmap: 'GrupoMabId', hidden: true }
                          ],
            pager: '#pager1',
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            caption: 'Códigos de Movimiento Mab',
            height: "100%"
        });
        //fin definicion de la grilla1

    };
    GrupoMab.initGrillaEstadosAntrioresPuestoTrabajo = function () {


        $("#grillaEstadosAnterioresAccionesPuestoTrabajo").jqGrid({
            datatype: 'json',

            colNames: ['Id', 'Estado anterior puesto trabajo', 'IdEstadoAnteriorPT'],
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'EstadoAnteriorPTtext', index: 'EstadoAnteriorPTtext', width: 250, sortable: false, jsonmap: 'EstadoAnteriorPTtext' },
                            { name: 'IdEstadoAnteriorPT ', index: 'IdEstadoAnteriorPT', sortable: false, jsonmap: 'IdEstadoAnteriorPT', hidden: true }
                            ],
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            height: "100%"

        });


        $("#btnQuitarEstadoPT1").click(function () {
            var data = $("#grillaEstadosAnterioresAccionesPuestoTrabajo").jqGrid("getGridParam", "selrow");
            $("#grillaEstadosAnterioresAccionesPuestoTrabajo").delRowData(data);
        });

        $("#btnAgregarEstadoPT1").click(function () {

            if ($('#EstadoAnteriorPtId option:selected').text() == "SELECCIONE") {
                Mensaje.Error.texto = 'Debe seleccionar un estado anterior  puesto de trabajo';
                Mensaje.Error.mostrar();
                return;
            }

            var estadoAnterior = {
                Id: $("#grillaEstadosAnterioresAccionesPuestoTrabajo").getGridParam("reccount") + 1,
                IdEstadoAnteriorPT: $("#EstadoAnteriorPtId :selected").val(),
                EstadoAnteriorPTtext: $("#EstadoAnteriorPtId :selected").text()
            };


            //validar que el estado no este agregado
            if ($("#EstadoAnteriorPtId").val() != "") {
                var data = $("#grillaEstadosAnterioresAccionesPuestoTrabajo").getRowData();

                for (i = 0; i < data.length; i++) {
                    if (data[i].EstadoAnteriorPTtext === $("#EstadoAnteriorPtId :selected").text()) {
                        Mensaje.Error.texto = "El estado " + data[i].EstadoAnteriorPTtext + " ya se encuentra agregado";
                        Mensaje.Error.mostrar();
                        return;
                    }
                }
            };


            $("#grillaEstadosAnterioresAccionesPuestoTrabajo").addRowData(estadoAnterior.Id, estadoAnterior, 'last');
            $('#EstadoAnteriorPtId').val(-1);

        });


    };
    GrupoMab.initGrillaEstadosAntrioresPuestoTrabajoAnteriores = function () {


        $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").jqGrid({
            datatype: 'json',
            colNames: ['Id', 'Estado anterior puesto trabajo', 'IdEstadoAnteriorPT'],
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'EstadoAnteriorPTtext', index: 'EstadoAnteriorPTtext', width: 250, sortable: false, jsonmap: 'EstadoAnteriorPTtext' },
                            { name: 'IdEstadoAnteriorPT ', index: 'IdEstadoAnteriorPT', sortable: false, jsonmap: 'IdEstadoAnteriorPT', hidden: true }
                            ],
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            height: "100%"

        });


        $("#btnQuitarEstadoPT2").click(function () {
            var data = $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").jqGrid("getGridParam", "selrow");
            $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").delRowData(data);
        });

        $("#btnAgregarEstadoPT2").click(function () {

            if ($('#EstadoAnteriorPtAnteriorId option:selected').text() == "SELECCIONE") {
                Mensaje.Error.texto = 'Debe seleccionar un estado anterior  puesto de trabajo';
                Mensaje.Error.mostrar();
                return;
            }

            var estadoAnterior = {
                Id: $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").getGridParam("reccount") + 1,
                IdEstadoAnteriorPT: $("#EstadoAnteriorPtAnteriorId :selected").val(),
                EstadoAnteriorPTtext: $("#EstadoAnteriorPtAnteriorId :selected").text()
            };

            //validar que el estado no este agregado
            if ($("#EstadoAnteriorPtAnteriorId").val() != "") {
                var data = $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").getRowData();

                for (i = 0; i < data.length; i++) {
                    if (data[i].EstadoAnteriorPTtext === $("#EstadoAnteriorPtAnteriorId :selected").text()) {
                        Mensaje.Error.texto = "El estado " + data[i].EstadoAnteriorPTtext + " ya se encuentra agregado";
                        Mensaje.Error.mostrar();
                        return;
                    }
                }
            };


            $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").addRowData(estadoAnterior.Id, estadoAnterior, 'last');
            $('#EstadoAnteriorPtAnteriorId').val(-1);

        });
    
    }
     GrupoMab.initGrillaCodigoMovimientoParaGrupoMAB= function () {
        $("#grillaCodigoMovimientoParaGrupoMAB").jqGrid({
            datatype: 'local',
            multiselect: false,
            colNames: ['Id', 'Codigo', 'Descripcion', 'Uso', 'GrupoMabId'],
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 150, sortable: false, jsonmap: 'Codigo' },
                            { name: 'Descripcion', index: 'Descripcion', width: 150, sortable: false, jsonmap: 'Descripcion' },
                            { name: 'Uso', index: 'Uso', width: 150, sortable: false, jsonmap: 'Uso' },
                            { name: 'GrupoMabId', index: 'GrupoMabId', width: 150, sortable: false, jsonmap: 'GrupoMabId', hidden: true }
                          ],
            pager: '#pager2',
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            caption: 'Códigos de Movimiento Mab del Grupo MAB seleccionado',
            height: "100%"
        });
    };
     GrupoMab.initCodigosMovimientoMabSeleccionados= function () {
        $("#gridCodigosMeMovimientoSeleccionados").jqGrid({
            datatype: 'local',
            multiselect: true,
            
            mtype: 'GET',
            colNames: ['Id', 'Codigo', 'Descripcion', 'Uso', 'GrupoMabId'],
            colModel: [
                            { name: 'Id', index: 'Id', width: 55, key: true, jsonmap: 'Id', hidden: true },
                            { name: 'Codigo', index: 'Codigo', width: 150, sortable: false, jsonmap: 'Codigo' },
                            { name: 'Descripcion', index: 'Descripcion', width: 150, sortable: false, jsonmap: 'Descripcion' },
                            { name: 'Uso', index: 'Uso', width: 150, sortable: false, jsonmap: 'Uso' },
                            { name: 'GrupoMabId', index: 'GrupoMabId', width: 150, sortable: false, jsonmap: 'GrupoMabId', hidden: true }
                          ],
            pager: '#pager2',
            rowNum: 10,
            sortname: 'invid',
            sortorder: 'desc',
            viewrecords: true,
            width: '100%',
            caption: 'Códigos de Movimiento Mab seleccionados',
            height: "100%"
        });
    };
    GrupoMab.initBotonAgregarQuitar = function () {


        $("#btnQuitar").click(function () {
            var data = $("#gridCodigosMeMovimientoSeleccionados").jqGrid("getGridParam", "selarrrow");
            var aux = null;
            var longitud = data.length;
            for (var i = 0; i < longitud; i++) {
                aux = $("#gridCodigosMeMovimientoSeleccionados").getRowData(data[0]);
                //$("#gridCodigosMeMovimientoSeleccionados").delRowData(data[0]);
                $("#gridCodigosMovimientoMab").addRowData(aux.Id, aux);
                $("#gridCodigosMeMovimientoSeleccionados").delRowData(aux.Id);
            }
        }); //END BTN QUITAR

        $("#btnAgregar").click(function () {
            var data = $("#gridCodigosMovimientoMab").jqGrid("getGridParam", "selarrrow");
            var aux = null;
            var longitud = data.length;
            for (var i = 0; i < longitud; i++) {
                aux = $("#gridCodigosMovimientoMab").jqGrid("getRowData", data[0]);
                $("#gridCodigosMeMovimientoSeleccionados").addRowData(aux.Id, aux);
                $("#gridCodigosMovimientoMab").delRowData(aux.Id);
            }
        }); //END BTN AGREGAR
    };
    GrupoMab.inicializarFuncionalidadCombos = function (estado) {

        $("#EstadoAsignacionId").setEnabled(false);
        $("#EstadoPosteriorPtId").setEnabled(false);
        $("#EstadoAnteriorPtId").setEnabled(false);
        $("#EstadoAsignacionAnteriorId").setEnabled(false);
        $("#EstadoPosteriorPtAnteriorId").setEnabled(false);
        $("#EstadoAnteriorPtAnteriorId").setEnabled(false);

        $("#btnAgregarEstadoPT1").setEnabled(false);
        $("#btnQuitarEstadoPT1").setEnabled(false)
        $("#btnAgregarEstadoPT2").setEnabled(false);
        $("#btnQuitarEstadoPT2").setEnabled(false)

        $("#ModificaEstadoAsignacionPuesto").click(function () {
            if ($("#ModificaEstadoAsignacionPuesto").is(":checked")) {
                $("#EstadoAsignacionId").setEnabled(true);
            } else { $("#EstadoAsignacionId").setEnabled(false); }
        });

        $("#ModificaEstadoPosteriorPuesto").click(function () {
            if ($("#ModificaEstadoPosteriorPuesto").is(":checked")) {
                $("#EstadoPosteriorPtId").setEnabled(true);
            } else { $("#EstadoPosteriorPtId").setEnabled(false); }

        });

        $("#ModificaEstadoAnteriorPuesto").click(function () {
            if ($("#ModificaEstadoAnteriorPuesto").is(":checked")) {
                $("#EstadoAnteriorPtId").setEnabled(true);
                $("#btnAgregarEstadoPT1").setEnabled(true);
                $("#btnQuitarEstadoPT1").setEnabled(true)
            } else {
                $("#EstadoAnteriorPtId").setEnabled(false);
                $("#btnAgregarEstadoPT1").setEnabled(false);
                $("#btnQuitarEstadoPT1").setEnabled(false)

            }

        });
        $("#ModificaEstadoAsignacionPuestoAnterior").click(function () {
            if ($("#ModificaEstadoAsignacionPuestoAnterior").is(":checked")) {
                $("#EstadoAsignacionAnteriorId").setEnabled(true);
            } else { $("#EstadoAsignacionAnteriorId").setEnabled(false); }
        });

        $("#ModificaEstadoAnteriorPuestoAnterior").click(function () {
            if ($("#ModificaEstadoAnteriorPuestoAnterior").is(":checked")) {
                $("#EstadoAnteriorPtAnteriorId").setEnabled(true);

                $("#btnAgregarEstadoPT2").setEnabled(true);
                $("#btnQuitarEstadoPT2").setEnabled(true)
            } else {
                $("#EstadoAnteriorPtAnteriorId").setEnabled(false);
                $("#btnAgregarEstadoPT2").setEnabled(false);
                $("#btnQuitarEstadoPT2").setEnabled(false);

            }
        });

        $("#ModificaEstadoPosteriorPuestoAnterior").click(function () {
            if ($("#ModificaEstadoPosteriorPuestoAnterior").is(":checked")) {
                $("#EstadoPosteriorPtAnteriorId").setEnabled(true);
            } else {
                $("#EstadoPosteriorPtAnteriorId").setEnabled(false);
                //---
              

            }
        });
    };


    //carga los datos para ver / editar / eliminar
    GrupoMab.cargarDatosGrillas = function () {
        $.get($.getUrl('/GrupoMab/GetEstadoPuestosPorEjecucionMabId/'), { 'ejecucionMabId': $('#EjecucionEnPuestoTrabajoId').val() }, function (data) {
            $("#grillaEstadosAnterioresAccionesPuestoTrabajo").addRowData(data.Id, data);
            $('#grillaEstadosAnterioresAccionesPuestoTrabajo').trigger('reloadGrid');
        });

        $.get($.getUrl('/GrupoMab/GetEstadoPuestosPorEjecucionMabId/'), { 'ejecucionMabId': $('#EjecucionEnPuestoTrabajoAnteriorId').val() }, function (data) {
            $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").addRowData(data.Id, data);
            $('#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior').trigger('reloadGrid');
        });

        GrillaUtil.setUrl($('#gridCodigosMeMovimientoSeleccionados'), $.getUrl('/GrupoMab/GetCodigosMovimientoByGrupoMabId/' + '?grupoMabId=' + parseInt($('#Id').val())));
        $('#gridCodigosMeMovimientoSeleccionados').setGridParam({ datatype: "json", mtype: 'GET', multiselect: true });
        $('#gridCodigosMeMovimientoSeleccionados').trigger('reloadGrid');


    }

    GrupoMab.registrar = function () {
        GrupoMab.initGrillaCodigosMovimientoMab();
        GrupoMab.initBotonAgregarQuitar();
        $("#btnAceptar").click(function () { GrupoMab.inicializarFuncionalidadCombos(); });
    }
    GrupoMab.eliminar = function () {
        $('#divAbmcPostEnviarModelo #TipoGrupo').attr('disabled', true);
        $('#btnAgregar, #btnQuitar').attr('disabled', true);
        GrupoMab.cargarDatosGrillas();

    }
    GrupoMab.ver = function () {
        $('.soloRegistro').hide();
        $('#btnAgregar, #btnQuitar').hide();
        $('#divAbmc #TipoGrupo').attr('disabled', true);
        $('#btnAgregar, #btnQuitar').attr('disabled', true);
        $("#btnAgregarQuitar").hide();

        GrupoMab.cargarDatosGrillas();
    }
    GrupoMab.editar = function () {
        GrupoMab.initGrillaCodigosMovimientoMab();
        GrupoMab.initBotonAgregarQuitar();
        GrupoMab.cargarDatosGrillas();

        $('#divAbmc #TipoGrupo').attr('disabled', true);

        //se habilita o desabilita los combos dependiendo que viene del model
        if ($("#ModificaEstadoAsignacionPuestoAnterior").is(":checked")) {
            $("#EstadoAsignacionAnteriorId").setEnabled(true);
        }

        if ($("#ModificaEstadoAnteriorPuestoAnterior").is(":checked")) {
            $("#EstadoAnteriorPtAnteriorId").setEnabled(true);
        }
        if ($("#ModificaEstadoPosteriorPuestoAnterior").is(":checked")) {
            $("#EstadoPosteriorPtAnteriorId").setEnabled(true);
        }
        //---
        if ($("#ModificaEstadoAsignacionPuesto").is(":checked")) {
            $("#EstadoAsignacionId").setEnabled(true);
        }

        if ($("#ModificaEstadoPosteriorPuesto").is(":checked")) {
            $("#EstadoPosteriorPtId").setEnabled(true);
        }

        if ($("#ModificaEstadoAnteriorPuesto").is(":checked")) {
            $("#EstadoAnteriorPtId").setEnabled(true);

        }



    }

    GrupoMab.init = function (estadoText) {

        GrupoMab.estado = estadoText;
        GrupoMab.initCodigosMovimientoMabSeleccionados();
        //inicializo cmo no editable los combos, para que luego cuando le damos al check, me de la opcion de habilitar
        GrupoMab.inicializarFuncionalidadCombos(estadoText);
        GrupoMab.initGrillaEstadosAntrioresPuestoTrabajo();
        GrupoMab.initGrillaEstadosAntrioresPuestoTrabajoAnteriores();

        switch (estadoText) {
            case 'Eliminar': GrupoMab.eliminar(); break;
            case 'Registrar':  GrupoMab.registrar(); break;
            case 'Editar':   GrupoMab.editar();  break;
            case 'Ver':   GrupoMab.ver();    break;
            default:
                break;
        };
    };

    Abmc.preEnviarModelo = function (datos) {
        var unidades = $("#gridCodigosMeMovimientoSeleccionados").getRowData();
        var codigosDeMovimiento = [];
        $.each(unidades, function (ind, val) {
            codigosDeMovimiento[ind] = {};
            codigosDeMovimiento[ind].Id = val.Id
            codigosDeMovimiento[ind].Codigo = val.Codigo;
            codigosDeMovimiento[ind].Descripcion = val.Descripcion;
            codigosDeMovimiento[ind].Uso = val.Uso;
            codigosDeMovimiento[ind].GrupoMabId = val.GrupoMabId;
        });
        //estados previo puesto trabajo 
        var estados = $("#grillaEstadosAnterioresAccionesPuestoTrabajo").getRowData();
        var estadoPuesto = [];
        $.each(estados, function (ind, val) {
            estadoPuesto[ind] = {};
            estadoPuesto[ind].Valor = val.EstadoAnteriorPTtext;
            estadoPuesto[ind].Descripcion = val.EstadoAnteriorPTtext;
            estadoPuesto[ind].Id = val.IdEstadoAnteriorPT;
          
        });

        var estadosAnterior = $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").getRowData();
        var estadoPuestoAnterior = [];
        $.each(estados, function (ind, val) {
            estadoPuestoAnterior[ind] = {};
            estadoPuestoAnterior[ind].Valor = val.EstadoAnteriorPTtext;
            estadoPuestoAnterior[ind].Descripcion = val.EstadoAnteriorPTtext;
            estadoPuestoAnterior[ind].Id = val.IdEstadoAnteriorPT;
        });

        $.formatoModelBinder(estadoPuestoAnterior, datos, "EstadosPuestoAnterioresPtAnterior");
        $.formatoModelBinder(estadoPuesto, datos, "EstadosPuestoAnterioresPt");
        $.formatoModelBinder(codigosDeMovimiento, datos, "CodigosMovimientoMab");
    };

    Abmc.postEnviarModelo = function (data) {
                if (data && data.status && GrupoMab.estado !== 'Editar') {
                    $("#gridCodigosMeMovimientoSeleccionados").clearGridData(false);
                    $("#gridCodigosMovimientoMab").setGridParam({ page: 1 });
                    $("#gridCodigosMovimientoMab").trigger('reloadGrid');
                }

        if (GrupoMab.estado == "Registrar" && data.model) {
            $("#listRecientes").delRowData(0);
            $("#listRecientes").addRowData("Id", data.model, "last");
            if (data.status) {
                $("#gridCodigosMeMovimientoSeleccionados").clearGridData();
                $("#grillaEstadosAnterioresAccionesPuestoTrabajo").clearGridData();
                $("#grillaEstadosAnterioresAccionesPuestoTrabajoAnterior").clearGridData();
            }
        }
    };

GrupoMab.estado = null;
