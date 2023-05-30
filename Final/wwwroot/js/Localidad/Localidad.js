$(document).ready(function () {
    $('#provincia').change(function () {
        var provinciaId = $(this).val();

        if (provinciaId) {
            $.ajax({
                url: '/Cliente/CargarLocalidades',
                type: 'POST',
                data: { provinciaId: provinciaId },
                success: function (response) {
                    // Ordenar las opciones alfabéticamente
                    var sortedOptions = $(response).sort(function (a, b) {
                        return $(a).text().localeCompare($(b).text());
                    });

                    $('#localidad').html(sortedOptions);
                    $('#localidad').prop('disabled', false);
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                }
            });
        } else {
            $('#localidad').html('<option value="">Seleccione una localidad</option>');
            $('#localidad').prop('disabled', true);
        }
    });
});