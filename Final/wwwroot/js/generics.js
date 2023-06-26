
function limpiarCampos() {
  let inputs = document.querySelectorAll('input');
  inputs.forEach(input => {
      input.value = '';
  });
}

function convertirAMayusculas() {
    var camposDeTexto = document.querySelectorAll('input[type="text"]');

    for (var i = 0; i < camposDeTexto.length; i++) {
        camposDeTexto[i].value = camposDeTexto[i].value.toUpperCase();
    }
}

