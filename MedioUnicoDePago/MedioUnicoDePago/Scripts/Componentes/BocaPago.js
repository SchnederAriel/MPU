$(document).ready(function () {

    //createTitleBox({
    //    title: "Forma de pago",
    //    tooltip: {
    //        elementId: ".box-tooltip",
    //        position: "top",
    //        type: "warning",
    //        message: ["Importante: Al elegir un nuevo medio, éste sustituirá permanentemente la opción anterior."]
    //    }
    //});

    //delayTooltip();
    // Buscar Bancos cuando tocas el enter
    if ($(".bocaAP input:focus")) {
        $(".bocaAP input").on("keyup", function (event) {
            // Number 13 is the "Enter" key on the keyboard
            if (event.keyCode === 13) {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                document.getElementById("btnSearch").click();
            }
        });
    }

    // Buscar Bancos cuando tocas el enter
    if ($(".bocaCBU input:focus")) {
        $(".bocaCBU input").on("keyup", function (event) {
            // Number 13 is the "Enter" key on the keyboard
            if (event.keyCode === 13) {
                // Cancel the default action, if needed
                event.preventDefault();
                // Trigger the button element with a click
                document.getElementById("btnValidar").click();
            }
        });
    }

    $('input[type="radio"].chkBDP').change(function () {
        if ($(this).is(':checked')) {
            switch ($(this).val()) {
                case "AP":
                    if ($("#hdnsolocbu").val() === '1') {
                        $('#agentePagador').attr('disabled', true);
                        $('label[for="agentePagador"]').css('opacity', '.2');
                        $('label[for="agentePagador"]').css('cursor', 'default');
                        oMessage.alert.show({
                            id: "#message-no-apoderado",
                            messages: ["CUIL MENOR NO POSEE APODERADO - REPRESENTANTE LEGAL"],
                            status: "warning"
                        });
                    }
                    else {
                        $(".bocaAP").removeClass("hidden");
                        $(".bocaCBU").addClass("hidden");
                        $(".bocaBVIRTUAL").addClass("hidden");
                        $("#CBU").addClass("hidden");
                        $("#CBU").val('');
                        $("input[name='codBanco']").focus();
                        $("#btnSearch").removeClass("hidden");
                    }
                    break;
                case "BVIRTUAL":
                    $(".bocaAP").addClass("hidden");
                    $(".bocaCBU").addClass("hidden");
                    $("#CBU").addClass("hidden");
                    $("select[id='listbvirtual']").focus();
                    $("#btnSearch").addClass("hidden");
                    $(".bocaBVIRTUAL").removeClass("hidden");
                    break;
                case "CBU":
                    $(".bocaAP").addClass("hidden");
                    $(".bocaBVIRTUAL").addClass("hidden");
                    $(".bocaCBU").removeClass("hidden");
                    $("#CBU").removeClass("hidden");
                    $("input[name='CBU']").focus();
                    $("#btnSearch").addClass("hidden");

                    break;
            }


            $(".search-result").addClass("hidden");

        }

        oMessage.input.hide({
            id: "#CBU"
        });
    });

    $('input[type="radio"].chkBDP').change();
    $("#valCBUError").hide();
    if ($("#ErrorCoelsa").val() != "")
    {
        $("#valCBUError").show();
    }
        
    $("#btnSearch").click(function (e) {
        e.preventDefault();
    });

    $(".button--clean-form").click(function () {
        $("#cbu").prop("checked", "checked");
        $("#CBU").removeClass("hidden");
        //$("#agentePagador").prop("checked", "checked");
        //$(".bocaAP").removeClass("hidden");
        //$(".bocaCBU").addClass("hidden");
        $('.search-result').addClass('hidden');
        $("#fechaPresentacion").focus();

    });
    if ($("#hdnsolocbu").val() === '1') {

        if ($("#hdnsolocbu").val() === '1') {
            //$("#chkAgentePagador").addClass("hidden");
            $('#agentePagador').attr('disabled', true);
            $('label[for="agentePagador"]').css('opacity', '.2');
            $('label[for="agentePagador"]').css('cursor', 'default');
            oMessage.alert.show({
                id: "#message-no-apoderado",
                messages: ["CUIL MENOR NO POSEE APODERADO - REPRESENTANTE LEGAL"],
                status: "warning"
            });
        }
    }
    else {
      $("#chkAgentePagador").removeClass("hidden");

    }

    $('#btnSend').click(function (e) {
        e.preventDefault(); // Se previene el envio del form

        oMessage.alert.hide({
            id: ".tableBDP #message-table"
        });

        oMessage.input.hide({
            id: "#CBU"
        });

        var tipoBDP = $('input[name=tipoBocaPago]:checked').val();

        //if (validarFechaPresentacion()) {
        //    return false;
        //}

        if (tipoBDP === "AP") {
            var agPg = $('input[name=selectedAgPag]:checked').val();

            if (agPg === '' || agPg === undefined) // se comprueba que tenga algun valor seleccionado
            {
                $('input[name=selectedAgPag]').addClass("input-validation-error");
                oMessage.alert.show({
                    id: ".tableBDP #message-table",
                    messages: ["No ha seleccionado ningún banco/agencia."],
                    status: "warning"
                });

                return false;

            } else {

                var resultAP = agPg.split(",");

                document.getElementById('codBanco').value = resultAP[0];
                document.getElementById('codAgencia').value = resultAP[1];

                //if ($('#hdnCodBanco').val() === $('#codBanco').val() && $('#hdnCodAgencia').val() === $('#codAgencia').val()) {

                //    oMessage.alert.show({
                //        id: ".tableBDP .table-message",
                //        messages: ["Banco y agencia seleccionado igual al vigente"],
                //        status: "warning"
                //    });

                //    return false;

                //}

                oMessage.alert.hide({
                    id: ".bocaAP #table-message"
                });


                if ($('#hdnsolocorreo').val() === '1' && ($('#codBanco').val() != '910' && $('#codBanco').val() != '911' && $('#codBanco').val() != '913' && $('#codBanco').val() != '914')) {

                    oMessage.alert.show({
                        id: ".tableBDP #message-table",
                        messages: ["Para menor solo permite BDP CORREO"],
                        status: "warning"
                    });

                    return false;

                }
                if ($('#codBanco').val() === '910' && $('#codAgencia').val() === '888') { //if ($('#hdnMenor').val() === '1' && $('#codBanco').val() === '910' && $('#codAgencia').val() === '888')

                    oMessage.alert.show({
                        id: ".tableBDP #message-table",
                        messages: ["ESTA INHABILITADA LA COMBINACIÓN DE SELECCIÓN DE BANCO 910 Y AGENCIA 888"],
                        status: "warning"
                    });

                    return false;

                }
                if ($('#hdnMenor').val() === '0' && ($('#codBanco').val() === '913' || $('#codBanco').val() === '914')) {
                    oMessage.alert.show({
                        id: ".tableBDP #message-table",
                        messages: ["BDP CORREO inhibido, debe ser 910 o 911."],
                        status: "warning"
                    });

                    return false;
                }

            }
        } else if (tipoBDP === "CBU") {
            var cbu = $('#CBU').inputmask('unmaskedvalue');

            if (validarCBUBDP()) {
                return false;
            }


        }

        if (!validarFormulario() && $('form').valid()) {

            $("#formBDP").submit();

        }
    });

    validateSubmit();

    resizeButtons();

    //Buscar Localidades cada vez que se cambie el Select de Provincias.
    $('#codProvincia').on("change", function (event) {
        if ($('#codProvincia').val() == null || $('#codProvincia').val() == "" || $('#codProvincia').val() == "-1") {

            $('#codLocalidad').attr("disabled", "disabled");
            $('#codLocalidad').val('-1');

        } else {
            var url = "buscarLocalidadesXProvincia"; //ESTO ANDA PUBLICADO
            //var url = "MedioUnicoDePago/Home/buscarLocalidadesXProvincia";// ESTO NO ANDA PUBLICADO
            if ($("#hidAmbiente").val() == "DESA")
                url = "/Home/buscarLocalidadesXProvincia";

            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                data: {
                    codProvincia: encodeURIComponent($('#codProvincia').val())
                },
                success: function (result) {
                    // Parse the returned json data
                    var opts = result.data;
                    $('#codLocalidad')
                        .empty()
                        .append('<option value="-1">SELECCIONE</option>'); 
                    // Use jQuery's each to iterate over the opts value
                    $.each(opts, function (i, d) {
                        // You will need to alter the below to get the right values from your json object.  Guessing that d.id / d.modelName are columns in your carModels data
                        $('#codLocalidad').append('<option value="' + d.id + '">' + d.descripcion + '</option>');
                    });
                    //console.log(result);
                    $('#codLocalidad').removeAttr("disabled"); 
                    error = result.Error;
                },
                async: false
            });
            $('#codLocalidad').removeAttr("disabled");
        }
    })
});

