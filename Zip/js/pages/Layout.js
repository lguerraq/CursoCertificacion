function initActiveMenu() {
    // === following js will activate the menu in left side bar based on url ====
    $("#sidebar-menu a").each(function () {
        if (this.href == window.location.href) {
            $(this).parent().addClass("active");
            $(this).parent().parent().parent().addClass("active");
        }
    });
}

var EventosLayaut = {
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
                    $("#ModalCambiarContraseña").modal("hide");
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
    ValidarUsuarioActividad: function () {

        $.ajax({
            url: urlValidarUsuarioActividad,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {},
            success: function (response) {
                if (!response.IsSuccess) {
                    window.location.href = urlCerrarSesionAutomatica;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest);
            }
        });
    }
}
$(document).ready(function () {
    initActiveMenu();

    EventosLayaut.ValidarUsuarioActividad();
    setInterval(function () {
        EventosLayaut.ValidarUsuarioActividad();
    }, 60000);

    $("#btnCambiarContraseña").on("click", function () {

        $("#formCambioContrasenia .validar").each(function () {
            !DBR.limpiarCampo(this);
        });
        $("#txtErroForm").html("");
        $("#txtErroForm").addClass("hide");
        $("#ModalCambiarContraseña").modal("show");
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
    $("#btnCambiarContraseñaGuardar").on("click", function () {

        var Cont = 0;
        $("#formCambioContrasenia .validar").each(function () {
            if (!DBR.validarCampo(this)) {
                Cont++;
            }
        });

        var NewPassword = $("#txtNewPassword").val();
        var RepNewPassword = $("#txtRepNewPassword").val();
        if (NewPassword != RepNewPassword) {
            $("#txtErroForm").html("Las nuevas contraseñas no coinciden.");
            $("#txtErroForm").removeClass("hide");
            return false;
        } else {
            $("#txtErroForm").html("");
            $("#txtErroForm").addClass("hide");
        }

        if (Cont == 0) {
            EventosLayaut.CambiarPassword();
        }
    });
});