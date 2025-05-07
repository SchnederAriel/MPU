$(document).ready(function () {

    $('.datepickerFPresentacion').datepicker(
        {
            format: 'dd/mm/yyyy',
            autoclose: true,
            language: 'es',
            zIndexOffset: 10,
            startDate: '04/11/2020',
            endDate: '+0d',
            clearBtn: true
        });

    createTitleBox({
        title: "Datos del nuevo decreto"
    });

  

    var dt = $('#tablaGrupoFamiliar840').dataTable({
        "pageLength": 20
    });
    dt.fnDestroy();

})



$("input").change(function () {
    var $input = $(this);
    $input.is(":checked");
}).change();

$('#btnSend840').click(function (event) {
    var selectedCuil = [];
    var selectedNombres = [];
    $('#checkboxes input[type=checkbox]:checked').each(function () {
        selectedCuil.push(this.id);
        selectedNombres.push(this.value);
       
    });
    if (selectedCuil.length > 0) {
        if ($('form').valid()) {

            $(this).attr("disabled", "disabled")
                .addClass("button--disabled");

            oMessage.alert.hide({
                id: ".familiar-group .table-message"
            });
            $('#cantSeleccionados').val(selectedCuil.length);
            $('#VectorCuilesSeleccionados').val(selectedCuil);
            $('#VectorNombresSeleccionados').val(selectedNombres);
            $('form').submit();
        } else {
            oMessage.alert.show({
                id: ".familiar-group .table-message",
                messages: ["Error - Verifique la fecha de presentación."],
                status: "warning"
            });
        }
    } else {
        oMessage.alert.show({
            id: ".familiar-group .table-message",
            messages: ["Error - Debe seleccionar al menos una fila."],
            status: "warning"
        });
        
    }


});