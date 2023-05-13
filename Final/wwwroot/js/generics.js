/**datos para ingresar :
 * Usuario: admin
 * password: admin
**/

function login() {
const usuarioActual = document.getElementById("usuario").value;
const password = document.getElementById("password").value; 

if (usuarioActual === "admin" && password === "admin") {
    alert("Bienvenido usuario: " + usuarioActual);
  } else {
    alert("datos incorrectos, reintente");
  }
}


function limpiarCampos() {
  let inputs = document.querySelectorAll('input');
  inputs.forEach(input => {
      input.value = '';
  });
}