function resizeButtons() {
    if ($(this).width() < 620) {
        $('input[name="selectedAgPag"]').removeClass('form__input--radio');
        $('.input-radio').removeClass('input-radio-button');
        $('input[name="tipoBocaPago"]').removeClass('form__input--radio');

    }
    else {
        $('.input-radio').addClass('input-radio-button');
        $('input[name="selectedAgPag"]').addClass('form__input--radio');
        $('input[name="tipoBocaPago"]').addClass('form__input--radio');

    }
}

$(window).resize(function () {
    resizeButtons();
});
//Funcion para enviar formulario
//validaciones antes de enviar el formulario
function validateSubmit() {
    $('#btnSend, #btnSend1').click(function (e) {
        e.preventDefault(); // Se previene el envio del form

        oMessage.alert.hide({
            id: ".tableBDP #message-table"
        });

        oMessage.input.hide({
            id: "#CBU"
        });

        var tipoBDP = $('input[name=tipoBocaPago]:checked').val();

        //if (validarFechaPresentacion()) {
        //    return false;
        //}

        if (tipoBDP === "AP") {
            var agPg = $('input[name=selectedAgPag]:checked').val();

            if (agPg === '' || agPg === undefined) // se comprueba que tenga algun valor seleccionado
            {
                $('input[name=selectedAgPag]').addClass("input-validation-error");
                oMessage.alert.show({
                    id: ".tableBDP #message-table",
                    messages: ["No ha seleccionado ningún banco/agencia."], 
                    status: "warning"
                });

                return false;

            } else {

                var resultAP = agPg.split(",");

                document.getElementById('codBanco').value = resultAP[0];
                document.getElementById('codAgencia').value = resultAP[1];

                //if ($('#hdnCodBanco').val() === $('#codBanco').val() && $('#hdnCodAgencia').val() === $('#codAgencia').val()) {

                //    oMessage.alert.show({
                //        id: ".tableBDP .table-message",
                //        messages: ["Banco y agencia seleccionado igual al vigente"],
                //        status: "warning"
                //    });

                //    return false;

                //}

                oMessage.alert.hide({
                    id: ".bocaAP #table-message"
                });


                if ($('#hdnsolocorreo').val() === '1' && ($('#codBanco').val() != '910' || $('#codBanco').val() != '911' || $('#codBanco').val() != '913' || $('#codBanco').val() != '914')) {

                    oMessage.alert.show({
                        id: ".tableBDP #message-table",
                        messages: ["Para menor solo permite BDP CORREO"],
                        status: "warning"
                    });

                    return false;

                }
                if ($('#codBanco').val() === '910' && $('#codAgencia').val() === '888') { //if ($('#hdnMenor').val() === '1' && $('#codBanco').val() === '910' && $('#codAgencia').val() === '888')
                    oMessage.alert.show({
                        id: ".tableBDP #message-table",
                        messages: ["ESTA INHABILITADA LA COMBINACIÓN DE SELECCIÓN DE BANCO 910 Y AGENCIA 888"],
                        status: "warning"
                    });

                    return false;

                }
                if ($('#hdnMenor').val() === '0' && ($('#codBanco').val() === '913' || $('#codBanco').val() === '914')) {
                    oMessage.alert.show({
                        id: ".tableBDP #message-table",
                        messages: ["BDP CORREO inhibido, debe ser 910 o 911."],
                        status: "warning"
                    });

                    return false;
                }

            }
        } else {
            var cbu = $('#CBU').inputmask('unmaskedvalue');

            if (validarCBUBDP()) {
                return false;
            }


        }

        if (!validarFormulario() && $('form').valid()) {

            $("#formBDP").submit();

        }
    });
}

