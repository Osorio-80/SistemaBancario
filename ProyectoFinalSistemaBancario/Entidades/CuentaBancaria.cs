using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalSistemaBancario.Dominio.Estrategias;
using ProyectoFinalSistemaBancario.Dominio.Interfaces;

namespace ProyectoFinalSistemaBancario.Entidades
{
    /// <summary>
    /// Clase abstracta base de la jerarquía bancaria.
    /// Aplica Strategy: delega retiro e interés a estrategias inyectadas.
    /// No puede instanciarse directamente (abstracción).
    /// </summary>
    public abstract class CuentaBancaria : IRetirable, ICalculableIntereses
    {
        public string NumeroCuenta { get; protected set; }
        public string NombreTitular { get; protected set; }
        protected double Saldo;

        // Strategy: estrategias inyectadas por el constructor
        private readonly IEstrategiaRetiro _estrategiaRetiro;
        private readonly IEstrategiaInteres _estrategiaInteres;

        protected CuentaBancaria(string numeroCuenta,
                                  string nombreTitular,
                                  double saldo,
                                  IEstrategiaRetiro estrategiaRetiro,
                                  IEstrategiaInteres estrategiaInteres)
        {
            NumeroCuenta = numeroCuenta;
            NombreTitular = nombreTitular;
            Saldo = saldo;
            _estrategiaRetiro = estrategiaRetiro;
            _estrategiaInteres = estrategiaInteres;
        }

        /// <summary>
        /// Delega el retiro a la estrategia inyectada.
        /// El tipo real decide cómo retira (despacho dinámico + Strategy).
        /// </summary>
        public string Retirar(double monto)
            => _estrategiaRetiro.Ejecutar(ref Saldo, monto);

        /// <summary>
        /// Delega el cálculo de interés a la estrategia inyectada.
        /// </summary>
        public double CalcularInteres()
            => _estrategiaInteres.Calcular(Saldo);

        /// <summary>
        /// Virtual: tiene implementación base reutilizable.
        /// Las hijas pueden enriquecerla con override.
        /// </summary>
        public virtual string ObtenerInfo()
            => $"[{GetType().Name}] " +
               $"Cuenta: {NumeroCuenta} | " +
               $"Titular: {NombreTitular} | " +
               $"Saldo: ${Saldo:N0}";
    }
}
