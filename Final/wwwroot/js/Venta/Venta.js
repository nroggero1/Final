function agregarProducto() {
    // Obtener los valores seleccionados
    var idProducto = document.getElementById("producto").value;
    var precioUnitario = obtenerPrecioUnitario(idProducto);
    var cantidad = parseInt(document.getElementById("cantidad").value);

    // Calcular el total parcial
    var totalParcial = precioUnitario * cantidad;

    // Crear la fila con los datos seleccionados
    var fila = "<tr>" +
        "<td>" + idProducto + "</td>" +
        "<td>" + precioUnitario + "</td>" +
        "<td>" + cantidad + "</td>" +
        "<td>" + totalParcial + "</td>" +
        "</tr>";

    // Agregar la fila a la tabla
    var tablaProductos = document.getElementById("tablaProductos");
    var tbody = tablaProductos.getElementsByTagName("tbody")[0];
    tbody.innerHTML += fila;

    // Limpiar campos
    producto.value = "";
    cantidad.value = "";
}

