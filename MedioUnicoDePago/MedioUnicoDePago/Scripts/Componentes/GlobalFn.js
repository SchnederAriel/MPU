// Objeto para manejar spinner
/* Creado por Lucas Silberberg 08/02/2019 */
// @function show        Muestra el spinner
// @function hide        Oculta el spinner
// @function text        Agrega texto (@param = texto a agregar)
var oSpinner = {
    show: function () {
        // Se muestra spinner
        document.getElementById("spinner-container").style.display = "block";
        // Se quita el scroll de la pantalla
        document.body.style.overflow = 'hidden';

        text = "cargando datos, espere por favor";
        $(".spinner__text").text(text);

    },
    hide: function () {
        // Se oculta spinner
        document.getElementById("spinner-container").style.display = "none";
        // Se agrega scroll
        document.body.style.overflow = 'auto';
    },
    text: function (text) {

        if (text !== "" && text !== undefined) {
            text = text;
        } else {
            text = "cargando datos, espere por favor";
        }

        //document.getElementsByClassName("spinner__text").innerHTML = text ;  
        $(".spinner__text").text(text);
    }

};

// Objeto para crear modales
/* Creado por Lucas Silberberg 27/05/2019 */
// @function create        Crea el modal
// @function header        Crea el contenido del header
// @function body          Crea el contenido del body
// @function footer        Crea el contenido del footer
// @function destroy       Destruye el modal
var oModal = {
    // Crea el modal
    create: {
        // Funcion para crear el modal
        /* 
        * @id                       id del modal
        * @close                    true, permite cerrar el modal haciendo click fuera del mismo / False, no se puede cerrar sin hacer click al botón 
        * 
        */
        modal: function (id, close) {


            if (id === null || id === undefined || id === "") {
                console.error("no ingresó ningun id");
                return false;
            }

            var html = "<div class='modal fade' id=' " + id + " 'tabindex='-1' role='dialog' aria-labelledby='exampleModalCenterTitle' aria-hidden='true'>";
            html += "  <div class='modal-dialog modal-dialog-centered' role='document'>";
            html += " <div class='modal-content'>";
            html += " <div class='modal-header'></div>";
            html += " <div class='modal-body'></div>";
            html += " <div class='modal-footer'></div>";
            html += " </div>";
            html += " </div>";
            html += " </div>";


            if (!close) {
                $(id).attr({
                    "data-keyboard": false,
                    "data-backdrop": 'static'
                });
            }

            $('body').prepend(html);

        },
        // Funcion para crear el modal
        /* 
        * @title                    Titulo del modal
        * 
        */
        header: function (title) {

            $(".modal-header").append("<h3>" + title + "</h3>");

        },

        body: function (obj) {

            /*
            {[
                title: titulo,
                content : [{
                    label: label,
                    text: texto
                }],
                [{
                    label: label,
                    text: texto
                }],
            ]}
        
            */

            for (var i = 0; i < obj.length; i++) {

                $(".modal-body").append('<div class="details-container"><div class="details__title">' + obj[i].title + '</div></div>');

                for (var j = 0; j < obj.content.length; j++) {
                    var col = "<div class='col-md-6'>";
                    col += "<div class='details__label'>" + obj[i].content[i].text + "</div>";
                    col += "<div class='details__text text--important'>" + obj[i].content[i].label + "</div>";
                    col += "</div>";


                    // ¿Es par?
                    if (i % 2) {

                        $(".details-container").append('<div class="row"></div>').append(col);
                        i++
                    } else {
                        $(".details-container").find(".row:last").append(col);
                    }
                }

            }


        },

        footer: function () {
            //$(".modal-footer")
        },
    },



    destroy: function (id) {
        $(id).remove();
    }
}



// Objeto para mostrar mensajes de Alerta y de notificacion en inputs (Success, Error y Warning).
/* Creado por Lucas Silberberg 17/02/2019 */
var oMessage = {

    // Muestra un mensaje de bloque en modo de alerta (@show muestra / @hide oculta )
    alert: {
        // Funcion para mostrar mensajes de Success, Error y Warning.
        /* 
        * @id                       id, clase o etiqueta en donde imprimimos el mensaje
        * @title                    Titulo del mensaje ( opcional )
        * @messages                 [] De mensajes
        * @status                   Tipo de status ( succes / error / warning )
        * 
        */
        show: function (obj) {

            try {

                // Declaración de variables
                var oStatus = getStatus(obj.status),
                    title = oStatus.title;

                // Si no se ingresa un titulo, dejamos uno por default de la función "getStatus"
                if (obj.title !== undefined && obj.title !== "")
                    title = obj.title;

                // Si no ingresa un id válido no permite continuar ya que es dato obligatorio
                if (obj.id === undefined || obj.id === "" || obj.id === null) {
                    console.error("No se ingresó un ID válido.");
                    return;
                }

                // HTML del mensaje
                var html = '<!-- Contenedor principal para los mensajes-->';
                html += '<div class="message-container">';
                html += '     <div class="' + oStatus.css + '">';
                html += '         <!-- Titulo del mensaje-->';
                html += '         <div class="message__title">' + title + ' </div>';
                html += '        <!-- Listado de mensajes -->';
                html += '         <ul class="list__group" style="text-transform:none;">';
                html += '         </ul>';
                html += '     </div>';
                html += ' </div>';

                // Si el elemento existe en el html, se muestra el mensaje
                if (!$(obj.id)) {
                    console.error("No éxiste el objeto donde insertar el mensaje");
                    return;
                } else {
                    $(obj.id).html(html);
                }

                // Listado de mensajes
                for (var i = 0; i < obj.messages.length; i++) {
                    $(obj.id + " .list__group").append('<li class="list__item">' + obj.messages[i] + '</li>');
                }

                animateScroll(obj.id);

            } catch (e) {
                console.error(e);
            }

        },
        // Funcion para ocultar mensajes de Success, Error y Warning.
        hide: function (obj) {

            // Vacia el mensaje
            $(obj.id).empty();

        }
    },

    // Muestra un mensaje de debajo de un input (@show muestra / @hide oculta )
    input: {

        // Funcion para mostrar mensajes en el input.
        /* 
        * @id                       id, clase o etiqueta en donde imprimimos el mensaje
        * @message                  Mensaje a mostrar
        * 
        */
        show: function (obj) {

            if ($(obj.id + " + .form__message.form__message--error span").attr("data-name") || $(obj.id + " + .form__message.form__message--error.field-validation-valid").attr("data-name")) {

                $(obj.id + " + .form__message.form__message--error span").text(obj.message);

                $(obj.id + " + .form__message.form__message--error.field-validation-valid").text(obj.message);

            } else {
                $(obj.id + " + .form__message.form__message--error span").text(obj.message)
                    .attr("data-name", "messageError");

                $(obj.id + " + .form__message.form__message--error.field-validation-valid").text(obj.message)
                    .attr("data-name", "messageError");
            }

            //if ($(obj.id + " + .form__message.form__message--error.field-validation-valid")) {
            //    $(obj.id + " + .form__message.form__message--error").addClass("field-validation-error");
            //    $(obj.id + " + .form__message.form__message--error").removeClass("field-validation-valid");
            //    $(obj.id + " + .form__message.form__message--error").attr("data-name", "messageError");
            //    $(obj.id + " + .form__message.form__message--error.field-validation-error").html("<span id='" + obj.id.substr(1, obj.id.length) + "-error'>" + obj.message + "</span>");

            //    $(obj.id).removeClass("valid");
            //    $(obj.id).addClass("input-validation-error");

            //} else {
            //    $(obj.id + " + .form__message.form__message--error span").text(obj.message).attr("data-name", "messageError");
            //}


        },

        // Funcion para ocultar mensajes en el input.
        hide: function (obj) {

            $(obj.id + " + .form__message.form__message--error span[data-name='messageError']").empty()
                .removeAttr("data-name");

            $(obj.id + " + .form__message.form__message--error.field-validation-valid[data-name='messageError']").empty()
                .removeAttr("data-name");
        }
    }


};


