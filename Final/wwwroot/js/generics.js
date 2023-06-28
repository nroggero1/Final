
function limpiarCampos() {
  let inputs = document.querySelectorAll('input');
  inputs.forEach(input => {
      input.value = '';
  });
}

function validarCampos() {
    var inputs = document.getElementsByTagName("input");

    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].value === "") {
            alert("Por favor, complete todos los campos.");
            return false;
        }
    }

    return true;
}

function convertirAMayusculas() {
    var camposDeTexto = document.querySelectorAll('input[type="text"]');

    for (var i = 0; i < camposDeTexto.length; i++) {
        camposDeTexto[i].value = camposDeTexto[i].value.toUpperCase();
    }
}