function validarCampos() {
    // Validar campo "nombre"
    var nombre = document.getElementById("nombre").value;
    if (nombre.trim() === "" || nombre.length > 50) {
        alert("El campo Nombre es obligatorio y debe tener un máximo de 50 caracteres.");
        return false;
    }

    return true;
}