// Funcion para obtener datos por status
/* Creado por Lucas Silberberg 17/02/2019 */
/*
* @status                   succes / error / warning   
* @title                    Titulo del mensaje ( opcional )

* @return                   Devuelve un objeto (@css estilo de css / @title titulo por default )
*/
function getStatus(status) {

    var oStatus = {};

    switch (status) {
        // status ok
        case "success":
            oStatus.css = "message--success";
            oStatus.title = "éxito";
            break;
        // status  Warning
        case "warning":
            oStatus.css = "message--warning";
            oStatus.title = "advertencia";
            break;
        // status error
        case "error":
            oStatus.css = "message--error";
            oStatus.title = "error";
            break;
    }

    return oStatus;
}


// Funcion para crear el titulo de cada box
/* Creado por Lucas Silberberg 09/04/2019 */
/*
 * @title                    Titulo del box   
 * @tObejct                  Objecto del tooltip (opcional) // Funcion createTooltip --> Ver parámetros en la función
 * 
 */
function createTitleBox(param) {

    var title = param.title,
        tObject = param.tooltip,
        html;

    if (title === "" || title === null) {
        console.error("params.title no está definido");
        return false;
    }

    // Html para crear los elementos
    html = "<div class='box-title-container'>";
    html += "<div class='row'>";
    html += "<div class='col-md-8'>";
    html += "<h2 class='form__title'>" + title + "</h2>";
    // va el tooltip
    html += "<div class='box-tooltip'></div>";
    html += "</div>";
    html += "<div class='col-md-4'> <div class='form__cronoliq'> próximo período a liquidar:  <span>" + formatPeriodoMMYYYY($('#cronoLiqVig').val()) + "</span></div></div>";
    //html += "<div class='col-md-4'> <div class='form__cronoliq'> período liquidado sin pagar:  <span>" + formatPeriodoMMYYYY($('#cronoLiqSinPagar').val()) + "</span></div></div>";
    //html += "<div class='col-md-4'> <div class='form__cronoliq'> liquidado puesto en pago:  <span>" + formatPeriodoMMYYYY($('#cronoLiqCursoPag').val()) + "</span></div></div>";
    //html += "<div class='col-md-4'> <div class='form__cronoliq'> liquidado puesto en pago anterior:  <span>" + formatPeriodoMMYYYY($('#cronoLiqCursoPagAnt').val()) + "</span></div></div>";
    html += "</div>";
    html += "</div>";

    // Insertar el Html al inicio del box
    $(".box.form-container:first").prepend(html);

    // Creamos el tooltip
    if (tObject !== "" && tObject !== null && tObject !== undefined)
        createTooltip(tObject);

}


// Funcion para crear el tooltip
/* Creado por Lucas Silberberg 11/04/2019 */
/*
 * @elementId                ID del elemento html 
 * @Title                   Titulo del Tooltip
 * @Message                 [] De mensajes
 * @position                Posición del tooltip
 * @Type                    Tipo de mensaje --> Succes / Error / Warning
 * 
 */
function createTooltip(param) {

    var elementId = param.elementId,
        tTitle = param.title,
        tMessage = param.message,
        tType = param.type,
        tposition = param.position,
        html = "",
        cssStatus = "",
        tElement = param.class,
        tIcon = "",
        classScroll = "";

    // Tipo de mensaje
    switch (tType) {
        // status ok
        case "success":
            cssStatus = "tooltip--success";
            tTitle = "éxito";
            break;
        // status  Warning
        case "warning":
            cssStatus = "tooltip--warning";
            tTitle = "advertencia";
            break;
        // status error
        case "error":
            cssStatus = "tooltip--error";
            tTitle = "error";
            break;
    }

    // posición del tooltip
    switch (tposition) {
        // Top
        case "top":
            tposition = "tooltip--top";
            break;
        // right
        case "right":
            tposition = "tooltip--right";
            break;
        // bottom
        case "bottom":
            tposition = "tooltip--bottom";
            break;
        // left
        case "left":
            tposition = "tooltip--left";
            break;
    }
    //class del tooltip
    switch (tElement) {
        // Top
        case "message":
            tElement = "tooltip__element__message";
            tIcon = "fa fa-envelope";
            break;
        case "triangleWarning":
            tElement = "tooltip__element__message";
            tIcon = "fas fa-exclamation-triangle";
            break;
        default:
            tElement = "tooltip__element";
            tIcon = "tooltip-icon";
            break;
    }

    // Si el titulo no es ingresado se muestra uno pode default
    if (param.title !== undefined && param.title !== "" && param.title !== null)
        tTitle = param.title;

    html += "<div class='tooltip-container'>";
    html += "<div class='" + tElement + "'>";
    html += "<div class='" + tIcon + "' style='position:absolute' ></div>";
    // Posición del tooltip
    html += "<div class='" + tposition + " " + cssStatus + "'>";
    // Titulo del Tooltip
    html += "<h3 class='tooltip__title'>" + tTitle + "</h3>";
    // Listado de texto
    html += "<ul class='list__group' style ='text-transform: none;'  id='scrollStyle'>";
    html += "</ul>";
    html += "<i></i>";

    html += "</div>";
    html += "</div>";
    html += "</div>";

    // Insertar el Html al inicio del box
    $(elementId).prepend(html);

    for (var i = 0; i < tMessage.length; i++) {
        $(elementId + " .list__group").append('<li class="list__item">' + tMessage[i] + '</li>');
    }

}



