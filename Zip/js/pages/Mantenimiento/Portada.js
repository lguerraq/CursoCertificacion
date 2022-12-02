var NombreImagen;

var image = document.getElementById('imgPreview');
var cropBoxData;
var canvasData;
var cropper;

var Eventos = {
    SavePortada: function () {
        var input = document.getElementById("fileFoto");

        var formData = new FormData();
        if (HabilitarValidacionDoc) {
            formData.append("documento", input.files[0]);
            formData.append("NombreImagen", input.files[0].name);
        } else {
            formData.append("NombreImagen", NombreImagen);
        }

        cropBoxData = cropper.getCropBoxData();
        canvasData = cropper.getCanvasData();

        formData.append("IdPortada", $("#txtIdPortada").val());
        formData.append("Descripcion", $("#txtDescripcion").val());
        formData.append("SubTitulo1", $("#txtSubTitulo1").val());
        formData.append("SubTitulo2", $("#txtSubTitulo2").val());
        formData.append("TextoEnlace", $("#txtTextoEnlace").val());
        formData.append("UrlEnlace", $("#txtUrlEnlace").val());
        //Datos canvax
        formData.append("left", cropBoxData.left);
        formData.append("top", cropBoxData.top);
        formData.append("width", cropBoxData.width);
        formData.append("height", cropBoxData.height);
        formData.append("widthCropper", canvasData.width);
        formData.append("heightCropper", canvasData.height);
        formData.append("naturalWidth", canvasData.naturalWidth);
        formData.append("naturalHeight", canvasData.naturalHeight);

        DBR.blockUIStar();

        $.ajax({
            url: urlSavePortada,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                $('#gridPortada').DataTable().ajax.reload();
                $("#modalPortada").modal("hide");
                DBR.ToastSuccess(response.Message);
            } else {
                DBR.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
    GetPortada: function (IdPortada) {
        var request = new Object();
        request.IdPortada = IdPortada;

        DBR.blockUIStar();

        $.ajax({
            url: urlGetPortada,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                Funciones.LimpiarCaposPortada();
                $("#txtIdPortada").val(response.IdPortada);
                $($("#fileFoto").next().children()[0]).val(response.NombreImagen);
                $("#txtDescripcion").val(response.Descripcion);
                $("#txtSubTitulo1").val(response.SubTitulo1);
                $("#txtSubTitulo2").val(response.SubTitulo2);
                if (response.TextoEnlace == null) {
                    $('#ckIncluirEnlance').iCheck('uncheck');
                } else {
                    $('#ckIncluirEnlance').iCheck('check');
                }
                $("#txtTextoEnlace").val(response.TextoEnlace);
                $("#txtUrlEnlace").val(response.UrlEnlace);
                
                $('#imgPreview').attr("src", urlCarpetaGaleria + response.NombreImagen);
                NombreImagen = response.NombreImagen;
                HabilitarValidacionDoc = false;
                $('#imgPreview').show();
                $("#modalPortada").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $(".gifLoadingGeneral").hide();
                console.log(XMLHttpRequest);
            }
        });
    },
    GetFoto: function (Nombre) {
        $("#viewFoto").attr("src", urlCarpetaGaleria + Nombre);
        $("#modalVerImagen").modal("show");
    },
    DeletePortada: function (IdPortada) {
        var request = new Object();
        request.IdPortada = IdPortada;

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
                            url: urlDeletePortada,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridPortada').DataTable().ajax.reload();
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
    LimpiarCaposPortada: function () {
        $('#imgPreview').hide();
        $('#imgPreview').attr("src", "");
        $('#fileFoto').filestyle('clear');
        $("#fileFoto").parent().removeClass("has-error");
        $("#fileFoto").next().next().addClass("hide");
        DBR.limpiarCampo("#txtDescripcion");
        $('#ckIncluirEnlance').iCheck('uncheck');
    },
}
var Grilla = {
    GrillaPortada: function () {
        $('#gridPortada').DataTable({
            "searching": true,
            "ordering": false,
            "processing": true,
            "serverSide": true,
            "destroy": true,
            "responsive": true,
            "language": setIdiomaSpanish,
            "ajax": {
                "url": urlListPortadaPaginado,
                "type": 'POST',
                "contentType": "application/json; charset=utf-8",
                "datatype": "JSON",
                "data": function (sSource) {
                    sSource.order = "ASC";
                    return JSON.stringify({ "page": sSource });
                },
            },
            "columns": [
                        { "data": "NombreImagen" },
                        { "data": "Descripcion" },
                        {
                            "data": function (row, type, set, meta) {
                                var foto = '<a data-toggle="tooltip1" data-placement="bottom" title="Ver imagen" onclick="Eventos.GetFoto(\'' + row.NombreImagen + '\')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-picture" aria-hidden="true"></span></a>';
                                return foto;
                            }, "sClass": 'text-center'
                        },
                        {
                            "data": function (row, type, set, meta) {
                                var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetPortada(' + row.IdPortada + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                                var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeletePortada(' + row.IdPortada + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';

                                return editar + " " + eliminar;
                            }, "sClass": 'text-center'
                        }
            ],
            "drawCallback": function (settings) {
                $('[data-toggle="tooltip1"]').tooltip({ container: 'body' });
            }
        });
    },
}
$(document).ready(function () {
    Grilla.GrillaPortada();
    $('.minimal').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
    });
    $("#btnAgregarPortada").on("click", function () {
        Funciones.LimpiarCaposPortada();
        $("#txtIdPortada").val(0);
        HabilitarValidacionDoc = true;
        $("#modalPortada").modal("show");
    });
    $("#btnGuardarPortada").on("click", function () {
        var input = document.getElementById("fileFoto");
        var validar = true;
        if (!DBR.validarCampo("#txtDescripcion")) {
            validar = false;
        }

        if (HabilitarValidacionDoc) {
            if (input.files.length == 0) {
                $("#fileFoto").parent().addClass("has-error");
                $("#fileFoto").next().next().removeClass("hide");
                validar = false;
            } else {
                $("#fileFoto").parent().removeClass("has-error");
                $("#fileFoto").next().next().addClass("hide");

                var extSoportado = "JPG,PNG,JPEG";
                var NameFile = input.files[0].name;
                var extension = NameFile.substr((NameFile.lastIndexOf('.') + 1));
                extension = extension.toUpperCase();

                if (extSoportado.indexOf(extension) < 0) {
                    $("#fileFoto").parent().addClass("has-error");
                    DBR.ToastError("Hay archivos no permitidos");
                    validar = false;
                } else {
                    $("#fileFoto").parent().removeClass("has-error");
                }
            }
        }
        if ($("#ckIncluirEnlance").prop("checked")) {
            if (!DBR.validarCampo("#txtTextoEnlace")) {
                validar = false;
            }
            if (!DBR.validarCampo("#txtUrlEnlace")) {
                validar = false;
            }
        }
        if (validar) {
            Eventos.SavePortada();
        }
    });
    $("#ckIncluirEnlance").on("ifChanged", function () {
        if ($(this).prop("checked")) {
            $("#containerTexto").removeClass("hide");
            $("#containerUrl").removeClass("hide");
        } else {
            $("#containerTexto").addClass("hide");
            $("#containerUrl").addClass("hide");
        }
        DBR.limpiarCampo("#txtTextoEnlace");
        DBR.limpiarCampo("#txtUrlEnlace");
    });
    $('#fileFoto').change(function (e) {
        HabilitarValidacionDoc = true;
        if (e.target.files.length == 0) {
            $('#fileFoto').filestyle('clear');
            $('#imgPreview').hide();
            $('#imgPreview').attr("src", "");
            return false;
        }
        var NameFile = e.target.files[0].name;
        var extension = NameFile.substr((NameFile.lastIndexOf('.') + 1));
        extension = extension.toUpperCase();

        var extensionesSoportadas = "JPG,JPEG,PNG";
        if (extensionesSoportadas.indexOf(extension) < 0) {
            $('#fileFoto').filestyle('clear');
            $('#imgPreview').hide();
            $('#imgPreview').attr("src", "");
            DBR.ToastError("Archivo no permitido.");
            return false;
        }
        if (e.target.files[0].size > 524288) {
            $('#fileFoto').filestyle('clear');
            $('#imgPreview').hide();
            $('#imgPreview').attr("src", "");
            DBR.ToastError("El archivo supera el limite de 0.5Mb permitido");
            return false;
        }
        addImage(e);
    });
    function addImage(e) {
        var file = e.target.files[0],
        imageType = /image.*/;

        if (!file.type.match(imageType)) {
            $('#imgPreview').hide();
            return;
        }
        var reader = new FileReader();
        reader.onload = fileOnload;
        reader.readAsDataURL(file);
    }
    function fileOnload(e) {
        var result = e.target.result;
        $('#imgPreview').attr("src", result);
        $('#imgPreview').fadeIn();

        cropper.destroy();
        cropper = null;

        image = document.getElementById('imgPreview');

        cropper = new Cropper(image, {
            autoCropArea: 1,
            viewMode: 2,
            zoomable: false,
            aspectRatio: widthCropper / heightCropper,
            ready: function () {
                //Should set crop box data first here
                cropper.setCropBoxData(cropBoxData).setCanvasData(canvasData);
            }
        });
    }
});


window.addEventListener('DOMContentLoaded', function () {

    $('#modalPortada').on('shown.bs.modal', function () {
        cropper = new Cropper(image, {
            autoCropArea: 1,
            viewMode: 2,
            zoomable: false,
            aspectRatio: widthCropper / heightCropper,
            ready: function () {
                //Should set crop box data first here
                cropper.setCropBoxData(cropBoxData).setCanvasData(canvasData);
            }
        });
    }).on('hidden.bs.modal', function () {
        cropBoxData = cropper.getCropBoxData();
        canvasData = cropper.getCanvasData();
        cropper.destroy();
        cropper = null;
    });
});