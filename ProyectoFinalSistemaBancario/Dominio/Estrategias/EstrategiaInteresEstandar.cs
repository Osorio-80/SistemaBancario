using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Estrategias
{
    /// <summary>
    /// Estrategia de interés estándar.
    /// Fórmula: Saldo * tasa / 100.
    /// Usada por CuentaAhorros (3%) y CuentaCorriente (0%).
    /// </summary>
    public class EstrategiaInteresEstandar : IEstrategiaInteres
    {
        private readonly double _tasa;

        public EstrategiaInteresEstandar(double tasa)
        {
            _tasa = tasa;
        }

        public double Calcular(double saldo)
            => saldo * (_tasa / 100);
    }
}