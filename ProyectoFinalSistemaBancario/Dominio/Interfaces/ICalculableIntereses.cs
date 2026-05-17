using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Interfaces
{
    /// <summary>
    /// Contrato para cuentas que calculan intereses.
    /// </summary>
    public interface ICalculableIntereses
    {
        double CalcularInteres();
    }
}
