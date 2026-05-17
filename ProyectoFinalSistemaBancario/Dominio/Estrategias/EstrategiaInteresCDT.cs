using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Estrategias
{
    /// <summary>
    /// Estrategia de interés para CDT.
    /// Fórmula: Saldo * 8% * (mesesTranscurridos / plazoMeses).
    /// Cast explícito a double para evitar división entera.
    /// </summary>
    public class EstrategiaInteresCDT : IEstrategiaInteres
    {
        private readonly double _tasa;
        private readonly int _plazoMeses;
        private readonly int _mesesTranscurridos;

        public EstrategiaInteresCDT(double tasa,
                                     int plazoMeses,
                                     int mesesTranscurridos)
        {
            _tasa = tasa;
            _plazoMeses = plazoMeses;
            _mesesTranscurridos = mesesTranscurridos;
        }

        public double Calcular(double saldo)
            => saldo * (_tasa / 100) *
               ((double)_mesesTranscurridos / _plazoMeses);
    }
}