// Funcion para setear inputs
/* Creado por Lucas Silberberg 22/04/2019 */
var oSetInput = {

    // Valida ingresar solo numeros
    onlyNumber: function (e) {
        var keycodeValue = e.which || e.keyCode;
        return (keycodeValue === 8 || keycodeValue === 0) ? null : keycodeValue >= 48 && keycodeValue <= 57;
    },

    onlyText: function (e) {
        var inputValue = e.which;
        // permite solo letras y espacios en blanco
        if (!(inputValue >= 65 && inputValue <= 120) && (inputValue !== 32 && inputValue !== 0) && !(inputValue === 122 || inputValue === 241 || inputValue === 121)) {
            e.preventDefault();
        }
    }

};

// Funcion para validaciones generales
/* Creado por Lucas Silberberg 22/04/2019 */
var oValidation = {

    // Input
    inputType: function () {
        $("input").keypress(function (event) {

            var inputType = $(this).attr('data-type');
            switch (inputType) {
                case "text":
                    return oSetInput.onlyText(event);
                case "number":
                    return oSetInput.onlyNumber(event);
                default:
                    break;
            }

        });
    },

    blurMaxLength: function (id) {
        var maxlength = $(id).attr("maxlength"),
            length = $(id).inputmask('unmaskedvalue').length;

        var name = $(id).attr("data-name"); // data-name del input
        // Seteo de caraterés máximos removiendo la máscara
        switch (name) {
            case "periodoDesdeHasta":
                maxlength = maxlength - 1;
                break;
            case "cuil":
                maxlength = maxlength;
                break;
            case "datepicker":
                maxlength = maxlength - 3;
                break;
            case "CBU":
                maxlength = maxlength - 1;
                break;

        }

        if (length >= maxlength) {
            //$(id).blur();
            return true;
        }
    },
    //Valida que estén activados los checkbox
    submitCheckbox: function () {

        var btnSubmit = $(".btnCheckbox"),
            esModificacion = $('#esModificacion').val().toString();

        if (esModificacion == false) {
            btnSubmit.attr("disabled", "disabled")
                .attr("data-toggle", "tooltip")
                .attr("title", "Debe seleccionar un relacionado")
                .addClass("button--disabled");

            if (($('input[type = "checkbox"]').is(':checked'))) {

                btnSubmit.removeAttr("disabled").removeClass("button--disabled").removeAttr("title");
            } else {

                btnSubmit.attr("disabled", "disabled")
                    .attr("data-toggle", "tooltip")
                    .attr("title", "Debe seleccionar un relacionado")
                    .addClass("button--disabled");
            }

        }

    },

    //Cbu
    cbu: function (data) {
        return validarCodigoBanco(data.substr(0, 8)) && /*validarBancosConvenio(cbu.substr(0, 3)) &&*/ validarCuenta(data.substr(8, 14));
    },

    // Cuenta
    count: function (data) {

        if (data.length != 14)
            return false;

        var digitoVerificador = data[13],
            suma = data[0] * 3 + data[1] * 9 + data[2] * 7 + data[3] * 1 + data[4] * 3 + data[5] * 9 + data[6] * 7 + data[7] * 1 + data[8] * 3 + data[9] * 9 + data[10] * 7 + data[11] * 1 + data[12] * 3,
            diferencia = (10 - (suma % 10)) % 10;

        return diferencia == digitoVerificador;

    },

    /* Bancos convenios
    BancosConvenio: function (codigo) {

        if (codigo === '015' || codigo === '150' || codigo === '311')
            return false
        else
            return true;
    },*/

    //formatear valor a mostrar de deposito judicial vigente DEPOSITO JUDICIAL Y DECRETO614
    mostrarVigentePorCuilito: function (text) {
        return text.toLowerCase() === "false" ? "No" : "Sí";
    },

    //Valida que en los input que tengan mascara tenga el "_" inlcuido.
    mascara: function (aux) {
        return aux.includes("_");
    },

    checkbox: function () {
        var error = false;

        if ($('form').find('input[type="checkbox"]').length > 0) {

            if ($('input[type = "checkbox"]').is(':checked')) {
                //error = false;
                oMessage.alert.hide({
                    id: ".table-message"
                });
            } else {
                error = true;
                //oMessage.input.show({
                //    id: "#tablaGrupoFamiliar_wrapper",
                //    message: "Seleccione algún cuil de la tabla"
                //});

                oMessage.alert.show({
                    id: ".table-message",
                    messages: ["Seleccione algún cuil de la tabla"],
                    status: "warning"
                });
                oMessage.alert.show({
                    id: ".table-message614",
                    messages: ["Seleccione algún cuil de la tabla"],
                    status: "warning"
                });
            }

        } else {

            oMessage.alert.show({
                id: ".table-message",
                messages: ["No tiene relaciones familiares que seleccionar"],
                status: "warning"
            });

            oMessage.alert.show({
                id: ".table-message614",
                messages: ["No se encuentran Relaciones con posibilidad de cobro de AAFF. De existir un error en la información deberá acreditar sus datos a través de la Atención Virtual - actualización de datos personales y familiares, o bien dirigirse personalmente a una UDAI con la documentación de la cartilla “actualización de vínculos familiares“ "],
                status: "warning",
            });

            error = true;
        }

        return error;
    }

};

// Funcion para formatear fechas
/* Creado por Lucas Silberberg 22/04/2019 */
var oFormatDate = {

    //se formatea la fecha MM/DD/YYYY para poder usarlo como un obj Date.
    strToDate: function (date) {

        if (date !== undefined && date !== null && date !== "") {

            var aux = date.split('/');
            return fecha = aux[1] + "/" + aux[0] + "/" + aux[2];
        }
    },

    // recibe YYYYMM y devuelve MM/YYYY
    periodo: function (date) {
        return date.toString().substr(4, 6) + "/" + date.toString().substr(0, 4);
    },


    date: function (data, format) {

        var pattern = /Date\(([^)]+)\)/,
            results = pattern.exec(data),
            dt = new Date(parseFloat(results[1])),
            ret = (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();

        if (data === null)
            return "-";

        if (format === "" || format === null || format === undefined)
            return console.error("no se ingresó ningun formato");

        switch (format) {
            case "MMYYYY":
                return moment(ret).format("MM/YYYY");

            case "DD/MM/YYYY":
                return moment(ret).format("DD/MM/YYYY");

        }

    }

};


