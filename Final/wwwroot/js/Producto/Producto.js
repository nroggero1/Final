
function calcularPrecioVentaSugerido() {
    var precioCompra = parseFloat(document.getElementById('PrecioCompra').value);
    var porcentajeGanancia = parseFloat(document.getElementById('PorcentajeGanancia').value);

    if (!isNaN(precioCompra) && !isNaN(porcentajeGanancia)) {
        var precioVentaSugerido = precioCompra * (porcentajeGanancia / 100) + precioCompra;
        document.getElementById('PrecioVentaSugerido').value = precioVentaSugerido.toFixed(2);
    }
}