//Iniciar Datatable busqueda de boca de pago
function buscarBDP() {
    var datos = {
        P_C_BCO: $('#codBanco').val(),
        P_C_AGE: $('#codAgencia').val(),
        P_C_PCIA: '-1',
        P_D_LOCA: $("#codLocalidad option:selected").text().trim(),
        P_C_POSTAL: $('#codPostal').val(),
        P_Solo_Correo: $('#hdnsolocorreo').val(),
        P_EsMenor: $('#hdnMenor').val()
    };

 

    $('#busquedaBDP').dataTable().fnDestroy();

    //Debe completar al menos un filtro    

    // Se valida que fecha de presentación tenga valor
    //if (datos.F_Presentacion !== '' && !validarFechaPresentacion()) {
    //    oMessage.input.hide({
    //        id: "#fechaPresentacion"
    //    });

        // se valida que haya seleciconado al menos un campo de búqueda
        if (datos.P_C_BCO !== '' || datos.P_C_AGE !== '' || datos.P_C_PCIA !== '-1' || datos.P_C_POSTAL != '') {

            $('#valfiltros').addClass('hidden');
            $('.search-result').removeClass('hidden');

            animateScroll('.search-result');
        } else {

            $('#valfiltros').removeClass('hidden');
            $('.search-result').addClass('hidden');
            return false;
        }

    //} else {
    //    oMessage.input.show({
    //        id: "#fechaPresentacion",
    //        message: "Fecha presentación no válida"
    //    });

    //    animateScroll('#fechaPresentacion');
    //    $("#fechaPresentacion").focus();

    //    return false;

    //}

    var url = "buscarAgentePagador"; //ESTO ANDA PUBLICADO
    //var url = "MedioUnicoDePago/Home/buscarAgentePagador"; //ESTO NO ANDA PUBLICADO
    if ($("#hidAmbiente").val() == "DESA")
        url = "/Home/buscarAgentePagador";


    var tables = $('#busquedaBDP').dataTable({
        "orderMulti": false, // for disable multiple column at once  
        "bLengthChange": false,
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json",
            "data": datos,
            "dataSrc": function (json) {

                $(".search-result").removeClass("hidden");
                //Validamos que la tabla no venga vacia, sino mostramos un warning
                if (json.error) {
                    oMessage.alert.show({
                        id: ".bocaAP #table-message",
                        messages: [json.mensaje],
                        status: "warning"
                    });

                    //json.data = [];
                } else {
                    $(".familiar-group .table-container").addClass("hidden");
                    oMessage.alert.hide({
                        id: ".bocaAP #table-message"
                    });

                }
                return json.data;

            },
            "async": false
        },
        "columns": [
            {
                "render": function (data, type, full, meta) {
                    return crearRadioButton(full);
                }
                //},
                //"width": "5%"
            },
            {
                "data": "Desc_Banco"/*, "width": "20%"*/,
                "render": function (data, type, row, meta) {
                    return leftFillNum(row["Cod_Banco"], 3) + " - " + data;
                }
            },
            {
                "data": "Desc_Agencia"/*, "width": "20%"*/,
                "render": function (data, type, row, meta) {
                    return leftFillNum(row["Cod_Agencia"], 3) + " - " + data;
                }
            },
            { "data": "Calle"/*, "width": "30%"*/ },
            { "data": "Localidad"/*, "width": "20%"*/ },
            { "data": "Cod_Postal"/*, "width": "5%"*/ }
        ]
    });
    resizeButtons();

    //$.fn.dataTable.ext.errMode = 'none';
    oMessage.alert.hide({
        id: ".tableBDP #message-table"
    });

    var menor = $('#hdnMenor').val();
    //if (menor==="1") {
    //    oMessage.alert.show({
    //        id: ".tableBDP #message-table",
    //        messages: ["Para menor solo permite BDP CORREO"],
    //        status: "warning"
    //    });
    //}
}

