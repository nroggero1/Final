//Funcion para calcular precio de venta sugerido
function calcularPrecioVentaSugerido() {
    var precioCompra = parseFloat(document.getElementById('precioCompra').value);
    var porcentajeGanancia = parseFloat(document.getElementById('porcentajeGanancia').value);

    if (!isNaN(precioCompra) && !isNaN(porcentajeGanancia)) {
        var precioVentaSugerido = precioCompra * (porcentajeGanancia / 100) + precioCompra;
        document.getElementById('PrecioVentaSugerido').value = precioVentaSugerido.toFixed(2);
    }
}