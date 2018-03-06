/*
* Script principal para CyD
*/

$(document).ready(function () {
	Plugins();

    $("body").on('click', 'button', function () {

        // Si el boton no tiene el atributo ajax no hacemos nada
        if ($(this).data('ajax') == undefined) return;

        // El metodo .data identifica la entrada y la castea al valor más correcto
        if ($(this).data('ajax') != true) return;

        var form = $(this).closest("form");
        var buttons = $("button", form);
        var button = $(this);
        var url = form.attr('action');

        if (button.data('confirm') != undefined)
        {
            if (button.data('confirm') == '') {
                if (!confirm('¿Esta seguro de realizar esta acción?')) return false;
            } else {
                if (!confirm(button.data('confirm'))) return false;
            }
        }

        if (!form.valid()) {
            return false;
        }

        // Creamos un div que bloqueara todo el formulario
        var block = $('<div class="block-loading" />');
        form.prepend(block);

        // En caso de que haya habido un mensaje de alerta
        $(".alert", form).remove();

        form.ajaxSubmit({
            dataType: 'JSON',
            type: 'POST',
            url: url,
            success: function (r) {
                block.remove();
                if (r.response) {
                    if (!button.data('reset') != undefined) {
                        if (button.data('reset')) form.reset();
                    }
                }

                // Mostrar mensaje
                if (r.message != null) {
                    if (r.message.length > 0) {
                        var css = "";
                        if (r.response) css = "alert-success";
                        else css = "alert-danger";

                        var message = '<div class="alert ' + css + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + r.message + '</div>';
                        form.prepend(message);
                    }
                }

                // Ejecutar funciones
                if (r.function != null) {
                    setTimeout(r.function, 0);
                }
                // Redireccionar
                if (r.href != null) {
                    if (r.href == 'self') window.location.reload(true);
                    else redirect(r.href);
                }
            }
        });

        return false;
    })
})

function Plugins() {
    // Verificamos si el plugin existe
    if (jQuery().datepicker)
    {
        $(".datepicker").datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            language: "es"
        }).attr('readonly', true);
        $(".birthday").datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            language: "es",
            viewMode: 2
        }).attr('readonly', true);
    }
}

function AjaxPopupModal(id, title, url, params)
{
	$("#" + id).remove();
    $("body").append('<div data-backdrop="static" id="' + id + '" class="modal fade"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title">' + title + '</h4></div><div class="modal-body"></div></div></div></div>');
    $("#" + id).modal();

    // Cargando
    $("#" + id).find('.modal-body').html('<blink>Estamos cagando el formulario ..</blink>');
    $.post(base_url(url),params, function(r){
    	$("#" + id).find('.modal-body').html(r);
    });
}

jQuery.fn.reset = function () {
    $("input:password,input:file,input:text,textarea", $(this)).val('');
    $("input:checkbox:checked", $(this)).click();
    $("select", $(this)).val(0);
};