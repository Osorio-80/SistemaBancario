using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Estrategias
{
    /// <summary>
    /// Contrato del patrón Strategy para cálculo de intereses.
    /// </summary>
    public interface IEstrategiaInteres
    {
        double Calcular(double saldo);
    }
}
