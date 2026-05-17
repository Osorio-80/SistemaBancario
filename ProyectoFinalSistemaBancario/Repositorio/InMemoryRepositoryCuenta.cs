using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalSistemaBancario.Entidades;

namespace ProyectoFinalSistemaBancario.Repositorio
{
    /// <summary>
    /// Implementación en memoria de ICuentaRepository.
    /// El sistema funciona exactamente igual que antes.
    /// Si mañana llega SqlRepository, solo se cambia
    /// esta clase en el punto de composición del Form.
    /// </summary>
    public class InMemoryRepositoryCuenta : ICuentaRepository
    {
        private readonly List<CuentaBancaria> _cuentas = new();

        public void Guardar(CuentaBancaria cuenta)
            => _cuentas.Add(cuenta);

        public CuentaBancaria ObtenerPorNumero(string numeroCuenta)
            => _cuentas.FirstOrDefault(
                c => c.NumeroCuenta == numeroCuenta);

        public IEnumerable<CuentaBancaria> ObtenerTodas()
            => _cuentas.AsReadOnly();

        public void Actualizar(CuentaBancaria cuenta)
        {
            var existente = ObtenerPorNumero(cuenta.NumeroCuenta);
            if (existente != null)
            {
                _cuentas.Remove(existente);
                _cuentas.Add(cuenta);
            }
        }

        public void Eliminar(string numeroCuenta)
        {
            var cuenta = ObtenerPorNumero(numeroCuenta);
            if (cuenta != null) _cuentas.Remove(cuenta);
        }
    }
}
