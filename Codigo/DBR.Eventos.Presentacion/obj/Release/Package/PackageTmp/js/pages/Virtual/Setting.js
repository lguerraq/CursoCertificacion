var Eventos = {
    CambiarPassword: function () {
        var request = new Object();
        request.OldPassword = $("#txtOldPassword").val();
        request.NewPassword = $("#txtNewPassword").val();
        request.RepNewPassword = $("#txtRepNewPassword").val();

        DBR.blockUIStar();

        $.ajax({
            url: urlCambiarPassword,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ 'usuario': request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    Funciones.LimpiarCampos();
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
    },
}
var Funciones = {
    LimpiarCampos: function () {
        DBR.limpiarCampo("#txtOldPassword")
        DBR.limpiarCampo("#txtNewPassword")
        DBR.limpiarCampo("#txtRepNewPassword")
    }
}

$(document).ready(function () {
    $('#btnActualizar').on("click", function (e) {
        e.preventDefault();
        var validar = true;
        if (!DBR.validarCampo("#txtOldPassword")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtNewPassword")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtRepNewPassword")) {
            validar = false;
        }
        if (validar) {
            Eventos.CambiarPassword();
        }
    });
    $("#txtRepNewPassword").on('keyup', function () {
        var NewPassword = $("#txtNewPassword").val();
        var RepNewPassword = $("#txtRepNewPassword").val();
        if (RepNewPassword.length >= NewPassword.length) {
            if (NewPassword != RepNewPassword) {
                $("#txtErroForm").html("Las nuevas contraseñas no coinciden.");
                $("#txtErroForm").removeClass("hide");
            } else {
                $("#txtErroForm").html("");
                $("#txtErroForm").addClass("hide");
            }
        }
    }).keyup();
});