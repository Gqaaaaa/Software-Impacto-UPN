console.log("JavaScript cargado correctamente");

document.addEventListener("DOMContentLoaded", function () {
  configurarLimpiezaDeFormularios();
  configurarValidacionDeFormularios();
});

// Activa la limpieza general para botones con texto Nuevo o Limpiar.
function configurarLimpiezaDeFormularios() {
  var botones = document.querySelectorAll("button");

  botones.forEach(function (boton) {
    var textoBoton = boton.textContent.trim().toLowerCase();

    if (textoBoton === "nuevo" || textoBoton === "limpiar") {
      boton.addEventListener("click", function (evento) {
        var formulario = obtenerFormularioRelacionado(boton);

        if (formulario) {
          evento.preventDefault();
          limpiarFormulario(formulario);
          mostrarMensajeFormulario("Formulario listo para nuevo registro.");
        }
      });
    }
  });
}

// Busca el formulario asociado al boton o el formulario principal de la pagina.
function obtenerFormularioRelacionado(boton) {
  if (boton.form) {
    return boton.form;
  }

  var seccion = boton.closest("section");

  if (seccion) {
    var formularioDeSeccion = seccion.querySelector("form");

    if (formularioDeSeccion) {
      return formularioDeSeccion;
    }
  }

  return document.querySelector("main form");
}

// Limpia los campos del formulario sin afectar tablas ni otros contenidos.
function limpiarFormulario(formulario) {
  formulario.reset();
}

// Valida formularios cuando el usuario presiona un boton Guardar.
function configurarValidacionDeFormularios() {
  var botones = document.querySelectorAll("button");

  botones.forEach(function (boton) {
    var textoBoton = boton.textContent.trim().toLowerCase();

    if (textoBoton.indexOf("guardar") === 0) {
      boton.addEventListener("click", function (evento) {
        var formulario = obtenerFormularioRelacionado(boton);

        if (formulario) {
          evento.preventDefault();
          validarFormulario(formulario);
        }
      });
    }
  });

  document.querySelectorAll("form").forEach(function (formulario) {
    formulario.addEventListener("submit", function (evento) {
      var botonEnviado = evento.submitter;
      var textoBoton = botonEnviado ? botonEnviado.textContent.trim().toLowerCase() : "";

      if (textoBoton.indexOf("guardar") === 0) {
        evento.preventDefault();
        validarFormulario(formulario);
      }
    });
  });
}

// Revisa campos obligatorios definidos con required dentro del formulario.
function validarFormulario(formulario) {
  var camposVacios = obtenerCamposObligatoriosVacios(formulario);

  if (camposVacios.length > 0) {
    camposVacios[0].focus();
    mostrarMensajeFormulario("Se deben completar los datos obligatorios.");
    return false;
  }

  mostrarMensajeFormulario("Datos validados correctamente. Pendiente conexion con backend.");
  return true;
}

// Obtiene inputs, selects o textareas obligatorios que esten vacios.
function obtenerCamposObligatoriosVacios(formulario) {
  var camposObligatorios = formulario.querySelectorAll("input[required], select[required], textarea[required]");
  var camposVacios = [];

  camposObligatorios.forEach(function (campo) {
    if (!campo.value.trim()) {
      camposVacios.push(campo);
    }
  });

  return camposVacios;
}

// Muestra un mensaje simple para confirmar la accion.
function mostrarMensajeFormulario(mensaje) {
  alert(mensaje);
}
