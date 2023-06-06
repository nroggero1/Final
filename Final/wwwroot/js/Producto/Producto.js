
function calcularPrecioVentaSugerido() {
    var precioCompra = parseFloat(document.getElementById('PrecioCompra').value);
    var porcentajeGanancia = parseFloat(document.getElementById('PorcentajeGanancia').value);

    if (!isNaN(precioCompra) && !isNaN(porcentajeGanancia)) {
        var precioVentaSugerido = precioCompra * (porcentajeGanancia / 100) + precioCompra;
        document.getElementById('PrecioVentaSugerido').value = precioVentaSugerido.toFixed(2);
    }
}

function buscarProducto() {
    var codigoBarras = document.getElementById("codigoBarras").value;
    // Realizar una petición al servidor para buscar el producto por el código de barras
    // y completar los campos correspondientes, como Nombre del Producto y Precio de Compra.
}