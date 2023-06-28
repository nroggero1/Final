//Funcion para calcular precio de venta sugerido
function calcularPrecioVentaSugerido() {
    var precioCompra = parseFloat(document.getElementById("precioCompra").value);
    var porcentajeGanancia = parseFloat(document.getElementById("porcentajeGanancia").value);

    var precioVentaSugerido = precioCompra + (precioCompra * (porcentajeGanancia / 100));

    // Mostrar el resultado en el campo de texto de precio de venta sugerido
    document.getElementById("precioVentaSugerido").value = precioVentaSugerido.toFixed(2);
}

