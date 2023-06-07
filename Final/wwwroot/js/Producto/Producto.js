//Funcion para calcular precio de venta sugerido
function calcularPrecioVentaSugerido() {
    var precioCompra = parseFloat(document.getElementById('precioCompra').value);
    var porcentajeGanancia = parseFloat(document.getElementById('porcentajeGanancia').value);

    if (!isNaN(precioCompra) && !isNaN(porcentajeGanancia)) {
        var precioVentaSugerido = precioCompra * (porcentajeGanancia / 100) + precioCompra;
        document.getElementById('PrecioVentaSugerido').value = precioVentaSugerido.toFixed(2);
    }
}

//Funcion para filtrar los productos en los viewbag
function filtrarProductos() {
    var marcaSeleccionada = document.getElementById("marca").value;
    var categoriaSeleccionada = document.getElementById("categoria").value;
    var productos = document.getElementById("producto").options;

    // Mostrar todos los productos al seleccionar "Seleccione la marca" o "Seleccione la categoria"
    if (marcaSeleccionada === "" || categoriaSeleccionada === "") {
        for (var i = 0; i < productos.length; i++) {
            productos[i].style.display = "";
        }
        return;
    }

    // Filtrar los productos según la marca y la categoria seleccionadas
    for (var i = 0; i < productos.length; i++) {
        var producto = productos[i];
        var marcaProducto = producto.getAttribute("data-marca");
        var categoriaProducto = producto.getAttribute("data-categoria");

        if (marcaProducto === marcaSeleccionada && categoriaProducto === categoriaSeleccionada) {
            producto.style.display = "";
        } else {
            producto.style.display = "none";
        }
    }
}


//Funcion para agregar un producto a la lista
var productos = [];
function agregarProducto() {
    var producto = {
        Id: document.getElementById('producto').value,
        Nombre: document.getElementById('producto').selectedOptions[0].text,
        Precio: document.getElementById('precioCompra').value,
        Cantidad: document.getElementById('cantidad').value
    };

    productos.push(producto);
    actualizarTablaProductos();
    limpiarCampos();
}

//Funcion para actualizar la tabla de productos en la vista
function actualizarTablaProductos() {
    var tabla = document.getElementById('tablaProductos').getElementsByTagName('tbody')[0];
    tabla.innerHTML = '';

    for (var i = 0; i < productos.length; i++) {
        var fila = tabla.insertRow(i);
        var celdaProducto = fila.insertCell(0);
        var celdaPrecio = fila.insertCell(1);
        var celdaCantidad = fila.insertCell(2);

        celdaProducto.innerHTML = productos[i].Nombre;
        celdaPrecio.innerHTML = productos[i].Precio;
        celdaCantidad.innerHTML = productos[i].Cantidad;
    }
}

//Funcion para limpiar los campos a medida que agrego un producto nuevo
function limpiarCampos() {
    document.getElementById('producto').value = '';
    document.getElementById('precioCompra').value = '';
    document.getElementById('cantidad').value = '';
}