// funciones para crear elementos html.
/* Creado por Lucas Silberberg 26/04/2019 */
var oCreate = {

    // Funcion para crear option buttons 
    // Recibe un array de objetos
    /*
     * @elementId               ID donde insertar los option button
     * @position                Identifica si es un listado para el fitro o para formuarlio ( @filter / @form )
     * @text                    [] texto de los labels
     * @model                   Model al que hace referencia
     * 
     */
    optionButton: function (obj) {
        try {
            var elementId = obj.id,
                position = obj.position,
                text = obj.text,
                model = obj.model,
                html = " <ul class='form-group-option'></ul>";


            // Si no ingresa un id válido no permite continuar ya que es dato obligatorio
            if (elementId === undefined || elementId === "" || elementId === null) {
                console.error("No se ingresó un ID donde ingresar el listado.");
                return;
            } else {
                // Si el elemento existe en el html, se muestra el mensaje
                if (!$(elementId)) {
                    console.error("No éxiste el objeto donde insertar el listado");
                    return;
                } else {
                    $(elementId).html(html);
                }
            }

            if (model === undefined || model === "" || model === null) {
                console.error("No se ingresó un model.");
                return;
            }


            // Listado de mensajes
            for (var i = 0; i < obj.text.length; i++) {
                var listId = text[i].replace(/ /g, "");

                var list = "<label class='form__label' for='" + listId + "'>" + text[i] + "</label>";


                $(elementId + " .form-group-option").append('<li class="form-group__item">' + list + '</li>');
            }

        } catch (e) {
            console.error(e);
        }
    }

};

// funcion para animar el ancla 
/* Creado por Lucas Silberberg 22/04/2019 */
/*
* @id      elemento al cual hace referencia
*/
function animateScroll(id) {
    $('body,html').animate({
        scrollTop: $(id).offset().top
    }, 1000);
}


// Funcion para deshabilitar y habilitar botones
/*
@id             id del botón
@boolean        true deshabilitar botón / false habilitar botón 
*/
function deshabilitaBoton(id, boolean) {

    var flag = true;

    if (flag !== undefined || flag !== null)
        flag = boolean;

    if (!flag) {
        $('#' + id).removeAttr("disabled")
            .removeClass("button--disabled");
    } else {
        $('#' + id).attr("disabled", "disabled")
            .addClass("button--disabled");
    }
}


//función para comparar una fecha ingresada entre dos fechas (una minima y una maxima).
//@param = {"ing" : fecha ingresada, "min": fecha minima permitida, "max": fecha maxima permitida}
//retorna true si la fecha ingresada esta fuera de los parametros
function compararFechas(param) {
    var aux = param.ing.split("/"),
        ing = dateFormat(param.ing),
        max = dateFormat(param.max),
        min = dateFormat(param.min);

    if (param.ing.indexOf("_") == -1) {
        if (!checkDate(param.ing) || new Date(ing) < new Date(min) || new Date(ing) > new Date(max)) {
            return true;
        } else {
            return false;
        }
    } else {
        return true;
    }
}


//Valida el formato y valor correcto de la fecha DD/MM/YYYY
function checkDate(val) {
    var er = /^(((0[1-9]|[12][0-9]|3[01])([-.\/])(0[13578]|10|12)([-.\/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-.\/])(0[469]|11)([-.\/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-.\/])(02)([-.\/])(\d{4}))|((29)(\.|-|\/)(02)([-.\/])([02468][048]00))|((29)([-.\/])(02)([-.\/])([13579][26]00))|((29)([-.\/])(02)([-.\/])([0-9][0-9][0][48]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][2468][048]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][13579][26])))$/;

    if (!er.test(val))
        return false;
    else
        return true;
}


// Setear DatePicker
function globalIniciarDatePickers() {

    deshabilitaBoton("btnSendBaja", true);

    $("#fechaPresentacionBaja").attr("type", "datetime");

    var config = {
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'es',
        todayBtn: false,
        orientation: "bottom right",
        zIndexOffset: 10,
        showOnFocus: false,
        endDate: '+0d',
        //startDate: new Date('2019-07-05') //Fecha implementacion de LUPA
        startDate: '-2y'
    };

    $("#fechaPresentacionBaja").datepicker(config).on('changeDate blur keypress', function (e) {

        validarFechaPresentacionBaja();

    });

}

function validarFechaPresentacionBaja() {
    //se valida ingreso manual de la fecha
    var today = new Date(),
        maxDate = today,
        minYear = maxDate.getFullYear() - 2,
        minDate = new Date(),
        fechaPresentacionAlta = $('#fechaPresentacionAlta').val(),
        param2 = {
            "ing": $("#fechaPresentacionBaja").val(),
            "min": fechaPresentacionAlta,
            "max": maxDate.getDate() + "/" + (maxDate.getMonth() + 1) + "/" + maxDate.getFullYear()
        },

    fechaValida = compararFechas(param2);

    minDate.setFullYear(minYear);

    if (fechaValida) {
        oMessage.input.show({
            id: "#fechaPresentacionBaja",
            message: "fecha presentacion de la baja no válida con respecto al alta del trámite"
        });
    } else {
        oMessage.input.hide({
            id: "#fechaPresentacionBaja"
        });
    }

    if ($("#fechaPresentacionBaja").val() === "") {
        deshabilitaBoton("btnSendBaja", true);

        oMessage.input.show({
            id: "#fechaPresentacionBaja",
            message: "Fecha de presentación no válida"
        });

    } else {
        deshabilitaBoton("btnSendBaja", false);
    }

    return fechaValida;
}


function validarPeriodoDesde(maxYear) {

    if (!maxYear) {
        maxYear = 0;
    }
    var mensajeError = "";
    if (oValidation.blurMaxLength("#periodoDesde")) {
        var inputFechaPresentacion = $('#fechaPresentacion'),
            inputPeriodoDesde = $('#periodoDesde'),
            resultFechaPresentacion = inputFechaPresentacion.val().split('/');

        maxYear = parseInt(resultFechaPresentacion[2]) + maxYear;

        params = {
            "ing": "01/" + inputPeriodoDesde.val(),
            "min": "01/" + resultFechaPresentacion[1] + "/" + (resultFechaPresentacion[2] - 2),
            "max": "01/" + resultFechaPresentacion[1] + "/" + maxYear
        },

            error = compararFechas(params);
        mensajeError = "Periodo incorrecto";
    } else {
        error = true;
        mensajeError = "Periodo incompleto";

    }

    if (error) {
        oMessage.input.show({
            id: "#periodoDesde",
            message: mensajeError
        });
    } else {
        oMessage.input.hide({
            id: "#periodoDesde"
        });
    }

    return error;
}

