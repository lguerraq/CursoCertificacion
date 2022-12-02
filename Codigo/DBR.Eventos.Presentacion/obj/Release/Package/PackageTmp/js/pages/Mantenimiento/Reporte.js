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
        GrillaReporteCorreos: function () {
            $('#gridReporteCorreo').DataTable({
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
                    "url": urlListCorreoReportePaged,
                    "type": 'POST',
                    "datatype": "json",
                    "data": {},
                },
                "columns": [
                            { "data": "Año", "sClass": 'text-center' },
                            {
                                "data": function (row, type, set, meta) {
                                    return parseIntMes(row.Mes);
                                }, "sClass": 'text-left'
                            },                           
                            {
                                "data": function (row, type, set, meta) {
                                    return Funciones.Moneda(row.TotalEnviados.toString());
                                }, "sClass": 'text-right'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return Funciones.Moneda(row.EnviadosGratis.toString());
                                }, "sClass": 'text-right'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return Funciones.Moneda(row.EnviadosAdicionales.toString());
                                }, "sClass": 'text-right'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return "S/. " + row.CostoEnvioAdicional.toFixed(4);
                                }, "sClass": 'text-right'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return "S/. " + row.TotalCostoAdicional.toFixed(2);
                                }, "sClass": 'text-right'
                            }
                ],
                "lengthMenu": [50, 100, 500],
                "pageLength": 50,
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    total = api
                        .column(6)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b.replace("S/. ", ""));
                        }, 0);


                    // Update footer
                    $(api.column(6).footer()).html(
                        "S/. " + total.toFixed(2)
                    );
                }
            });
        },
    }
}


var Funciones = {
    Moneda: function(entrada) {
        var num = entrada.replace(/\./g, "");
        if (!isNaN(num)) {
            num = num.toString().split("").reverse().join("").replace(/(?=\d*\.?)(\d{3})/g, "$1 ");
            num = num.split("").reverse().join("").replace(/^[\.]/, "");
            entrada = num;
        }
        return entrada;
    }
}
$(document).ready(function () {

    config.iniGrids.GrillaReporteCorreos();

});