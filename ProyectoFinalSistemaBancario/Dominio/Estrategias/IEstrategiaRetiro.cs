using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Dominio.Estrategias
{
    /// <summary>
    /// Contrato del patrón Strategy para retiros.
    /// Cada tipo de cuenta inyecta su propia implementación.
    /// El ref permite modificar el saldo directamente.
    /// </summary>
    public interface IEstrategiaRetiro
    {
        string Ejecutar(ref double saldo, double monto);
    }
}
