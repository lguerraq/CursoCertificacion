var EventosLayautAula = {
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
    EventosLayautAula.ValidarUsuarioActividad();
    setInterval(function () {
        EventosLayautAula.ValidarUsuarioActividad();
    }, 60000);
});