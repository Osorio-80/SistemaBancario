using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Estrategias
{
    /// <summary>
    /// Estrategia de retiro para CuentaCorriente.
    /// Aplica RN-04: cobra 4x1000 sobre el monto retirado.
    /// Aplica RN-01: saldo debe cubrir monto más comisión.
    /// </summary>
    public class EstrategiaRetiroConGravamen : IEstrategiaRetiro
    {
        // RN-04: tasa del gravamen financiero colombiano
        private const double TasaGravamen = 0.004;

        public string Ejecutar(ref double saldo, double monto)
        {
            double comision = monto * TasaGravamen;
            double totalDescontar = monto + comision;

            // RN-01: verificar que el saldo cubra monto + comisión
            if (saldo < totalDescontar)
                return $"Saldo insuficiente. Se requieren " +
                       $"${totalDescontar:N0} " +
                       $"(retiro ${monto:N0} + comisión ${comision:N0}). " +
                       $"Saldo actual: ${saldo:N0}";

            saldo -= totalDescontar;
            return $"Retiro exitoso de ${monto:N0}. " +
                   $"Comisión 4x1000: ${comision:N0}. " +
                   $"Nuevo saldo: ${saldo:N0}";
        }
    }
}
