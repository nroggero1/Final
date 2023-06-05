
var productosCount = 1;

function agregarProducto() {
    var nuevoProductoHtml = `<div class="producto-group">
                                    <hr>
                                    <h3>Producto adicional</h3>

                                    <!-- Producto -->
                                    <div class="input-group mb-3">
                                        <label class="input-group-text">Producto</label>
                                        <select class="form-control" name="productos[${productosCount}].IdProducto">
                                            <option value="">Seleccione un producto</option>
                                            @foreach (var producto in ViewBag.Productos)
                                            {
                                                <option value="@producto.Id">@producto.Nombre</option>
                                            }
                                        </select>
                                    </div>

                                    <!-- Cantidad -->
                                    <div class="input-group mb-3">
                                        <label class="input-group-text">Cantidad</label>
                                        <input type="number" class="form-control" name="productos[${productosCount}].Cantidad" placeholder="Ingresar cantidad">
                                    </div>

                                    <!-- Precio de Compra -->
                                    <div class="input-group mb-3">
                                        <label class="input-group-text">Precio de Compra</label>
                                        <input type="number" class="form-control" name="productos[${productosCount}].PrecioCompra" placeholder="Ingresar Precio de Compra">
                                    </div>

                                    <!-- Resto de los campos del producto adicional -->
                                    <!-- ... -->

                                </div>`;

    $("#productos-container").append(nuevoProductoHtml);
    productosCount++;
}