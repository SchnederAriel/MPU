
$(document).ready(function () {
    validateFechaFallecimiento();
    $('#btnSend').addClass("button--disabled--ddjj");

    /*
    createTitleBox({
        title: "Datos del nuevo decreto 614"
    });*/

    $("#fechaPresentacion").on("change keypress blur", function (e) {
        validarFechaPresentacion() ? false : validateFechaFallecimiento();

        cleanCheckbox();
        $(".search-result").addClass("hidden");
        $('#tablaGrupoFamiliar').dataTable().fnDestroy();
        $('input[name="selectedEmbargo"]').val("");

    });

    $('#periodoDesde').on('keyup', function () {

        if (oValidation.blurMaxLength("#periodoDesde")) {
            validarPeriodoDesde();
        }

        cleanCheckbox();
        $(".search-result").addClass("hidden");
        $('#tablaGrupoFamiliar').dataTable().fnDestroy();
        $('input[name="selectedEmbargo"]').val("");

        if ($('#periodoHasta').val() !== "") {
            validarPeriodoHasta();
        }

    });

    $('#periodoHasta').on('blur', function () {
        if ($(this).val() !== "") {
            if (!validarPeriodoDesde()) {
                validarPeriodoHasta();
            } else {
                $('#periodoDesde').focus();
            }
        } else if ($(this).val() === "") {
            oMessage.input.hide({
                id: "#periodoHasta"
            });
        }
    });

    $('input[name="periodoHastaR"]').on('blur', function () {
        oMessage.input.hide({
            id: '#modPeriodoHasta'
        });
        validarPeriodoHasta("#" + $(this).attr("id"));
    });

    buscarGrupoFamiliar();



});


$('#btnSend').click(function (event) {    
    event.preventDefault();

    if (validateCheckbox()) {
        return false;
    }

    var cantTotal = 0, cantReadOnly = 0, cantVacios = 0;

    $("input[name='periodoHastaR']").each(function (e) {
        cantTotal++;
        if ($(this).prop('readonly')) {
            cantReadOnly++;
        } else if ($(this).val() === "") {
            cantVacios++;
        }
    });

    if (cantTotal === cantReadOnly + cantVacios) {
        oMessage.input.show({
            id: '#modPeriodoHasta',
            message: "Ingrese al menos un período hasta"
        });
    }

    var selectedCuil = [];
    var selectedNombres = [];
    $('#checkboxes input[type=checkbox]:checked').each(function () {
        selectedCuil.push(this.id);
        selectedNombres.push(this.value);

    });
    if (selectedCuil.length > 0) {
        if ($('form').valid()) {
            $(this).attr("disabled", "disabled")
                .addClass("button--disabled--ddjj");

            $('#cantSeleccionados').val(selectedCuil.length);
            $('#VectorCuilesSeleccionados').val(selectedCuil);
            $('#VectorNombresSeleccionados').val(selectedNombres);
            $('form').submit();
        }
    } else {
        oMessage.alert.show({
            id: ".familiar-group .table-message",
            messages: ["Error - Debe seleccionar al menos una fila."],
            status: "warning"
        });

    }

});



function validateFechaFallecimiento() {
    var fechaFallecido = $("#fechaFallecimiento").val();
    var fechaPresentacion = $("#fechaPresentacion").val();

    if (fechaFallecido !== "" && fechaFallecido !== undefined && fechaFallecido !== null) {

        var error = false;
        var maxDate = new Date(dateFormat(fechaFallecido));
        var minYear = maxDate.getFullYear() - 2;
        var minDate = new Date();
        minDate.setFullYear(minYear);

        if (fechaPresentacion !== "" && fechaPresentacion !== undefined && fechaPresentacion !== null) {

            var param = {
                "ing": fechaPresentacion,
                "min": minDate.getDate() + "/" + (minDate.getMonth() + 1) + "/" + minDate.getFullYear(),
                "max": maxDate.getDate() + "/" + (maxDate.getMonth() + 1) + "/" + maxDate.getFullYear()
            };

            error = compararFechas(param);

            if (error) {
                oMessage.input.show({
                    id: "#fechaPresentacion",
                    message: "fecha presentacion mayor a la fecha de fallecimiento"
                });
            } else {
                oMessage.input.hide({
                    id: "#fechaPresentacion"
                });
            }

            if (error) {
                $('#tablaGrupoFamiliar').dataTable().fnDestroy();
                $(".search-result").addClass("hidden");

            } else {
                if (!validarPeriodoDesde()) {
                    //buscarGrupoFamiliar($("#periodoDesde").val());

                    if ($('#periodoHasta').val() !== "") {
                        validarPeriodoHasta();
                    }

                } else {
                    $('#tablaGrupoFamiliar').dataTable().fnDestroy();
                    $(".search-result").addClass("hidden");
                }

            }
        }

        //});
    }

    return error;

}