function validarPeriodoHasta(idPeriodoHasta) {

    if (!idPeriodoHasta) {
        idPeriodoHasta = "#periodoHasta";
    }

    var mensajeError = "";

    if (oValidation.blurMaxLength(idPeriodoHasta)) {

        var inputPeriodoHasta = $(idPeriodoHasta),
            inputPeriodoDesde = $('#periodoDesde'),
            params = {
                "ing": "01/" + inputPeriodoHasta.val(),
                "min": "01/" + inputPeriodoDesde.val(),
                "max": "01/" + inputPeriodoHasta.val()
            },
            error = compararFechas(params);
        mensajeError = "Periodo incorrecto";

    } else if ($(idPeriodoHasta).val() !== "") {

        error = true;
        mensajeError = "Periodo incompleto";

    } else {

        error = false;
    }

    if (error) {
        oMessage.input.show({
            id: idPeriodoHasta,
            message: mensajeError
        });
    }
    else {
        oMessage.input.hide({
            id: idPeriodoHasta
        });
    }

    return error;
}

//Validar que en el formulario no contenga algun mensaje de error.
function validarFormulario() {
    return $('form').find('span[data-name="messageError"]').length > 0;
}

// Función para setear los shortcuts
/* Creado por Lucas Silberberg 30/05/2019 */
function setShortCut(flag) {

    $(document).keydown(function (e) {

        if (flag === undefined) {
            switch (e.which) {

                // F2: Volver al panel principal
                case 113:
                    window.location.href = "../../Dashboard/Index";
                    e.preventDefault();
                    break;

                // F3: Volver un paso atrás
                case 114:
                    window.history.back();
                    e.preventDefault();
                    break;

                // F4: Borrar formuarlio
                case 115:
                    $(".button--clean-form").trigger("click");
                    e.preventDefault();
                    break;

                //// F5: Borrar formuarlio
                //case 116:
                //    e.preventDefault();
                //    break;
            }
        }

        if (flag) {
            switch (e.which) {
                // F3: Volver un paso atrás
                case 114:
                    window.location.href = "../../Dashboard/Index";
                    e.preventDefault();
                    break;
            }

        }

    });
}

function hideTable() {

    try {

        $(".table-container").closest(".row").addClass("hidden");

    } catch (e) {
        console.error(e);
    }

}

// Crear botones
function CreateButton() {
    try {
        $(".button-container ").remove();

        $("lupa-button-container").each(function (index) {

            var data = $(this).attr("data-button");
            var button = "";
            var type = "";

            console.log(data);
            if (data !== "" && data !== undefined) {
                type = data;
            } else {
                console.error("no ingreso ninguna clase");
                return;
            }

            switch (type) {
                case 'continue':
                    button = '<button type="submit" value="Continuar" id="btnSend" class="button--blue">Continuar</button>';
                    break;

                case 'save':
                    button = '<button type="submit" value="Continuar" id="btnSend" class="button--blue">aceptar</button>';
                    break;
            }

            // Se crea la estructura del botón
            var url = "@Url.Action('Index', 'Dashboard', new { area= '', cuil = @Model.cuil})";
            var html = '<!-- Botones -->';
            html += '<div class="button-container text-align--right">';
            html += '   <div class="button--clean-form"></div>';
            html += '   <div class="button--text-red" onclick="location.href=""' + url + '" >Cancelar formulario</div>';
            html += button;
            html += ' </div>';

            $(this).replaceWith(html);
        });
    } catch (e) {
        console.error(e);
    }

}

//// Modificar Kendo
function newKendo() {
    $(".k-selectable").addClass("table");
    $(".k-selectable").find("tbody").addClass("table__body").removeAttr("style");
    $(".k-selectable").find("tr").addClass("table__row").removeAttr("style");
    $(".k-selectable").find("td").addClass("table__column").removeAttr("style");
}

// Agrega la etiqueta opcional
function fieldOptional() {

    $(".label--optional").each(function () {

        var text = $(this).text();
        var html = "<small> campo opcional</small>";

        $(this).html(text + " - " + html);

    });

}

// Funcion global para el spinner en el ajax
function ajaxLoading() {
    var flag = true;
    var ajaxLoadingTimeout;

    $(document).ajaxStart(function () {

        flag = true;
        ajaxLoadingTimeout = setTimeout(function () {
            if (flag) {
                oSpinner.show();
            }
        }, 100);

    });

    $(document).ajaxStop(function () {
        flag = false;
        clearTimeout(ajaxLoadingTimeout);
        oSpinner.hide();

    });

    $(document).ajaxComplete(function () {
        flag = false;
        clearTimeout(ajaxLoadingTimeout);
        oSpinner.hide();

    });
}

// Setear Inputs
function setInput() {

    // Formato del datepicker
    $('input[type="datetime"]')
        .datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            language: 'es',
            todayBtn: false,
            orientation: "bottom right",
            todayHighlight: false,
            zIndexOffset: 10,
            showOnFocus: false
        })
        .on('click', function (e) {
            $(this).datepicker('show');
        })
        .attr({
            "placeholder": "DD/MM/AAAA",
            "data-name": "datepicker",
            "maxlength": 10,
            "data-required": true
        })
        .inputmask({ "mask": "99/99/9999" });

    // Formato del periodo desde y periodo hasta
    $('input[name="periodoDesdeJud"], input[name="periodoHastaJud"],input[name="periodoDesde"], input[name="periodoHasta"], input[name = "periodoHastaR"]')
        .attr({
            "placeholder": "MM/AAAA",
            "data-name": "periodoDesdeHasta",
            "maxlength": 7
        })
        .inputmask({ "mask": "99/9999" });

    // Formato para Cbu
    $("input[name='CBU']")
        .attr({
            "placeholder": "Ingrese el cbu sin guiones (-)",
            "maxlength": 23,
            "data-type": "number",
            "data-name": "CBU"
        })
        .inputmask({ "mask": "99999999-99999999999999" });

    // Removemos el autocomplete de los inputs type text
    $('input').attr({ "autocomplete": "off" });

    //Formato para cuil
    $('input[data-name="cuil"]')
        .attr({
            "placeholder": "Ingrese el cuil sin guiones (-)",
            "maxlength": 11,
            "data-type": "number"
        });
    //.inputmask({ "mask": "99-99999999-99" }); 
    $("button").attr({
        "data-type": "submit"
    });

    $(".modal").attr({
        "data-keyboard": false,
        "data-backdrop": 'static'
    })
}


