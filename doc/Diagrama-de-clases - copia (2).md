# Diagrama de Clases

## Jerarquía principal

CuentaBancaria (abstract)
  ├── implements: IRetirable, ICalculableIntereses
  ├── CuentaAhorros    → implements: IDepositable
  ├── CuentaCorriente  → implements: IDepositable
  └── CDT              → NO implements IDepositable

## Estrategias

IEstrategiaRetiro
  ├── EstrategiaRetiroSimple
  ├── EstrategiaRetiroConGravamen
  └── EstrategiaRetiroCDT

IEstrategiaInteres
  ├── EstrategiaInteresEstandar
  └── EstrategiaInteresCDT

## Repositorio

ICuentaRepository
  └── InMemoryRepositoryCuenta

## Servicio

IServicioBancario
  └── ServicioBancario
        ├── depende de: ICuentaRepository
        └── usa: RegistroDeFabricas → ICuentaFactory

## Factory

ICuentaFactory
  ├── CuentaAhorrosFactory
  ├── CuentaCorrienteFactory
  └── CDTFactory