function buscarGrupoFamiliar() {
    //se destruye la tabla al iniciar la nueva búsqueda
    $('#tablaGrupoFamiliar').dataTable().fnDestroy();

    var dt = $('#tablaGrupoFamiliar').dataTable({
        "pageLength": 20,
        "searching": false,
        "info": false,
        "bLengthChange": false,
        "bSort": false,
        "ajax": {
            //asi me anduvo bien publicado "url": "AltaDecreto/Listado"
            //asi me funciono en local. "url": "Listado"
            "url": "Listado",
            "type": "GET",
            "datatype": "json",
            "data": { cuil : parseInt($('#cuil').val())},
            "dataSrc": function (json) {
                //Validamos que la tabla no venga vacia, sino mostramos un warning
                if (json.data.HayError) {
                    var msj = json.data.Mensaje;
                    oMessage.alert.show({
                        id: ".familiar-group .table-message",
                        messages: [msj],
                        status: "error"
                    });
                    $('#btnSend').addClass('hidden');
                    $('#sinRelaciones').addClass('hidden');



                } else {
                    if (json.data.Relaciones != null && json.data.Relaciones.length > 0) {
                        $(".familiar-group .table-container").removeClass("hidden");
                        $('#btnSend').removeClass('hidden');
                        $('#sinRelaciones').addClass('hidden');

                        oMessage.alert.hide({
                            id: ".familiar-group .table-message"
                        });

                        return json.data.Relaciones;


                    } else {
                        $('#btnSend').addClass('hidden');

                        $(".familiar-group .table-container").addClass("hidden");
                        
                        $('#sinRelaciones').removeClass('hidden');

                        //oMessage.alert.show({
                        //    id: ".familiar-group .table-message",
                        //    messages: ["Para continuar con este trámite es necesario que actualices los datos de tus hijas e hijos, ya que no registramos que se encuentren recibiendo asignaciones familiares. Podes hacerlo desde Atención Virtual o con turno en una oficina."],
                        //    status: "warning"
                        //});
                    }
                }

                deshabilitaBoton("btnSearch", false);

                return json.data;
            },
        },
        "columns": [
            {
                "render": function (data, type, full, meta) {
                    return crearCheckBox(full);
                }
            },
            { "data": "ApellidoYNombre" },
            { "data": "Cuil_Relacionado" },
            {
                "render": function (data, type, full, meta) {
                    return mostrarOtroProgenitor(full);
                }
            },
            { "data": "Relacion" }

        ],
        "initComplete": function () {
            var table = $('#tablaGrupoFamiliar').DataTable();
            table.rows().iterator('row', function (context, index) {
                let row = table.row(index);
                var html = createChildRow(row.data());
                if (html !="")
                    row.child(html,"advertencia").show();
            });
            $('table.dataTable.no-footer').css('border-bottom', '2px solid #dee2e6');
        },
        "createdRow": function (row, data, index) {
            $('td',row).addClass("fila");
        }
    });
}

function HabilitarBtnSend() {
    if ($('input[type = "checkbox"]').is(':checked')) {
        $('#btnSend').removeClass("button--disabled--ddjj");
        $('#msjError').hide();

    } else {
      
        $('#btnSend').addClass("button--disabled--ddjj");
        $('#msjError').show();


    }
}


function crearCheckBox(full) {
    var btn = '';
    if (!full.ErrorADP && !full.RenunciaVigente && !full.DepostioVigente && !full.DecretoVigente && !full.ArgentaVigente && (full.EsMenor18 || (!full.EsMenor18 && (full.Incapacidad === 'S')))) {
        btn += '<ul class="form-group-option">';
        btn += ' <li class="form-group__item">';
        btn += '<td class="table__title"> <input onclick="HabilitarBtnSend()" class="form__input-checkbox" type="checkbox" name="selectedEmbargo" value="' + full.ApellidoYNombre + " - " + full.Cuil_Relacionado + ";" + full.ApellidoYNombreEmbargado + " - " + full.Cuil_Embargado + '" id="' + full.Cuil_Relacionado + '"></td>';
        //btn += ' <div class="input-check-button"></div>';
        btn += ' </li>';
        btn += ' </ul>';
    }
    return btn;
}


function validarMostrarFecha(dato) {
    var fecha = formatDateDDMMYYYY(dato);

    if (dato === null) {
        fecha = "-";
    }
    return fecha;
}

function mostrarOtroProgenitor(full) {
    if (full.codigoRelacion !== 1 && full.codigoRelacion !== 2 && full.codigoRelacion !== 25) {
        if (full.Cuil_Embargado === null) {
            return " - ";
        } else {
            return full.ApellidoYNombreEmbargado ;
        }
    } else {
        return "-";
    }

}



function createChildRow(full) {
    var datos = [];
    var html = '';

    if (full.DepostioVigente || full.ArgentaVigente || full.DecretoVigente) {
        datos.push("CUIL con trámite de embargo vigente. Para realizar éste trámite deberá dirigirse con turno a una UDAI con documentación " +
            "donde conste que detenta el Cuidado Personal(Tenencia) de los menores mediante la Sentencia Judicial o Testimonio que lo avala");
    }   

    if (full.ErrorADP) {
    datos.push( "CUIL con datos incompletos en la Base de Datos ADP. Para realizar éste trámite deberá acreditar sus datos a través de " +
        "la Atención Virtual - actualización de datos personales y familiares, o bien dirigirse personalmente a una UDAI con la documentación" +
        " de la cartilla “actualización de vínculos familiares");
    }
    if (full.RenunciaVigente) {
        datos.push("Tenés registrada la renuncia al cobro de asignaciones familiares. Si querés modificar esto, podés realizar este trámite con turno en una oficina. ");
    }

    if (datos.length > 0) {

        html += '<div class="tooltip-container">';
        html += '<div class="tooltip__element__message" >';
        html += '<div class="fas fa-exclamation-triangle" > </div></div>';
         $.each(datos, function (index, item) {
             html += '<div class="message-advertencia">  ' + item+ '</div>';
         });
        html += '</div>';
    }


    return html;
}


function showDateProgenitor(full) {

    var dateStart,
        dateFinish;

    if (full.FechaInicioRelacion === null) {

        return false;

    } else {
        dateStart = full.FechaInicioRelacion;

        if (full.FechaHastaVigenciaRelacion === null) {

            return dateStart;

        } else {
            dateFinish = full.FechaHastaVigenciaRelacion;
            return dateStart + " - " + dateFinish;
        }

    }

}

