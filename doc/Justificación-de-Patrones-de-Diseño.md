# Justificación de Patrones de Diseño y Arquitectura

## Arquitectura — N-Layer Mejorado

### ¿Qué es?
Organización del sistema en capas donde cada una tiene
una responsabilidad única y se comunica con la siguiente
únicamente a través de interfaces, nunca de concretos.

### ¿Por qué la elegimos?
La arquitectura anterior tenía tres problemas concretos:
- El Form sabía qué clases concretas existían (new CuentaAhorros)
- Las reglas de negocio vivían mezcladas dentro de las entidades
- Cambiar la persistencia requería reescribir el servicio

N-Layer mejorado resuelve los tres sin agregar complejidad
injustificada para el tamaño del proyecto.

### Comparación antes vs ahora

| Aspecto            | Antes                        | Ahora                        |
|--------------------|------------------------------|------------------------------|
| Comunicación       | Directa entre concretos      | Interfaces en todos los cruces|
| Creación objetos   | new en el Form               | Factory Method               |
| Reglas de negocio  | Dentro de las entidades      | Estrategias independientes   |
| Persistencia       | Lista directa en Servicio    | ICuentaRepository inyectado  |
| Agregar tipo nuevo | Modificar Form + Servicio    | Solo agregar archivos nuevos |
| Violación LSP      | CDT.Depositar() rompía contrato | CDT no implementa IDepositable|

---

## Patrón 1 — Strategy (Comportamiento)

### Problema que resuelve
Antes, el comportamiento de retiro y cálculo de interés
estaba hardcodeado dentro de cada clase hija mediante override.

```csharp// ❌ ANTES — lógica atrapada en la herencia
public class CuentaCorriente : CuentaBancaria
{
public override string Retirar(double monto)
{
// lógica del 4x1000 mezclada aquí
// si cambia la regla, hay que entrar a esta clase
}
}

### Solución implementada
Se extrajeron las reglas a clases independientes e intercambiables.

```csharp// ✅ AHORA — Strategy inyectada por constructor
public class CuentaCorriente : CuentaBancaria, IDepositable
{
public CuentaCorriente(string numeroCuenta,
string nombreTitular, double saldo)
: base(numeroCuenta, nombreTitular, saldo,
new EstrategiaRetiroConGravamen(), // ← Strategy RN-04
new EstrategiaInteresEstandar(0.0))
{ }
}// La regla del 4x1000 vive aquí, aislada y testeable
public class EstrategiaRetiroConGravamen : IEstrategiaRetiro
{
private const double TasaGravamen = 0.004;public string Ejecutar(ref double saldo, double monto)
{
    double comision       = monto * TasaGravamen;
    double totalDescontar = monto + comision;    if (saldo < totalDescontar)
        return $"Saldo insuficiente. Se requieren ${totalDescontar:N0}";    saldo -= totalDescontar;
    return $"Retiro exitoso. Comisión 4x1000: ${comision:N0}";
}
}

### Dónde vive en el códigoDominio/Estrategias/
IEstrategiaRetiro.cs
IEstrategiaInteres.cs
EstrategiaRetiroSimple.cs        → CuentaAhorros
EstrategiaRetiroConGravamen.cs   → CuentaCorriente (RN-04)
EstrategiaRetiroCDT.cs           → CDT (RN-03)
EstrategiaInteresEstandar.cs     → Ahorros 3%, Corriente 0%
EstrategiaInteresCDT.cs          → CDT 8% proporcional

### Beneficio demostrable
Agregar CuentaPremium con retiro diferente requiere:
- Crear EstrategiaRetiroPremium.cs (archivo nuevo)
- Ninguna clase existente se modifica (OCP aplicado)

---

## Patrón 2 — Factory Method (Creacional)

### Problema que resuelve
El Form tenía conocimiento directo del modelo de dominio,
violando la separación de capas.

```csharp// ❌ ANTES — Form acoplado a los concretos
if (tipo == "Ahorros")
nuevaCuenta = new CuentaAhorros(numero, nombre, saldo);
else if (tipo == "Corriente")
nuevaCuenta = new CuentaCorriente(numero, nombre, saldo);
else if (tipo == "CDT")
nuevaCuenta = new CDT(numero, nombre, saldo, plazo, meses);
// Agregar CuentaPremium = modificar el Form

### Solución implementada
Cada tipo de cuenta tiene su propia fábrica. Un registro
central mapea el string con la fábrica correcta.

```csharp// ✅ AHORA — Form no sabe qué clase concreta se crea
_servicio.AgregarCuenta(tipo, numero, nombre, saldo, parametros);// ServicioBancario usa la fábrica
public void AgregarCuenta(string tipo, string numero,
string nombre, double saldo,
Dictionary<string, object> parametros)
{
ICuentaFactory fabrica = RegistroDeFabricas.ObtenerFabrica(tipo);
CuentaBancaria cuenta  = fabrica.Crear(numero, nombre, saldo, parametros);
_repositorio.Guardar(cuenta);
}// RegistroDeFabricas — agregar Premium = una línea
private static readonly Dictionary<string, ICuentaFactory> _fabricas =
new Dictionary<string, ICuentaFactory>
{
{ "Ahorros",   new CuentaAhorrosFactory()   },
{ "Corriente", new CuentaCorrienteFactory() },
{ "CDT",       new CDTFactory()             }
// { "Premium", new CuentaPremiumFactory() } ← solo esto
};

### Dónde vive en el códigoFabricas/
ICuentaFactory.cs
CuentaAhorrosFactory.cs
CuentaCorrienteFactory.cs
CDTFactory.cs
RegistroDeFabricas.cs

### Beneficio demostrable
Agregar CuentaPremium requiere:
- Crear CuentaPremiumFactory.cs (archivo nuevo)
- Una línea en RegistroDeFabricas
- Form1.cs no cambia
- ServicioBancario.cs no cambia

---

## Patrón 3 — Repository (Arquitectónico)

### Problema que resuelve
ServicioBancario dependía directamente de List<CuentaBancaria>.
Cambiar a base de datos requería reescribir el servicio completo.

```csharp// ❌ ANTES — dependencia directa a la lista
public class ServicioBancario
{
private List<CuentaBancaria> _cuentas = new List<CuentaBancaria>();
// Cambiar a SQL = reescribir todo esto
}

### Solución implementada
El servicio depende de ICuentaRepository. La implementación
concreta se inyecta por constructor desde el Form.

```csharp// ✅ AHORA — dependencia de la abstracción
public class ServicioBancario : IServicioBancario
{
private readonly ICuentaRepository _repositorio;public ServicioBancario(ICuentaRepository repositorio)
{
    _repositorio = repositorio; // inyectado, no creado
}
}// Form1 — punto de composición
ICuentaRepository repositorio = new InMemoryRepositoryCuenta();
_servicio = new ServicioBancario(repositorio);
// Cambiar a SQL = solo esta línea cambia

### Dónde vive en el códigoRepositorio/
ICuentaRepository.cs
InMemoryRepositoryCuenta.cs

### Beneficio demostrable
Migrar a SQL requiere:
- Crear SqlRepositoryCuenta : ICuentaRepository
- Cambiar una línea en Form1 constructor
- ServicioBancario.cs no cambia
- Ninguna otra clase cambia