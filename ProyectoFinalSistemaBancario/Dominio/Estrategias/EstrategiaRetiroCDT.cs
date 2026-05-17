using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Estrategias
{
    /// <summary>
    /// Estrategia de retiro para CDT.
    /// Aplica RN-03: bloquea retiros hasta cumplir el plazo.
    /// Aplica RN-01: saldo no negativo si el plazo está cumplido.
    /// </summary>
    public class EstrategiaRetiroCDT : IEstrategiaRetiro
    {
        private readonly int _plazoMeses;
        private readonly int _mesesTranscurridos;

        public EstrategiaRetiroCDT(int plazoMeses, int mesesTranscurridos)
        {
            _plazoMeses = plazoMeses;
            _mesesTranscurridos = mesesTranscurridos;
        }

        public string Ejecutar(ref double saldo, double monto)
        {
            // RN-03: verificar que el plazo esté cumplido
            if (_mesesTranscurridos < _plazoMeses)
                return $"CDT bloqueado. Faltan " +
                       $"{_plazoMeses - _mesesTranscurridos} mes(es) " +
                       $"para cumplir el plazo de {_plazoMeses} meses.";

            // RN-01: verificar saldo suficiente
            if (saldo < monto)
                return $"Saldo insuficiente. " +
                       $"Saldo actual: ${saldo:N0}";

            saldo -= monto;
            return $"Plazo cumplido. Retiro exitoso de ${monto:N0}. " +
                   $"Nuevo saldo: ${saldo:N0}";
        }
    }
}