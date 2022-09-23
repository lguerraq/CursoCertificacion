var Contador;
var Evaluacion = {
    SaveEvaluacion: function () {
        var evaluacion = [];
        var preguntas = [];

        $.each($("input[type=radio]"), function (i, v) {
            if (!preguntas.includes($(v).attr("name"))) {
                preguntas.push($(v).attr("name"));
            }
        });

        $.each(preguntas, function (i, v) {
            var respuestaSeleccionada = $("input[type=radio][name=" + v + "]:checked").val();
            evaluacion.push({ IdPregunta: parseInt(v), IdRespuesta: parseInt(respuestaSeleccionada) });
        });

        var IdEvento = $("#txthdIdEvento").val();

        DBR.blockUIStar("Guardando...");

        $.ajax({
            url: urlSaveEvaluacion,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ idCuestionario: $("#hdfIdCuestionario").val(), "request": evaluacion, "IdEvento": IdEvento }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    DBR.ToastSuccess(response.Message);
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    Contador = setInterval(function () {
                        clearInterval(Contador);
                        window.location.reload();
                    }, 1200);     
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    },
    ValidateEvaluacion: function () {

        var preguntas = [];
        var result = true;

        $.each($("input[type=radio]"), function (i, v) {
            if (!preguntas.includes($(v).attr("name"))) {
                preguntas.push($(v).attr("name"));
            }
        });

        $.each(preguntas, function (i, v) {
            var respuestaSeleccionada = $("input[type=radio][name=" + v + "]:checked").val();

            if (respuestaSeleccionada == undefined || respuestaSeleccionada == null) {
                result = false;
                return false;
            }
        });

        return result;
    },
    GenerarCertificado: function () {
        var documentos = new Object();
        documentos.IdEvento = $("#txthdIdEvento").val();

        DBR.blockUIStar("Generando..");
        $.ajax({
            url: urlCargarPdfCertificadoUtomatico,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "documentos": documentos }),
            success: function (response) {
                console.log(response);
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    window.open(urlDescargarPdfGenerado + "?NombreDocumento=" + response.Message, '_blank');
                    window.location.reload();
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    },
    DescargarCertificado: function () {
        var documentos = new Object();
        documentos.IdEvento = $("#txthdIdEvento").val();

        DBR.blockUIStar("Descargando..");
        $.ajax({
            url: urlCargarPdfCertificadoUtomatico,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "documentos": documentos }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    window.location.href = urlDescargarPdfGenerado + "?NombreDocumento=" + response.Message;
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    },
}

$(document).ready(function () {
    $("body").on("contextmenu", function (e) {
        return false;
    });

    //Disable part of page
    $("#id").on("contextmenu", function (e) {
        return false;
    });

    $(".login-btn").on("click", function (e) {
        e.preventDefault();
        window.location.href = urlLeccion + "/" + $(this).data("id") + "?rowId=" + $(this).data("curso");
    });

    $("#btnGuardarEvaluacion").click(function (e) {
        e.preventDefault();
        $("#txtError").html("");
        if (Evaluacion.ValidateEvaluacion()) {
            Evaluacion.SaveEvaluacion();
        } else {
            DBR.ToastError("Por favor marque todas las respuestas");
        }
    });
    $("#btnGenerarCertificado").click(function (e) {
        e.preventDefault();
        $("#modalConfirmacion").modal("show");
    });
    $("#btnGeneracionCertificado").on("click", function (e) {
        e.preventDefault();
        Evaluacion.GenerarCertificado();
    });
    $("#btnDescargarCertificado").on("click", function (e) {
        e.preventDefault();
        Evaluacion.DescargarCertificado();
    });
});

