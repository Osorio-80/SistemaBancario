using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Interfaces
{
    /// <summary>
    /// Contrato para cuentas que permiten retiros.
    /// Todos los tipos implementan esta interfaz.
    /// </summary>
    public interface IRetirable
    {
        string Retirar(double monto);
    }
}
