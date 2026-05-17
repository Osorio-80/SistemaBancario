# Diagrama de Estructura del Proyecto

PRESENTACION ──────► IServicioBancario
                            │
                     ServicioBancario
                      │           │
              ICuentaRepository   RegistroDeFabricas
                      │                  │
          InMemoryRepository       ICuentaFactory
                      │            ├── AhorrosFactory
                  CuentaBancaria   ├── CorrienteFactory
                  (abstracta)      └── CDTFactory
                      │
              IEstrategiaRetiro + IEstrategiaInteres
                   (Strategy Pattern)