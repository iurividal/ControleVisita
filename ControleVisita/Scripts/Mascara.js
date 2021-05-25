var options = {
    onKeyPress: function (cpf, ev, el, op) {
        var masks = ['000.000.000-000', '00.000.000/0000-00'];
        $('.cpf').mask((cpf.length > 14) ? masks[1] : masks[0], op);
    }
}



$('.cpf').length > 11 ? $('.cpf').mask('00.000.000/0000-00', options) : $('.cpf').mask('000.000.000-00#', options);