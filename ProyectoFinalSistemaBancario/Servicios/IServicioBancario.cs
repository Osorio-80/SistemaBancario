using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Servicios/IServicioBancario.cs
namespace ProyectoFinalSistemaBancario.Servicios
{
    /// <summary>
    /// Contrato del servicio bancario.
    /// N-Layer mejorado: el Form depende de esta abstracción.
    /// </summary>
    public interface IServicioBancario
    {
        void AgregarCuenta(string tipo,
                              string numeroCuenta,
                              string nombreTitular,
                              double saldo,
                              Dictionary<string, object> parametros);
        string Depositar(string numeroCuenta, double monto);
        string Retirar(string numeroCuenta, double monto);
        string GetResumenTodas();
        string GetResumenCuenta(string numeroCuenta);
    }
}
