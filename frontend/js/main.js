console.log("JavaScript cargado correctamente");

document.addEventListener("DOMContentLoaded", function () {
  configurarLimpiezaDeFormularios();
  configurarValidacionDeFormularios();
  configurarBusquedaEnTablas();
  configurarConfirmacionEliminacion();
  configurarEdicionSimulada();
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
          guardarRegistroSimulado(formulario);
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
        guardarRegistroSimulado(formulario);
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

  return true;
}

// Simula el guardado despues de validar los campos obligatorios.
function guardarRegistroSimulado(formulario) {
  var formularioValido = validarFormulario(formulario);

  if (formularioValido) {
    mostrarMensajeFormulario("Registro guardado de forma simulada. Pendiente conexion con backend.");
  }
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

// Filtra las tablas simuladas cuando el usuario presiona un boton Buscar.
function configurarBusquedaEnTablas() {
  var botones = document.querySelectorAll("button");

  botones.forEach(function (boton) {
    var textoBoton = boton.textContent.trim().toLowerCase();

    if (textoBoton === "buscar") {
      boton.addEventListener("click", function (evento) {
        var formulario = obtenerFormularioRelacionado(boton);

        if (formulario) {
          evento.preventDefault();
          buscarEnTablas(formulario);
        }
      });
    }
  });

  document.querySelectorAll("form").forEach(function (formulario) {
    formulario.addEventListener("submit", function (evento) {
      var botonEnviado = evento.submitter;
      var textoBoton = botonEnviado ? botonEnviado.textContent.trim().toLowerCase() : "";

      if (textoBoton === "buscar") {
        evento.preventDefault();
        buscarEnTablas(formulario);
      }
    });
  });
}

// Obtiene el texto de busqueda y lo aplica a todas las tablas de la pagina.
function buscarEnTablas(formulario) {
  var textoBusqueda = obtenerTextoBusqueda(formulario);
  var tablas = document.querySelectorAll("table");

  if (textoBusqueda === "") {
    mostrarTodasLasFilas(tablas);
    return;
  }

  var resultadosEncontrados = filtrarTablasPorTexto(tablas, textoBusqueda);

  if (!resultadosEncontrados) {
    mostrarMensajeFormulario("No se encontraron resultados.");
  }
}

// Toma el valor del campo de busqueda principal del formulario.
function obtenerTextoBusqueda(formulario) {
  var campoBusqueda = formulario.querySelector("input[type='search']");

  if (!campoBusqueda) {
    campoBusqueda = formulario.querySelector("input[type='text']");
  }

  return campoBusqueda ? campoBusqueda.value.trim().toLowerCase() : "";
}

// Oculta o muestra filas segun el texto ingresado.
function filtrarTablasPorTexto(tablas, textoBusqueda) {
  var hayResultados = false;

  tablas.forEach(function (tabla) {
    var filas = tabla.querySelectorAll("tbody tr");

    filas.forEach(function (fila) {
      var textoFila = obtenerTextoFila(fila);
      var coincide = textoFila.indexOf(textoBusqueda) !== -1;

      fila.hidden = !coincide;

      if (coincide) {
        hayResultados = true;
      }
    });
  });

  return hayResultados;
}

// Convierte el contenido de una fila a texto comparable.
function obtenerTextoFila(fila) {
  return fila.textContent.trim().toLowerCase();
}

// Restaura todas las filas cuando la busqueda esta vacia.
function mostrarTodasLasFilas(tablas) {
  tablas.forEach(function (tabla) {
    var filas = tabla.querySelectorAll("tbody tr");

    filas.forEach(function (fila) {
      fila.hidden = false;
    });
  });
}

// Solicita confirmacion antes de eliminar registros de forma simulada.
function configurarConfirmacionEliminacion() {
  var botones = document.querySelectorAll("button");

  botones.forEach(function (boton) {
    var textoBoton = boton.textContent.trim().toLowerCase();

    if (textoBoton === "eliminar") {
      boton.addEventListener("click", function (evento) {
        evento.preventDefault();
        confirmarEliminacionSimulada();
      });
    }
  });
}

// Muestra mensajes segun la decision del usuario.
function confirmarEliminacionSimulada() {
  var confirmaEliminacion = confirm("Esta seguro de que desea eliminar este registro?");

  if (confirmaEliminacion) {
    mostrarMensajeFormulario("Registro eliminado de forma simulada. Pendiente conexion con backend.");
  } else {
    mostrarMensajeFormulario("Eliminacion cancelada.");
  }
}

// Simula la accion de editar registros en todo el sistema.
function configurarEdicionSimulada() {
  var botones = document.querySelectorAll("button");

  botones.forEach(function (boton) {
    var textoBoton = boton.textContent.trim().toLowerCase();

    if (textoBoton === "editar") {
      boton.addEventListener("click", function (evento) {
        evento.preventDefault();
        mostrarMensajeFormulario("Edicion simulada. Pendiente conexion con backend.");
      });
    }
  });
}

// Muestra un mensaje simple para confirmar la accion.
function mostrarMensajeFormulario(mensaje) {
  alert(mensaje);
}
