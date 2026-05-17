using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalSistemaBancario.Entidades;

namespace ProyectoFinalSistemaBancario.Fabricas
{
    public class CDTFactory : ICuentaFactory
    {
        public CuentaBancaria Crear(string numeroCuenta,
                                    string nombreTitular,
                                    double saldo,
                                    Dictionary<string, object> parametros)
        {
            int plazo = (int)parametros["plazoMeses"];
            int meses = (int)parametros["mesesTranscurridos"];
            return new CDT(numeroCuenta, nombreTitular, saldo, plazo, meses);
        }
    }
}
