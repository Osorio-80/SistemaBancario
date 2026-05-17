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
    /// Cuenta corriente. Implementa IDepositable.
    /// Inyecta EstrategiaRetiroConGravamen (RN-04) e interés 0%.
    /// </summary>
    public class CuentaCorriente : CuentaBancaria, IDepositable
    {
        public double CuotaManejo { get; private set; }

        public CuentaCorriente(string numeroCuenta,
                                string nombreTitular,
                                double saldo)
            : base(numeroCuenta, nombreTitular, saldo,
                   new EstrategiaRetiroConGravamen(),
                   new EstrategiaInteresEstandar(0.0))
        {
            CuotaManejo = 25000;
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

