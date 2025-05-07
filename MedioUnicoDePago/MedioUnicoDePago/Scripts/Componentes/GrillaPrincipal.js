$(document).ready(function () {

    //inicializar el uso de popovers
    $('[data-toggle="popover"]').popover();

    deshabilitaBoton("genreclamos", true);

    //Acción de click en los filtro períodos
    $(".icon--search").on("click", function (e) {
        e.stopPropagation();
        //¡¡¡¡¡¡ NO USAR NUNCA EL TRIGGER DE JQUERY porque en IE9 explota !!!!!!!
        $("#btnLiquiPeriodos").click();
        //console.log({ CUIL: $("#hCUIL").val(), PE_PAGO_DESDE: $("#liquidesde").val().replace("/", ""), PE_PAGO_HASTA: $("#liquihasta").val().replace("/", ""), SISTEMA: $("#Sistemas").val() });
    });

    ////Expandir/Colapsar grilla
    //$(".table__link").on("click", function (e) {
    //    e.preventDefault();

    //    if ($("#Grilla").hasClass("tblchica")) {
    //        $("#vermasp").text("ver menos períodos");
    //        $("#Grilla").removeClass("tblchica");
    //        $("#Grilla").addClass("tblgrande");
    //        $(".table__link .arrow--down").addClass("rotate-180");
    //    }
    //    else {
    //        $("#vermasp").text("ver más períodos");
    //        $("#Grilla").removeClass("tblgrande");
    //        $("#Grilla").addClass("tblchica");
    //        $("html, body").animate({ scrollTop: 0 }, "fast");
    //        $(".table__link .arrow--down").removeClass("rotate-180");
    //    }

    //    return false;

    //});

    //Modal de reclamos de períodos seleccionados de la grilla principal.
    //$("#genreclamos").on("click", function (e) {
    //    e.preventDefault();
    //    e.stopPropagation();

    //    rows = $('#Grilla').data('kendoGrid').select();
    //    if (rows.length > 0) {
    //        $('#lupa-modal-reclamos').modal('show');
    //    }
    //});

    //Foco en FP al abrir el modal
    //$('#lupa-modal-reclamos').on('shown.bs.modal', function () {
    //    $("#fechaPresentacion").focus();
    //});

    //Reseteo de campos al cerrar el modal de reclamos
    //$('#lupa-modal-reclamos').on('hidden.bs.modal', function (e) {
    //    $(".msjreclamo").addClass("hidden");
    //    $("#fechaPresentacion").val('');
    //    $("#btnGenerarReclamos").removeClass("hidden");
    //    $(".fpreclamos").removeClass("hidden");
    //    $("#btnNoReclamos").html("No");

    //});

    //Funcionalidad para generar reclamos (POPUP) desde grilla principal
    //$("#btnGenerarReclamos").on("click", function (e) {

    //    if ($("#fechaPresentacion").val() === '' || validarFormulario()) {
    //        //detengo todo y salgo de la función.
    //        e.preventDefault();
    //        return false;
    //    }

    //    //El método en el ActionResult necesita un array de objetos (ParamReclamos en el controller)
    //    var reclamos = [];
    //    var cuilTitular = $('.titular__cuil').text();
    //    var rows = $('#Grilla').data('kendoGrid').select();

    //    //por cada fila seleccionada completo los datos de reclamos a pasar.
    //    rows.each(function (e) {
    //        var dataItem = $("#Grilla").data("kendoGrid").dataItem(this);
    //        reclamos.push({
    //            periodo: dataItem.PERIODOPAGO,
    //            beneficio: dataItem.NRO_BEN,
    //            rendicion: dataItem.CODESTADORENDICION,
    //            tipoliq: dataItem.CODTIPOLIQ,
    //            codigoproducto: dataItem.CODIGOPRODUCTO,
    //            codigoestadopago: dataItem.CODESTADOPAGO //Se agrega estado_pago para diferenciar los reclamos de SUAF y UVHI 
    //        });
    //    });

    //    var grid = $("#Grilla").getKendoGrid();
    //    //Envío cuil, FP y array de objetos {cuil, beneficio, rendición, tipoliqui}
    //    $.ajax({
    //        url: "GenerarReclamos",
    //        type: 'POST',
    //        dataType: 'json',
    //        data: {
    //            cuil: encodeURIComponent(cuilTitular.trim()),
    //            fechapresentacion: $("#fechaPresentacion").val(),
    //            reclamos: reclamos
    //        },
    //        success: function (result) {
    //            //console.log(result);
    //            var msj = "";

    //            result.tramites.forEach(function (item, index, arr) {
    //                msj += arr[index] + "&nbsp;<br>";
    //            });

    //            $(".msjreclamo").html(msj);
    //            $(".msjreclamo").removeClass("hidden");
    //            $(".fpreclamos").addClass("hidden");
    //            $("#btnGenerarReclamos").addClass("hidden");
    //            $("#btnNoReclamos").html("Cerrar");

    //            $("#Grilla").getKendoGrid().clearSelection();
    //            if (grid.select().length == 0) {
    //                deshabilitaBoton("genreclamos", true);
    //            }

    //        }
    //    });
    //});

});

function onChange(e) {
    grid = e.sender;
    var currentDataItem = grid.dataItem(this.select());


    //// Selección sólo con un click: https://www.telerik.com/forums/multiple-row-selection-without-holding-ctrl-key
    //$("#Grilla tbody").on("click", "tr", function (e) {        
    var rowElement = this;
    var row = $(rowElement);
    var grid = $("#Grilla").getKendoGrid();

    if (row.hasClass("k-state-selected")) {

        var selected = grid.select();

        selected = $.grep(selected, function (x) {
            var itemToRemove = grid.dataItem(row);
            var currentItem = grid.dataItem(x);
            return itemToRemove.PERIODOPAGO != currentItem.PERIODOPAGO;
        });

        //console.log(selected);
        grid.clearSelection();
        grid.select(selected);
        e.preventDefault();
        //e.stopPropagation();
    } else {
        grid.select(row);
        //e.stopPropagation();
        e.preventDefault();

    }

    if (grid.select().length > 0) {

        deshabilitaBoton("genreclamos", false);
    }
    else {

        deshabilitaBoton("genreclamos", true);
    }

    // });

}