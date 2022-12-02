var config = {
    ini: function () {
        $.ajaxSetup({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {}
        });
    },
    iniGrids: {
        GrillaGaleria: function () {
            $('#gridGaleria').DataTable({
                "searching": false,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListGaleria,
                    "type": 'POST',
                    "datatype": "json",
                    "data": {},
                },
                "columns": [
                            { "data": "Nombre" },
                            {
                                "data": function (row, type, set, meta) {
                                    if (row.Activo) {
                                        return "SI";
                                    } else {
                                        return "NO";
                                    }
                                }, "sClass": 'text-center'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return '<a data-toggle="tooltip1" data-placement="bottom" title="Ver foto" onclick="Eventos.GetFoto(\'' + row.Nombre + '\')" class=""><i class="fa fa-image" aria-hidden="true"></i></a>';
                                }, "sClass": 'text-center'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                   
                                    var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar foto" onclick="Eventos.DeleteEvento(' + row.IdGaleria + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                                    var activar = '<a data-toggle="tooltip1" data-placement="bottom" title="Activar" onclick="Eventos.UpdateEstadoGaleria(' + row.IdGaleria + ',' + 1 + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></a>';
                                    var desactivar = '<a data-toggle="tooltip1" data-placement="bottom" title="Desactivar" onclick="Eventos.UpdateEstadoGaleria(' + row.IdGaleria + ',' + 0 + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></a>';
                                    if (row.Activo) {
                                        activar = '';
                                    } else {
                                        desactivar = '';
                                    }
                                    return eliminar + " " + activar + " " + desactivar;
                                }, "sClass": 'text-center'
                            }
                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        },
    }
}
var Eventos = {
    SaveGaleria: function () {
        var input1 = document.getElementById("fileFoto");

        var formData = new FormData();
        formData.append("documento", input1.files[0]);
        formData.append("Descripcion", $("#txtDescripcion").val());

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveGaleria,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                $('#gridGaleria').DataTable().ajax.reload();
                $("#modalGaleria").modal("hide");
                DBR.ToastSuccess(response.Message);
            } else {
                DBR.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
    GetFoto: function (Nombre) {
        $("#viewFoto").attr("src", urlCarpetaGaleria + Nombre);
        $("#modalVerImagen").modal("show");
    },
    DeleteEvento: function (IdGaleria) {
        var request = new Object();
        request.IdGaleria = IdGaleria;

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
                            url: urlDeleteGaleria,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridGaleria').DataTable().ajax.reload();
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
    UpdateEstadoGaleria: function (IdGaleria, Activo) {
        var request = new Object();
        request.IdGaleria = IdGaleria;
        request.Activo = Activo;

        DBR.blockUIStar();
        $.ajax({
            url: urlUpdateActivoGaleria,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridGaleria').DataTable().ajax.reload();
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
}
$(document).ready(function () {
    config.iniGrids.GrillaGaleria();
    $("#btnAgregarFoto").on("click", function () {
        $("#fileFoto").filestyle('clear');
        $("#fileFoto").parent().removeClass("has-error");
        $("#fileFoto").next().next().addClass("hide");

        $("#modalGaleria").modal("show");
    });
    $("#btnGuardarFoto").on("click", function () {
        var validar = true;
        var input = document.getElementById("fileFoto");
        if (input.files.length == 0) {
            $("#fileFoto").parent().addClass("has-error");
            $("#fileFoto").next().next().removeClass("hide");
            validar = false;
        } else {
            $("#fileFoto").parent().removeClass("has-error");
            $("#fileFoto").next().next().addClass("hide");
            if (input.files[0].size > 1048576) {
                DBR.ToastWarning("Verifique que el archivo adjunto no supere el 1MB");
                validar = false;
            }
        }
        if (!DBR.validarCampo("#txtDescripcion")) {
            validar = false;
        }
        if (validar) {
            Eventos.SaveGaleria();
        }
        
    });
});