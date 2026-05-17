using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalSistemaBancario.Fabricas
{
    /// <summary>
    /// Mapea el tipo de cuenta con su fábrica correspondiente.
    /// Agregar CuentaPremium = agregar una línea aquí.
    /// El Form y el Servicio no cambian. (OCP demostrable)
    /// </summary>
    public static class RegistroDeFabricas
    {
        private static readonly Dictionary<string, ICuentaFactory> _fabricas =
            new Dictionary<string, ICuentaFactory>
            {
                { "Ahorros",   new CuentaAhorrosFactory()   },
                { "Corriente", new CuentaCorrienteFactory() },
                { "CDT",       new CDTFactory()             }
            };

        public static ICuentaFactory ObtenerFabrica(string tipo)
        {
            if (!_fabricas.ContainsKey(tipo))
                throw new ArgumentException(
                    $"Tipo de cuenta no soportado: {tipo}");
            return _fabricas[tipo];
        }

        // Para demostrar OCP en la presentación
        public static IEnumerable<string> ObtenerTiposDisponibles()
            => _fabricas.Keys;
    }
}
