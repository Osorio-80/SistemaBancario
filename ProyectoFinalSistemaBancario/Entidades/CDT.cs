using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalSistemaBancario.Dominio.Estrategias;

namespace ProyectoFinalSistemaBancario.Entidades
{
    /// <summary>
    /// Certificado de Depósito a Término.
    /// NO implementa IDepositable (ISP + LSP correctos).
    /// Inyecta EstrategiaRetiroCDT (RN-03) e InteresCDT (8%).
    /// </summary>
    public class CDT : CuentaBancaria
    {
        public double TasaInteres { get; private set; }
        public int PlazoMeses { get; private set; }
        public int MesesTranscurridos { get; private set; }

        public CDT(string numeroCuenta,
                   string nombreTitular,
                   double saldo,
                   int plazoMeses,
                   int mesesTranscurridos)
            : base(numeroCuenta, nombreTitular, saldo,
                   new EstrategiaRetiroCDT(plazoMeses, mesesTranscurridos),
                   new EstrategiaInteresCDT(8.0, plazoMeses, mesesTranscurridos))
        {
            TasaInteres = 8.0;
            PlazoMeses = plazoMeses;
            MesesTranscurridos = mesesTranscurridos;
        }

        /// <summary>
        /// CDT no acepta depósitos (RN-02).
        /// Al no implementar IDepositable, este método
        /// no existe en el contrato público del CDT.
        /// Si alguien intenta depositarle, el servicio lo maneja.
        /// </summary>
        public override string ObtenerInfo()
            => base.ObtenerInfo() +
               $" | Plazo: {PlazoMeses} meses" +
               $" | Transcurridos: {MesesTranscurridos} meses";
    }
}
