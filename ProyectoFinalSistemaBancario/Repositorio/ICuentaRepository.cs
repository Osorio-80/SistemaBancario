using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalSistemaBancario.Entidades;

namespace ProyectoFinalSistemaBancario.Repositorio
{
    /// <summary>
    /// Contrato del repositorio de cuentas.
    /// N-Layer mejorado: Servicio depende de esta abstracción,
    /// nunca de la implementación concreta.
    /// </summary>
    public interface ICuentaRepository
    {
        void Guardar(CuentaBancaria cuenta);
        CuentaBancaria ObtenerPorNumero(string numeroCuenta);
        IEnumerable<CuentaBancaria> ObtenerTodas();
        void Actualizar(CuentaBancaria cuenta);
        void Eliminar(string numeroCuenta);
    }
}