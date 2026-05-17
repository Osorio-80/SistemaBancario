using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalSistemaBancario.Entidades;
// Fabricas/ICuentaFactory.cs

namespace ProyectoFinalSistemaBancario.Fabricas
{
    /// <summary>
    /// Contrato del patrón Factory Method.
    /// Cada tipo de cuenta tiene su propia fábrica.
    /// El Form depende de esta abstracción.
    /// </summary>
    public interface ICuentaFactory
    {
        CuentaBancaria Crear(string numeroCuenta,
                             string nombreTitular,
                             double saldo,
                             Dictionary<string, object> parametros);
    }
}
