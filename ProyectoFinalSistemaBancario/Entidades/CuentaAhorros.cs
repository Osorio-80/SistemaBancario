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
    /// Cuenta de ahorros. Implementa IDepositable.
    /// Inyecta EstrategiaRetiroSimple y EstrategiaInteresEstandar(3%).
    /// </summary>
    public class CuentaAhorros : CuentaBancaria, IDepositable
    {
        public double TasaInteres { get; private set; }

        public CuentaAhorros(string numeroCuenta,
                              string nombreTitular,
                              double saldo)
            : base(numeroCuenta, nombreTitular, saldo,
                   new EstrategiaRetiroSimple(),
                   new EstrategiaInteresEstandar(3.0))
        {
            TasaInteres = 3.0;
        }

        /// <summary>
        /// Depósito estándar: suma el monto al saldo.
        /// </summary>
        public string Depositar(double monto)
        {
            Saldo += monto;
            return $"Depósito exitoso de ${monto:N0}. " +
                   $"Nuevo saldo: ${Saldo:N0}";
        }
    }
}
