var EditarDocumento1 = false;

var config = {
    iniGrids: {
        GrillaSuceso: function () {
            $('#gridSucesos').DataTable({
                "searching": true,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "stateSave": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListSucesoPaginado,
                    "type": 'POST',
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "datatype": "json",
                    "data": function (oPaged) {
                        var request = new Object();
                        return JSON.stringify({ "page1": oPaged });
                    },
                },
                "columns": [
                            {
                                "data": function (row, type, set, meta) {
                                    return '<div class="ClBodyNombreSuceso" title="' + row.NombreSuceso + '" style="max-width:400px; overflow:hidden;white-space:nowrap;text-overflow: ellipsis;">' + row.NombreSuceso + '</div>';
                                }, "bSortable": false, "sClass": 'text-left', "width": "50%"
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return parseJsonDate(row.Fecha);
                                }, "bSortable": false, "sClass": 'text-center', "width": "15%"
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    if (row.Activo) {
                                        return "SI";
                                    } else {
                                        return "NO";
                                    }
                                }, "sClass": 'text-center', "width": "10%"
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return '<a href="' + urlDescargaDocumentos + row.ImagenSuceso + '" target="_blank">FOTO' + (meta.row + 1) + '.JPG</a>';
                                }, "sClass": 'text-center', "width": "10%"
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetSuceso('
                                        + row.IdSuceso + ','
                                        + (meta.row + 1)
                                        + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                                    var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteSuceso(' + row.IdSuceso + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                                    var activar = '<a data-toggle="tooltip1" data-placement="bottom" title="Activar" onclick="Eventos.UpdateEstadoSuceso(' + row.IdSuceso + ',' + 1 + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></a>';
                                    var desactivar = '<a data-toggle="tooltip1" data-placement="bottom" title="Desactivar" onclick="Eventos.UpdateEstadoSuceso(' + row.IdSuceso + ',' + 0 + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></a>';
                                    
                                    if (row.Activo) {
                                        activar = '';
                                    } else {
                                        desactivar = '';
                                    }
                                    return editar + " " + eliminar + " " + activar + " " + desactivar;
                                }, "sClass": 'text-center', "width": "15%"
                            }
                ],
                "drawCallback": function (settings) {
                    $(".ClBodyNombreSuceso").css("max-width", $(".ClNombreSuceso").width());
                    $('[data-toggle="tooltip1"]').tooltip({ container: "body" });
                }
            });
        }
    }
}
var Eventos = {
    SaveSuceso: function () {
        var input1 = document.getElementById("fileFoto");

        var formData = new FormData();
        formData.append("documento1", input1.files[0]);

        formData.append("IdSuceso", $("#txtIdSuceso").val());
        formData.append("NombreSuceso", $("#txtNombreSuceso").val());
        formData.append("Descripcion", $("#txtDescripcion").val());
        formData.append("Fecha", $("#txtFecha").val());
        formData.append("Horas", $("#txtHoras").val());       

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveSuceso,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                $('#gridSucesos').DataTable().ajax.reload();
                $("#modalSuceso").modal("hide");
                DBR.ToastSuccess(response.Message);
            } else {
                DBR.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
    GetSuceso: function (IdSuceso, row) {
        $("#lblTitleAgregarSuceso").html("Editar");
        Funciones.LimpiarCamposSuceso();

        var request = new Object();
        request.IdSuceso = IdSuceso;

        DBR.blockUIStar();
        $.ajax({
            url: urlGetSuceso,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();

                $("#txtIdSuceso").val(response.IdSuceso);
                $("#ddlTipo").val(response.Tipo);
                $("#txtNombreSuceso").val(response.NombreSuceso);
                $("#txtDescripcion").val(response.Descripcion);
                $("#txtFecha").val(parseJsonDate(response.Fecha));
                $("#txtHoras").val(response.Horas);
                $($("#fileFoto").next().children()[0]).val("FOTO" + row + ".PDF");
                EditarDocumento1 = false;
                $("#cboTema").val(response.IdsTemas).trigger("change");
                $("#modalSuceso").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeleteSuceso: function (IdSuceso) {
        var request = new Object();
        request.IdSuceso = IdSuceso;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de eliminar el registro?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urlDeleteSuceso,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridSucesos').DataTable().ajax.reload();
                                    DBR.ToastSuccess(response.Message);
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                DBR.blockUIStop();
                                console.log(textStatus);
                            }
                        });
                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    UpdateEstadoSuceso: function (IdSuceso, Activo) {
        var request = new Object();
        request.IdSuceso = IdSuceso;
        request.Activo = Activo;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de cambiar el estado del registro?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urUpdateEstadoSuceso,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridSucesos').DataTable().ajax.reload();
                                    DBR.ToastSuccess(response.Message);
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                DBR.blockUIStop();
                                console.log(textStatus);
                            }
                        });
                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    }
}
var Funciones = {
    LimpiarCamposSuceso: function () {
        $("#fileFoto").filestyle('clear');
        $("#fileFoto").parent().removeClass("has-error");
        $("#fileFoto").next().next().addClass("hide");

        DBR.limpiarCampo("#txtDescripcion");
        DBR.limpiarCampo("#txtNombreSuceso");
        DBR.limpiarCampo("#txtFecha");
        DBR.limpiarCampo("#txtHoras");
    },
}
$(document).ready(function () {

    config.iniGrids.GrillaSuceso();
    $('#txtFecha').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy"
    });
    $("#txtHoras").numericInput({ allowFloat: false, allowNegative: false });

    var _URL = window.URL || window.webkitURL;
    $("#fileFoto").on("change", function () {
        EditarDocumento1 = true;
        var image, file;

        if ((file = this.files[0])) {

            var sizeByte = this.files[0].size;
            var sizekiloBytes = parseInt(sizeByte / 1024);

            image = new Image();

            image.onload = function () {
                if (sizekiloBytes > 1024) {
                    DBR.ToastWarning("Verifique que la foto adjunta no supere el 1MB");
                    $("#fileFoto").filestyle('clear');
                }
                if (this.width !== 780 || this.height != 450) {
                    DBR.ToastWarning("Verifique que la foto compla con las dimenciones ancho: 780px y alto:450px");
                    $("#fileFoto").filestyle('clear');
                }
            };
            image.src = _URL.createObjectURL(file);
        }
    });
    $("#btnAgregarSuceso").on("click", function () {
        EditarDocumento1 = true;
        $("#lblTitleAgregarSuceso").html("Agregar");
        Funciones.LimpiarCamposSuceso();
        $("#modalSuceso").modal("show");
    });
    $("#btnGuardarSuceso").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtNombreSuceso")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtDescripcion")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtFecha")) {
            validar = false;
        }       
        if (!DBR.validarCampo("#txtHoras")) {
            validar = false;
        }
        if (EditarDocumento1) {
            var input = document.getElementById("fileFoto");
            if (input.files.length == 0) {
                $("#fileFoto").parent().addClass("has-error");
                $("#fileFoto").next().next().removeClass("hide");
                validar = false;
            } else {
                $("#fileFoto").parent().removeClass("has-error");
                $("#fileFoto").next().next().addClass("hide");
                if (input.files[0].size > 3145728) {
                    DBR.ToastWarning("Verifique que el archivo adjunto no supere el 1MB");
                    validar = false;
                }
            }
        }
        if (validar) {
            Eventos.SaveSuceso();
        }
    });
});