// Hace focus al siguiente input luego de completar los caracteres
function focusNextInput() {
    try {
        $("input").keyup(function (e) {

            // Se captura el Tab, si es tab no deberia saltar del input y deberia dejar corregir los datos
            if (e.keyCode == 9 || e.keyCode == 16) {
                return false;
            }
            var name = $(this).attr("data-name"), // data-name del input
                maxNumber = parseInt($(this).attr("maxlength")); // Cantidad máxima de caractéres

            // Seteo de caraterés máximos removiendo la máscara
            switch (name) {
                case "periodoDesdeHasta":
                    maxNumber = maxNumber - 1;
                    break;
                case "cuil":
                    maxNumber = maxNumber;
                    break;
                case "datepicker":
                    maxNumber = maxNumber - 2;
                    break;
                case "CBU":
                    maxNumber = maxNumber - 1;
                    break;

            }


            // Se valida luego de superar la cantidad de caractéres permitidos 
            if ($(this).inputmask('unmaskedvalue').length === maxNumber) {

                // VER !! Si el datepicker se esconde borra el valor del input
                /* var value = $(this).val();
                 $(this).datepicker('hide');
                 $(this).val(value);
                */
                var nextInput = $(this).closest(".col-md-6").next().find("input"),
                    lastInput = $(this).closest(".col-md-6"),
                    lastRow = $(this).closest(".row"),
                    button = $("button.button--blue");


                $(this).blur();
                // Si el input está bloqueado o es el último pasa a la siguiente fila y hace foco en el primer input
                if (nextInput.attr("disabled") === "disabled" || lastInput.is(':last-child')) {

                    var firstInput = $(this).closest(".row").next().find("input:first");

                    // Si es la última fila / último input del formualario hace foco en el botón para enviar o continuar
                    if (lastRow.is(':last-child')) {

                        // si es el último input del formulario, hace foco en los botones para continuar / enviar 
                        if (!button.hasClass("button--disabled"))
                            button.focus();

                    } else {

                        firstInput.focus();

                        // Si el input está oculto le hace focus al siguiente ( componente razor )
                        if (firstInput.attr("type") === "hidden") {
                            firstInput.next().focus();
                        }
                    }

                } else {
                    nextInput.focus();
                }
            }

        });
    } catch (e) {
        console.error(e);
    }
}


// Funcion para clickear una fila y selecionar el input chekbox o radio
function clickRowTable() {

    try {
        $(document).on("click", ".table__row", function () {

            var input = $(this).find(".table__column input");

            // Si el input existe
            if (input) {
                // ¿ Está chequeado ?
                if (!input.is(':checked')) {
                    input.attr("checked", true);
                } else {
                    input.attr("checked", false);
                }

            }
            // Validar el botón submit si hay algun elemento chequeado
            validateSubmit();
        });
    } catch (e) {
        console.error(e);
    }

}

function setFechaPresentacion() {
    var config = {
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'es',
        todayBtn: false,
        orientation: "bottom right",
        todayHighlight: false,
        zIndexOffset: 10,
        showOnFocus: false,
        endDate: '+0d',
        //startDate: '-2y'
    };

    $("#fechaPresentacion").datepicker(config);

}

function validarFechaPresentacion() {

    //se valida ingreso manual de la fecha
    var today = new Date();
    var maxDate = today;
    var minYear = maxDate.getFullYear() - 2;
    var minDate = new Date();
    minDate.setFullYear(minYear);

    var param = {
        "ing": $("#fechaPresentacion").val(),
        "min": minDate.getDate() + "/" + (minDate.getMonth() + 1) + "/" + minDate.getFullYear(),
        "max": maxDate.getDate() + "/" + (maxDate.getMonth() + 1) + "/" + maxDate.getFullYear()
    };

    var fechaValida = compararFechas(param);

    if (fechaValida) {
        oMessage.input.show({
            id: "#fechaPresentacion",
            message: "fecha presentación no válida"
        });
    } else {
        oMessage.input.hide({
            id: "#fechaPresentacion"
        });
    }

    return fechaValida;
}

//Funcion para validar que este al menos un checkbox seleccionado o dibujado.
//Aplica para algunos modulos (Decreto 614, Renuncia al cobro, Deposito Judicial)
function validateCheckbox() {
    var error = false;

    if ($('form').find('input[type="checkbox"]').length > 0) {

        if ($('input[type = "checkbox"]').is(':checked')) {
            //error = false;
            oMessage.input.hide({
                id: ".table-message"
            });
        } else {
            error = true;
            oMessage.input.show({
                id: ".table-message",
                message: "Seleccione algún cuil de la tabla"
            });

        }

    } else {

        oMessage.alert.show({
            id: ".table-message",
            messages: ["No tiene relaciones familiares que seleccionar"],
            status: "warning"
        });

        error = true;
    }

    return error;
}

/********* FUNCIONES VIEJAS CHEQUEAR Y BORRAR *********/

function delayTooltip() {
    $(".box-title-container .tooltip--top.tooltip--warning").addClass("is-visible");

    setTimeout(function () { $(".box-title-container .tooltip--top.tooltip--warning").removeClass("is-visible") }, 3000);
}

function delayTooltipDashboard() {
    $(".mensajes-tooltip .tooltip--top.tooltip--warning").addClass("is-visible");

    setTimeout(function () { $(".mensajes-tooltip .tooltip--top.tooltip--warning").removeClass("is-visible") }, 3000);
}

// Funcion para dar formato de la fecha
// Recibe fecha en DDMMYYYY y retorna Formato MMDDYYYY
function dateFormat(date) {

    if (date !== undefined && date !== null && date !== "") {

        var aux = date.split('/');
        return fecha = aux[1] + "/" + aux[0] + "/" + aux[2]; //se formatea la fecha MM/DD/YYYY para poder usarlo como un obj Date.
    }

}

function formatPeriodoMMYYYY(aux) {
    var periodo = "";
    if (aux !== "0") {
        periodo = aux.toString().substr(4, 6) + "/" + aux.toString().substr(0, 4);
    }
    return periodo;
}

function formatDateMMYYYY(data) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(data);
    var dt = new Date(parseFloat(results[1]));
    var ret = (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();

    return moment(ret).format("MM/YYYY");
}

