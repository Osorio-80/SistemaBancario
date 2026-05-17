# Mapeo de Principios SOLID

## Tabla resumen

| Principio | Violación anterior | Corrección aplicada |
|---|---|---|
| SRP | ServicioBancario gestionaba lista, operaciones y formato | Separado en Servicio, Repository y Fábricas |
| OCP | Agregar cuenta nueva obligaba a modificar Form y Servicio | Factory + Strategy: solo agregar archivos |
| LSP | CDT.Depositar() rompía el contrato de CuentaBancaria | CDT no implementa IDepositable |
| ISP | CuentaBancaria forzaba Depositar() en todas las hijas | IDepositable, IRetirable e ICalculableIntereses segregados |
| DIP | Form dependía de ServicioBancario concreto | Todas las capas dependen de interfaces |

---

## S — Single Responsibility Principle
### Una clase, una razón para cambiar

**Definición simple:** Cada clase hace una sola cosa.
Como en un restaurante: el chef cocina, el mesero sirve,
el cajero cobra. Nadie hace el trabajo del otro.

**Cómo se ve en el código:**
CuentaBancaria    → define estructura del dominio bancario
EstrategiaRetiro  → encapsula la lógica de UN tipo de retiro
ServicioBancario  → coordina operaciones entre capas
InMemoryRepository→ gestiona la persistencia en memoria
Form1             → captura, delega y muestra. Nada más

**Evidencia concreta:**
Si cambia la regla del 4x1000 colombiana, solo cambia
EstrategiaRetiroConGravamen.cs. Ninguna otra clase se toca.

```csharp
// EstrategiaRetiroConGravamen — responsabilidad única: aplicar 4x1000
public class EstrategiaRetiroConGravamen : IEstrategiaRetiro
{
    private const double TasaGravamen = 0.004; // ← solo aquí vive esta regla

    public string Ejecutar(ref double saldo, double monto)
    {
        double comision = monto * TasaGravamen;
        // ...
    }
}
```

---

## O — Open/Closed Principle
### Abierto para extensión, cerrado para modificación

**Definición simple:** El sistema puede crecer sin romperse.
Como un edificio donde puedes agregar pisos sin tocar
los cimientos ni los pisos existentes.

**Evidencia concreta:**
Agregar CuentaPremium con retiro diferente:
ARCHIVOS NUEVOS (extensión):
✓ EstrategiaRetiroPremium.cs
✓ CuentaPremiumFactory.cs
✓ Una línea en RegistroDeFabricas
ARCHIVOS QUE NO SE TOCAN (cerrado):
✗ Form1.cs
✗ ServicioBancario.cs
✗ CuentaBancaria.cs
✗ EstrategiaRetiroSimple.cs
✗ EstrategiaRetiroConGravamen.cs
✗ EstrategiaRetiroCDT.cs

```csharp
// RegistroDeFabricas — agregar Premium = una línea
{ "Ahorros",   new CuentaAhorrosFactory()   },
{ "Corriente", new CuentaCorrienteFactory() },
{ "CDT",       new CDTFactory()             },
{ "Premium",   new CuentaPremiumFactory()   } // ← solo esto
```

---

## L — Liskov Substitution Principle
### Los hijos deben respetar el contrato del padre

**Definición simple:** Si tienes una cuenta bancaria,
deberías poder depositar en ella. Si un tipo de cuenta
te da sorpresas inesperadas, algo está mal.

**El problema anterior:**

```csharp
// ❌ ANTES — CDT rompía el contrato
// Cualquier código que recibiera CuentaBancaria y llamara
// Depositar() esperaba que funcionara. CDT sorprendía.
public override string Depositar(double monto)
    => "Un CDT no acepta depósitos"; // comportamiento inesperado
```

**La solución:**

```csharp
// ✅ AHORA — CDT directamente no tiene Depositar()
// El compilador garantiza que nadie puede llamarlo
public class CDT : CuentaBancaria // sin IDepositable
{ }

// ServicioBancario verifica estructuralmente
if (cuenta is IDepositable depositable)
    return depositable.Depositar(monto);
else
    return "Esta cuenta no acepta depósitos. (RN-02)";
```

---

## I — Interface Segregation Principle
### No obligues a implementar lo que no se necesita

**Definición simple:** No le des a alguien responsabilidades
que no le corresponden. Un CDT no debería tener un método
Depositar aunque sea para decir que no puede usarlo.

**Interfaces segregadas implementadas:**
IDepositable

Depositar(monto) : string
→ Implementan: CuentaAhorros ✓  CuentaCorriente ✓  CDT ✗

IRetirable

Retirar(monto) : string
→ Implementan: CuentaAhorros ✓  CuentaCorriente ✓  CDT ✓

ICalculableIntereses

CalcularInteres() : double
→ Implementan: CuentaAhorros ✓  CuentaCorriente ✓  CDT ✓


**Antes vs ahora:**

```csharp
// ❌ ANTES — interfaz monolítica (todos debían implementar todo)
public interface ICuentaBancaria
{
    string Depositar(double monto);  // CDT no debería tener esto
    string Retirar(double monto);
    double CalcularInteres();
}

// ✅ AHORA — interfaces segregadas
public interface IDepositable         { string Depositar(double monto); }
public interface IRetirable           { string Retirar(double monto); }
public interface ICalculableIntereses { double CalcularInteres(); }
// CDT solo implementa IRetirable e ICalculableIntereses
```

---

## D — Dependency Inversion Principle
### Depender de abstracciones, no de concretos

**Definición simple:** Los jefes no dependen de empleados
específicos, dependen del cargo. Si el empleado cambia,
el jefe no cambia su forma de trabajar.

**Mapa de dependencias del sistema:**
Form1
└──► IServicioBancario         ← NO depende de ServicioBancario
ServicioBancario
├──► ICuentaRepository         ← NO depende de InMemoryRepository
└──► ICuentaFactory            ← NO depende de CuentaAhorrosFactory
CuentaBancaria
├──► IEstrategiaRetiro         ← NO depende de EstrategiaRetiroSimple
└──► IEstrategiaInteres        ← NO depende de EstrategiaInteresEstandar

**Evidencia en código:**

```csharp
// Form1 — depende de la interfaz
private IServicioBancario _servicio; // ← interfaz, no concreto

// ServicioBancario — recibe la abstracción por constructor
public ServicioBancario(ICuentaRepository repositorio)
{
    _repositorio = repositorio; // ← interfaz, no concreto
}

// CuentaBancaria — recibe estrategias por constructor
protected CuentaBancaria(string numeroCuenta,
                          string nombreTitular,
                          double saldo,
                          IEstrategiaRetiro  estrategiaRetiro,  // ← interfaz
                          IEstrategiaInteres estrategiaInteres) // ← interfaz
```

**Punto de composición — donde se conecta todo:**

```csharp
// Form1 constructor — única clase que conoce los concretos
ICuentaRepository repositorio = new InMemoryRepositoryCuenta();
IServicioBancario servicio    = new ServicioBancario(repositorio);
_servicio = servicio;
// Si mañana hay SQL: solo cambiar InMemoryRepositoryCuenta
// por SqlRepositoryCuenta en esta línea. Nada más cambia.
```