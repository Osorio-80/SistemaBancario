using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Interfaces
{
    /// <summary>
    /// Contrato para cuentas que aceptan depósitos.
    /// CDT NO implementa esta interfaz (ISP + LSP).
    /// </summary>
    public interface IDepositable
    {
        string Depositar(double monto);
    }
}
