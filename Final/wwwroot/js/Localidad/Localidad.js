<script>
    function filtrarLocalidades() {
        var provinciaId = document.getElementById("provincia").value;

    var localidadesSelect = document.getElementById("localidad");

    localidadesSelect.innerHTML = "";

    var option = document.createElement("option");
    option.value = "";
    option.text = "Seleccione una localidad";
    localidadesSelect.appendChild(option);

    // Filtrar las localidades según la provincia seleccionada
    var localidades = @Json.Serialize(ViewBag.Localidades);

    for (var i = 0; i < localidades.length; i++) {
            if (localidades[i].IdProvincia == provinciaId) {
                var option = document.createElement("option");
    option.value = localidades[i].Id;
    option.text = localidades[i].Nombre;
    localidadesSelect.appendChild(option);
            }
        }
    }
</script>