function crearRadioButton(full) {
    var radiobtn = '';
    radiobtn = '<ul class="form-group-option"> ';
    radiobtn += '  <li class="form-group__item">';
    radiobtn += '<input type="radio" class="form__input--radio" value="' + full.Cod_Banco + ',' + full.Cod_Agencia + '" name="selectedAgPag" id="selectedAgPag">';
    radiobtn += '<div class="input-radio-button input-radio"></div>';
    radiobtn += ' </li>';
    radiobtn += '</ul>';
    return radiobtn;
}

function EsCVU() {
    var cbu = $('#CBU').inputmask('unmaskedvalue');
    var cbuNum1 = cbu.substr(0, 3);
    var ret = cbuNum1 === "000" ? true : false;
    return ret;

}

function CBUvalidarPreexistencia() {
    var preexiste = false;

    var cbu = $('#CBU').inputmask('unmaskedvalue');

    //validar cbu correcto y si es igual al anterior
    if (validarCBUlocal(cbu)) {


        var resultcbu = $('#CBU').val().split("-");

        document.getElementById('cbuInicio').value = resultcbu[0];
        document.getElementById('cbuFinal').value = resultcbu[1];

        //if ($('#hdnCbuInicio').val() === $('#cbuInicio').val() && $('#hdnCbuFinal').val() === $('#cbuFinal').val()) {

        //    oMessage.input.show({
        //        id: "#CBU",
        //        message: "CBU ingresado igual al vigente"
        //    });

        //    $("#CBU").focus();

        //    return true;
        //}


    } else {
        return "vacio";
    }
    return preexiste;
}

