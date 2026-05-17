using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Estrategias
{
    /// <summary>
    /// Estrategia de retiro estándar para CuentaAhorros.
    /// Aplica RN-01: saldo no puede quedar negativo.
    /// </summary>
    public class EstrategiaRetiroSimple : IEstrategiaRetiro
    {
        public string Ejecutar(ref double saldo, double monto)
        {
            // RN-01: verificar saldo suficiente
            if (saldo < monto)
                return $"Saldo insuficiente. " +
                       $"Saldo actual: ${saldo:N0}";

            saldo -= monto;
            return $"Retiro exitoso de ${monto:N0}. " +
                   $"Nuevo saldo: ${saldo:N0}";
        }
    }
}
