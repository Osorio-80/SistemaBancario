# Sistema Bancario
### Herramientas de Programación II

¿Qué hace este sistema?

Simula las operaciones fundamentales de una entidad bancaria.
El operador puede registrar cuentas de Ahorro, Corriente y CDT,
realizar depósitos y retiros, y consultar saldos e intereses.
Cada tipo de cuenta aplica sus propias reglas financieras.

---

Tecnología

| Componente | Tecnología |
|---|---|
| Lenguaje | C# / .NET |
| Interfaz | Windows Forms |
| Arquitectura | N-Layer mejorado con interfaces |
| Patrones GoF | Strategy, Factory Method, Repository |
| Principios | SOLID completo |

---

Cómo ejecutar

1. Clonar el repositorio
2. Abrir `SistemaBancario.sln` en Visual Studio 2026
3. Verificar que el proyecto apunta a .NET 9
4. Presionar F5

---

Estructura del proyecto

📁 Dominio

  -	Interfaces     → IDepositable, IRetirable, ICalculableIntereses
  
  -	Estrategias    → Strategy Pattern (retiro e interés)
    
📁 Entidades         → CuentaBancaria (abstract), Ahorros, Corriente, CDT

📁 Repositorio       → ICuentaRepository + InMemoryRepositoryCuenta

📁 Fabricas          → Factory Method + RegistroDeFabricas

📁 Servicios         → IServicioBancario + ServicioBancario

📁 Presentacion      → Form1.cs

Patrones de diseño implementados

Strategy
Extrae las reglas de retiro e interés a clases independientes.
Agregar un nuevo tipo de cuenta no modifica ninguna clase existente.

Factory Method
Delega la creación de cuentas a fábricas especializadas.
El Form nunca usa `new CuentaAhorros()` directamente.

Repository
Desacopla la persistencia del servicio mediante ICuentaRepository.
Cambiar a SQL requiere una sola línea en el punto de composición.

---

Principios SOLID

| Principio | Aplicación |
|---|---|
| SRP | Cada clase tiene una sola responsabilidad |
| OCP | Nuevo tipo de cuenta = nuevos archivos, nada se modifica |
| LSP | CDT no implementa IDepositable, evita contratos rotos |
| ISP | IDepositable, IRetirable e ICalculableIntereses segregados |
| DIP | Todas las capas dependen de interfaces, no de concretos |

---

Reglas de negocio

| Regla | Descripción | Implementada en |
|---|---|---|
| RN-01 | Saldo nunca negativo | EstrategiaRetiroSimple y ConGravamen |
| RN-02 | CDT no acepta depósitos | CDT no implementa IDepositable |
| RN-03 | CDT bloquea retiros hasta cumplir plazo | EstrategiaRetiroCDT |
| RN-04 | CuentaCorriente cobra 4x1000 | EstrategiaRetiroConGravamen |

---

Equipo

| Integrante | Rol principal |
|---|---|
| [Miguel Osorio] | Arquitectura y patrones |
| [Carlos Muriel] | Entidades y estrategias |
| [Davinson Correa] | Interfaz y documentación |

---