function validarCBUBDP() {
    var error = false;
    $('#valCBU').addClass('hidden');

    if ($("#CBU").val() === "") {
        error = true;
        $("#CBU").focus();

        oMessage.input.show({
            id: "#CBU",
            message: "Ingrese CBU"
        });

    } else if (EsCVU()) {
        error = true;
        oMessage.input.show({
            id: "#CBU",
            message: "Debe ingresar un CBU. No está permitido ingresar un CVU"
        });
    }else if (CBUvalidarPreexistencia()) {
        error = true;
    } else if (EsCbuInhibido($("#CBU").val(), $("#ListadoBancosInhibidos").val())) {
        error = true;
        oMessage.input.show({
            id: "#CBU",
            message: "La CBU ingresada esta INHIBIDA como Medio de Pago de Asignaciones"
        });
    } else {


        var cbu = $('#CBU').inputmask('unmaskedvalue');
        cuilTitular = $('.titular__cuil').text();
        $.ajax({
            url: "validarCBU",
            type: 'GET',
            dataType: 'json',
            data: {
                cbu: encodeURIComponent(cbu.trim()),
                cuil: encodeURIComponent(cuilTitular.trim())
            },
            success: function (result) {
                //console.log("RESPONSE -> " + result);
                if (result.data.Respuesta === null) {
                    return false;
                }
                if ((result.data.Respuesta === "00") || (result.data.Respuesta === "0")) {

                    $('#valCBU').text(result.data.Mensaje);
                    $('#valCBU').removeClass('hidden');

                    oMessage.input.hide({
                        id: "#CBU"
                    });

                    error = false;

                } else {
                    $('#valCBU').addClass('hidden');

                    oMessage.input.show({
                        id: "#CBU",
                        message: result.data.Mensaje
                    });
                    error = true;
                }

            },
            async: false
        });
    }

    return error;
} 

function EsCbuInhibido(cbu, bancosInhibidos) {
    let cbuNum1 = cbu.substr(0, 3);
    return bancosInhibidos.includes(cbuNum1) ? true : false;
}
