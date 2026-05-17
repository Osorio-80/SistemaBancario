using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalSistemaBancario.Dominio.Interfaces;
using ProyectoFinalSistemaBancario.Entidades;
using ProyectoFinalSistemaBancario.Fabricas;
using ProyectoFinalSistemaBancario.Repositorio;

// Servicios/ServicioBancario.cs
namespace ProyectoFinalSistemaBancario.Servicios
{
    /// <summary>
    /// Implementación del servicio bancario.
    /// Coordina Factory Method, Repository y polimorfismo.
    /// No conoce ningún control de Windows Forms.
    /// Depende de abstracciones (DIP).
    /// </summary>
    public class ServicioBancario : IServicioBancario
    {
        // DIP: depende de interfaces, no de concretos
        private readonly ICuentaRepository _repositorio;

        public ServicioBancario(ICuentaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        /// <summary>
        /// Factory Method: obtiene la fábrica correcta y crea la cuenta.
        /// El servicio no sabe qué clase concreta se crea.
        /// </summary>
        public void AgregarCuenta(string tipo,
                                   string numeroCuenta,
                                   string nombreTitular,
                                   double saldo,
                                   Dictionary<string, object> parametros)
        {
            ICuentaFactory fabrica = RegistroDeFabricas.ObtenerFabrica(tipo);
            CuentaBancaria cuenta = fabrica.Crear(
                numeroCuenta, nombreTitular, saldo, parametros);
            _repositorio.Guardar(cuenta);
        }

        /// <summary>
        /// Busca la cuenta y delega el depósito.
        /// Verifica si implementa IDepositable (CDT no lo hace).
        /// </summary>
        public string Depositar(string numeroCuenta, double monto)
        {
            var cuenta = _repositorio.ObtenerPorNumero(numeroCuenta);
            if (cuenta == null)
                return "Cuenta no encontrada.";

            // ISP: solo las cuentas depositables aceptan depósitos
            if (cuenta is IDepositable depositable)
                return depositable.Depositar(monto);

            return "Esta cuenta no acepta depósitos. (RN-02)";
        }

        /// <summary>
        /// Busca la cuenta y delega el retiro.
        /// Strategy ejecuta la lógica correcta según el tipo.
        /// </summary>
        public string Retirar(string numeroCuenta, double monto)
        {
            var cuenta = _repositorio.ObtenerPorNumero(numeroCuenta);
            if (cuenta == null)
                return "Cuenta no encontrada.";

            // Polimorfismo + Strategy: cada cuenta retira diferente
            return cuenta.Retirar(monto);
        }

        /// <summary>
        /// Recorre toda la lista con polimorfismo y Strategy.
        /// ObtenerInfo() y CalcularInteres() se comportan
        /// diferente según el tipo real de cada objeto.
        /// </summary>
        public string GetResumenTodas()
        {
            var cuentas = _repositorio.ObtenerTodas().ToList();

            if (!cuentas.Any())
                return "No hay cuentas registradas.";

            string resultado = "";
            foreach (CuentaBancaria c in cuentas)
            {
                resultado += c.ObtenerInfo() + "\r\n";
                resultado += $"Interés calculado: " +
                             $"${c.CalcularInteres():N0}\r\n";
                resultado += "\r\n";
            }
            return resultado;
        }

        /// <summary>
        /// Devuelve el resumen de una cuenta específica.
        /// </summary>
        public string GetResumenCuenta(string numeroCuenta)
        {
            var cuenta = _repositorio.ObtenerPorNumero(numeroCuenta);
            if (cuenta == null) return "Cuenta no encontrada.";

            return cuenta.ObtenerInfo() + "\r\n" +
                   $"Interés calculado: ${cuenta.CalcularInteres():N0}";
        }
    }
}