function formatDateDDMMYYYY(data) {
    if (data === null) {
        return "-";
    } else {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(data);
        var dt = new Date(parseFloat(results[1]));
        var ret = (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();

        return moment(ret).format("DD/MM/YYYY");
    }

}

//Conversión de un DATE de C# para mostrar fecha, horas y minutos.
function formatDateDDMMYYYYHHMM(data) {
    if (data === null || data === "") {
        return "-";
    } else {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(data);
        var dt = new Date(parseFloat(results[1]));
        var ret = (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes();

        return moment(ret).format("DD/MM/YYYY kk:mm");
    }
}

/* Validaciones de cbu*/
function validarCBUlocal(cbu) {

    var cbuNum1 = cbu.substr(0, 8),
        cbuNum2 = cbu.substr(8, 14);

    if (cbu === "") {
        $("#CBU").focus();

        oMessage.input.show({
            id: "#CBU",
            message: "Ingrese CBU"
        });
        return false;

    }
    else if (cbu === "0000000000000000000000" || cbu === "5555555555555555555555") {
        oMessage.input.show({
            id: "#CBU",
            message: "CBU inválido"
        });
        return false;
    }

    if (!validarCodigoBanco(cbuNum1)) {
        $("#CBU").focus();

        oMessage.input.show({
            id: "#CBU",
            message: "CBU de banco no válido"
        });

        return false;
    }

    if (!validarCuenta(cbuNum2)) {
        $("#CBU").focus();

        oMessage.input.show({
            id: "#CBU",
            message: "CBU de cuenta no válida"
        });
        return false;
    }

    if (cbu.substr(0, 1) > "5") {
        $("#CBU").focus();

        oMessage.input.show({
            id: "#CBU",
            message: "CBU de tipo de moneda no válido"
        });
        return false;
    }

    /*if (!validarBancosConvenio(cbu.substr(0, 3))) {
        //$("#CBU").focus();

        oMessage.input.show({
            id: "#CBU",
            message: "Banco fuera de convenio"
        });
        return false;
    }*/

    return true;
}

/* Validaciones de cbu deposito judicial*/
function validarCBUDeposito(cbu, modoPago) {

    var cbuNum1 = cbu.substr(0, 8),
        cbuNum2 = cbu.substr(8, 14);

    if (modoPago != 3) {

        if (cbu === "") {
            $("#CBU").focus();

            oMessage.input.show({
                id: "#CBU",
                message: "Ingrese CBU"
            });
            return false;
        } else if (cbu === "0000000000000000000000" || cbu === "5555555555555555555555") {
            oMessage.input.show({
                id: "#CBU",
                message: "CBU inválido"
            });
            return false;
        }

        if (!validarCodigoBanco(cbuNum1)) {
            $("#CBU").focus();

            oMessage.input.show({
                id: "#CBU",
                message: "CBU de banco no válido"
            });

            return false;
        }

        if (!validarCuenta(cbuNum2)) {
            $("#CBU").focus();

            oMessage.input.show({
                id: "#CBU",
                message: "CBU de cuenta no válida"
            });
            return false;
        }

        if (cbu.substr(0, 1) > "5") {
            $("#CBU").focus();

            oMessage.input.show({
                id: "#CBU",
                message: "CBU de tipo de moneda no válido"
            });
            return false;
        }
    }
    return true;
}

function validarCuenta(cuenta) {

    if (cuenta.length !== 14) { return false; }
    var digitoVerificador = cuenta[13];
    var suma = cuenta[0] * 3 + cuenta[1] * 9 + cuenta[2] * 7 + cuenta[3] * 1 + cuenta[4] * 3 + cuenta[5] * 9 + cuenta[6] * 7 + cuenta[7] * 1 + cuenta[8] * 3 + cuenta[9] * 9 + cuenta[10] * 7 + cuenta[11] * 1 + cuenta[12] * 3;
    var diferencia = (10 - (suma % 10)) % 10;
    return diferencia.toString() === digitoVerificador;
}

/*function validarBancosConvenio(codigo) {

    if (codigo === '015' || codigo === '150'  || codigo === '311')
        return false;
    else
        return true;
}*/

function validarCodigoBanco(codigo) {

    if (codigo.length != 8) { return false }
    var banco = codigo.substr(0, 3);
    var digitoVerificador1 = codigo[3];
    var sucursal = codigo.substr(4, 3);
    var digitoVerificador2 = codigo[7];

    var suma = banco[0] * 7 + banco[1] * 1 + banco[2] * 3 + digitoVerificador1 * 9 + sucursal[0] * 7 + sucursal[1] * 1 + sucursal[2] * 3;

    var diferencia = (10 - (suma % 10)) % 10;

    return diferencia == digitoVerificador2;
}

function validarCuil(cuil) {

    cuil = cuil.toString().replace(/[-_]/g, "");

    if (cuil.length != 11)
        return false;
    else {
        var mult = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
        var total = 0;
        for (var i = 0; i < mult.length; i++) {
            total += parseInt(cuil[i]) * mult[i];
        }
        var mod = total % 11;
        var digito = mod == 0 ? 0 : mod == 1 ? 9 : 11 - mod;

    }
    return digito == parseInt(cuil[10]);
}

//formatear valor a mostrar de deposito judicial vigente DEPOSITO JUDICIAL Y DECRETO614
function mostrarVigentePorCuilito(text) {
    return text ? "Sí" : "No";
}

//formatear valor a mostrar de deposito judicial vigente DEPOSITO JUDICIAL Y DECRETO614
function mostrarDecretoArgentaVigentePorCuilito(textDecre, textArg) {
    return textDecre || textArg ? "Sí" : "No";
}

//Valida que en los input que tengan mascara tenga el "_" inlcuido.
function validarMascara(aux) {
    return aux.includes("_");
}

//Valida que estén activados los checkbox
function validateSubmit() {

    var btnSubmit = $(".btnCheckbox");
    var esModificacion = (typeof ($('#esModificacion').val()) == "undefined") ? false : $('#esModificacion').val().toString();

    if (esModificacion == false) {
        btnSubmit.attr("disabled", "disabled")
            .attr("data-toggle", "tooltip")
            .attr("title", "Debe seleccionar un relacionado")
            .addClass("button--disabled");

        if (($('input[type = "checkbox"]').is(':checked'))) {

            btnSubmit.removeAttr("disabled").removeClass("button--disabled").removeAttr("title");

        } else {

            btnSubmit.attr("disabled", "disabled")
                .attr("data-toggle", "tooltip")
                .attr("title", "Debe seleccionar un relacionado")
                .addClass("button--disabled");
        }

    }

}

function myfunction(e) {
    console.log(e.keyCode);
    if (e.keyCode === 13) {
        e.stopPropagation();
        e.preventDefault();

        return false;
    }
}

//Agregar ceros a la izquierda
function leftFillNum(num, width) {
    return num
        .toString()
        .padStart(width, 0);
}

function Padder(len, pad) {
    if (len === undefined) {
        len = 1;
    } else if (pad === undefined) {
        pad = '0';
    }

    var pads = '';
    while (pads.length < len) {
        pads += pad;
    }

    this.pad = function (what) {
        var s = what.toString();
        return pads.substring(0, pads.length - s.length) + s;
    };
}

function cleanCheckbox() {
    $("form").find("input[type='checkbox']").prop('checked', false);
}

/********* FIN DE FUNCIONES VIEJAS *********/


$(document).ready(function () {

    $('.notAuthorized').remove();

    $('#btnSendBaja').click(function (event) {

        event.preventDefault();

        if (!validarFormulario() && !validarFechaPresentacionBaja()) {
            $('form').submit();
        }

    });

    //borrar mensaje de error y fecha al cancelar el modal de baja.
    $('div[data-dismiss="modal"]').click(function (event) {
        oMessage.input.hide({
            id: "#fechaPresentacionBaja"
        });

        $('#fechaPresentacionBaja').val("");

    });

    //setear fecha de presentacion
    //setFechaPresentacion();

    //validar fecha de presentacion
    //$("#fechaPresentacion").datepicker().on('changeDate blur', function (e) {

    //    if (oValidation.blurMaxLength("#fechaPresentacion")) {
    //        validarFechaPresentacion();
    //    }

    //});

    setShortCut();

    //clickRowTable();

    // Función para hacer focus al siguiente input
    //focusNextInput();

    // Función loading spinner
    ajaxLoading();

    // Setear las etiquetas opcionales;
    fieldOptional();

    // Setear datepicker
    //globalIniciarDatePickers();

    setInput();

    oValidation.inputType();

    //invocar modal
    $(document).on("click", ".show-modal", function () {
        $("#lupa-modal").modal('show');
        $("#lupa-modal").modal({
            backdrop: 'static',
            keyboard: false
        });
        document.body.style.overflow = 'hidden';
    });
    $(document).on("click", "div[data-dismiss='modal']", function () {
        document.body.style.overflow = 'auto';
    });

    // Modificar kendo
    newKendo();

    //Foco en el primer input de un form
    if (!$("form").hasClass("form--no-focus")) {
        $("form :input:enabled:visible:first").focus();
    }

    $(".button--clean-form").on("click", function () {

        //Obtengo todos los inputs separado por tipos del form padre
        var formularioInputText = $(this).parents("form:first").find("input[type='text']");
        var formularioInputRadio = $(this).parents("form:first").find("input[type='radio']");
        var formularioInputCheck = $(this).parents("form:first").find("input[type='checkbox']");
        var formularioInputDatetime = $(this).parents("form:first").find("input[type='datetime']");
        var formularioSelect = $(this).parents("form:first").find('select');
        var formularioTable = $(this).parents("form:first").find('table');
        var formularioHidden = $(this).parents("form").find('div[class^="step-"]');
        var searchResult = $(this).parents("form").find('.search-result'); //box de busqueda de cuiles relacionados.

        //Limpio los inputText y deselecciono los radio y checkbox

        $("input[type='text'], input[type='datetime']").each(function () {
            if (!$(this).prop('disabled') && !$(this).prop('readonly'))
                $(this).val("");
        });

        //formularioTable.dataTable().fnDestroy();
        formularioSelect.find('option:eq(0)').prop('selected', true);
        formularioInputRadio.prop('checked', false);
        formularioInputCheck.prop('checked', false);


        if (searchResult.length > 0) {
            $(".search-result").addClass("hidden");
        }

        if (!$("form").hasClass("form--no-focus")) {
            $("form :input:enabled:visible:first").focus();
        }

        validateSubmit();

    });

    $(".back-button, .back-history").on("click", function () {
        history.go(-1);
    });

    //Configuración del datatable global
    $.extend(true, $.fn.dataTable.defaults, {
        "paging": true,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        "autoFill": true,
        //scrollY: '224px',
        //scrollCollapse: false,
        "pageLength": 6,
        "processing": true, // for show progress bar
        "serverSide": false, // for process server side
        "filter": false, // this is for disable filter (search box)
        "createdRow": function (row, data, index) {
            $('td', row).addClass('table__column');
            $(row).addClass('table__row');
        },
        "language": {
            "lengthMenu": "Mostrando _MENU_ registros por página",
            "zeroRecords": "No se encontraron registros",
            "info": "Mostrando pagina _PAGE_ de _PAGES_ ",
            "infoEmpty": "No hay registros disponibles",
            "emptyTable": "No hay datos",
            "infoFiltered": "(filtered from _MAX_ total records)",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "infoPostFix": "",
            "thousands": ".",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Filtro:",
            "aria": {
                "sortAscending": ": Ordenar columna ascendente",
                "sortDescending": ": Ordenar columna descendente"
            }
        }
    });

    $(window).keydown(function (event) {
        if (event.keyCode === 13) {
            if ($('button[data-type="submit"]').hasClass("button--disabled")) {
                event.preventDefault();
                return false;
            }
        }
    });

    //Solución al issue del modal bootstrap con atributo fade 
    //https://stackoverflow.com/questions/32862394/bootstrap-modals-keep-adding-padding-right-to-body-after-closed
    $(document.body).on('hide.bs.modal,hidden.bs.modal', function () {
        $('body').css('padding-right', '0');
    });

    //Para grilla en IE9
    //https://stackoverflow.com/questions/51719553/padstart-not-working-in-ie11
    if (!String.prototype.padStart) {
        String.prototype.padStart = function padStart(targetLength, padString) {
            targetLength = targetLength >> 0; //truncate if number or convert non-number to 0;
            padString = String((typeof padString !== 'undefined' ? padString : ' '));
            if (this.length > targetLength) {
                return String(this);
            }
            else {
                targetLength = targetLength - this.length;
                if (targetLength > padString.length) {
                    padString += padString.repeat(targetLength / padString.length); //append to original to ensure we are longer than needed
                }
                return padString.slice(0, targetLength) + String(this);
            }
        };
    }

    //Para grilla en IE9
    if (!String.prototype.repeat) {
        String.prototype.repeat = function (count) {
            'use strict';
            if (this == null) {
                throw new TypeError('can\'t convert ' + this + ' to object');
            }
            var str = '' + this;
            count = +count;
            if (count != count) {
                count = 0;
            }
            if (count < 0) {
                throw new RangeError('repeat count must be non-negative');
            }
            if (count == Infinity) {
                throw new RangeError('repeat count must be less than infinity');
            }
            count = Math.floor(count);
            if (str.length == 0 || count == 0) {
                return '';
            }
            // Ensuring count is a 31-bit integer allows us to heavily optimize the
            // main part. But anyway, most current (August 2014) browsers can't handle
            // strings 1 << 28 chars or longer, so:
            if (str.length * count >= 1 << 28) {
                throw new RangeError('repeat count must not overflow maximum string size');
            }
            var maxCount = str.length * count;
            count = Math.floor(Math.log(count) / Math.log(2));
            while (count) {
                str += str;
                count--;
            }
            str += str.substring(0, maxCount - str.length);
            return str;
        }
    }
});

