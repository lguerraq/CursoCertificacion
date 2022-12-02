var contenidoHtml = "";
var editor;
var Eventos = {
    ListAllEventoCombo: function () {
        $.ajax({
            url: urlListAllEventoCombo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                DBR.LlenarCombo(response, "#cboEvento", true, false);
                //var ultimoValor = response[response.length - 1].Value;
                //$("#cboEvento").val(ultimoValor).trigger("change");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    GetContenidoVirtualByEvento: function () {

        var request = new Object();
        request.IdEvento = $("#cboEvento").val();

        DBR.blockUIStar();
        $.ajax({
            url: urlGetContenidoVirtualByEvento,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                $("#txtViertualContenido").val(response.IdVirtualContenido);
                response.Contenido = response.Contenido.replace(/div/g, "p");
                CKEDITOR.instances.txtMensajeCompleto.setData(response.Contenido);
                contenidoHtml = response.Contenido;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                $("#txtViertualContenido").val(0);
                CKEDITOR.instances.txtMensajeCompleto.setData("<p></p>");
                contenidoHtml = "<p></p>";
                console.log(textStatus);
            }
        });
    },
    SaveVirtualContenido: function () {

        var request = new Object();
        request.IdVirtualContenido = $("#txtViertualContenido").val();
        request.IdEvento = $("#cboEvento").val();
        request.Contenido = contenidoHtml;

        DBR.blockUIStar();
        $.ajax({
            url: urlSaveVirtualContenido,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $("#txtViertualContenido").val(response.Codigo);
                    editor.setReadOnly(true);
                    $("#btnGuardarContenido").hide();
                    $("#btnEditarContenido").show();
                    DBR.ToastSuccess(response.Message);
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    //VirtualEvento    
    ListVirtualVideoByEvento: function () {

        var request = new Object();
        request.IdEvento = $("#cboEvento").val();

        $.ajax({
            url: urlListVirtualVideoByEvento,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                Funciones.LlenarVirtualVideo(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    SaveVirtualVideo: function () {

        var request = new Object();
        request.IdVirtualVideo = $("#txtIdVirtualVideo").val();
        request.IdEvento = $("#cboEvento").val();
        request.Url = $("#txtUrl").val();

        DBR.blockUIStar();
        $.ajax({
            url: urlSaveVirtualVideo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $("#modalVirtualVideo").modal("hide");
                    Eventos.ListVirtualVideoByEvento();
                    DBR.ToastSuccess(response.Message);
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    GetVirtualVideo: function (IdVirtualVideo) {

        $("#txtIdVirtualVideo").val(IdVirtualVideo);
        DBR.limpiarCampo("#txtUrl")
        $("#lblTitleAgregarVirtualVideo").html("Editar");

        var request = new Object();
        request.IdVirtualVideo = $("#txtIdVirtualVideo").val();

        DBR.blockUIStar();
        $.ajax({
            url: urlGetVirtualVideo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                $("#txtUrl").val(response.Url);
                $("#modalVirtualVideo").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    DeleteVirtualVideo: function (IdVirtualVideo) {
        var request = new Object();
        request.IdVirtualVideo = IdVirtualVideo;

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
                            url: urlDeleteVirtualVideo,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    Eventos.ListVirtualVideoByEvento();
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
}

var Funciones = {
    LlenarVirtualVideo: function (data) {
        var row = "";
        var Ancho = $("#cabeceraGrilla").width();
        $.each(data, function (e, i) {

            var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetVirtualVideo(' + i.IdVirtualVideo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
            var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteVirtualVideo(' + i.IdVirtualVideo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';

            row = row + '<tr>' +
                            '<td><div class="UrlVideos">' + i.Url + '</div></td>' +
                            '<td class="text-center">' + editar + ' ' + eliminar + '</td>' +
                        '</tr>';
        });
        if (row == "") {
            row = row + '<tr><td colspan="2">No se encontraron registros</td></tr>';
        }
        $("#bodyVirtualVideo").html(row);
        $(".UrlVideos").width(Ancho);
        $('[data-toggle="tooltip1"]').tooltip();
    }
}
$(document).ready(function () {
    Eventos.ListAllEventoCombo();
    Eventos.GetContenidoVirtualByEvento();
    Eventos.ListVirtualVideoByEvento();

    var controlMensComp = CKEDITOR.replace('txtMensajeCompleto', { height: 500 });
    controlMensComp.on('change', function (evt) {
        contenidoHtml = evt.editor.getData();
    });
    controlMensComp.on('instanceReady', function (ev) {
        editor = ev.editor;
        editor.setReadOnly(true);
    });
    $("#btnEditarContenido").on("click", function () {
        editor.setReadOnly(false);
        $(this).hide();
        $("#btnGuardarContenido").show();
    });
    $("#btnGuardarContenido").on("click", function () {
        Eventos.SaveVirtualContenido();
    });
    $("#cboEvento").on("change", function () {
        editor.setReadOnly(true);
        $("#btnGuardarContenido").hide();
        $("#btnEditarContenido").show();
        Eventos.GetContenidoVirtualByEvento();
        Eventos.ListVirtualVideoByEvento();
    });

    //VirtualVideo
    $("#btnAgregarVideo").on("click", function () {
        $("#txtIdVirtualVideo").val(0)
        DBR.limpiarCampo("#txtUrl");
        $("#lblTitleAgregarVirtualVideo").html("Agregar");
        $("#modalVirtualVideo").modal("show");
    });
    $("#btnGuardarVirtualVideo").on("click", function () {
        if (DBR.validarCampo("#txtUrl")) {
            Eventos.SaveVirtualVideo();
        }
    });
});