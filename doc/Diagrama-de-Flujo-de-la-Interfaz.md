# Diagrama de Flujo de la Interfaz

## Flujo General del Sistema
INICIO
│
▼
Form1 carga
│
├── InicializarUI()   → construye controles
├── WireEvents()      → conecta eventos
└── UpdateUIState()   → botones deshabilitados
│
▼
ESPERA DE EVENTO
│
├──────────────────────────────────────────┐
│                                          │
▼                                          ▼
[Selecciona CDT en ComboBox]        [Llena campos de cuenta]
│                                          │
▼                                          ▼
Aparece Panel CDT                   UpdateUIState()
(txtPlazoMeses,                     Habilita btnAgregarCuenta
txtMesesTranscurridos)                       │
▼
[Clic Registrar Cuenta]
│
▼
BtnAgregarCuenta_Click()
│
┌────────┴────────┐
│                 │
Válido           Inválido
│                 │
▼                 ▼
ServicioBancario    Resultado ROJO
.AgregarCuenta()   "Error: campos
│            inválidos"
▼
RegistroDeFabricas
.ObtenerFabrica(tipo)
│
▼
Factory crea objeto
(Ahorros/Corriente/CDT)
│
▼
Repository.Guardar()
│
▼
Resultado VERDE
"Cuenta registrada"

## Flujo de Depósito
[Ingresa número + monto]
│
▼
[Clic ↑ Depositar]
│
▼
BtnDepositar_Click()
│
▼
ServicioBancario.Depositar()
│
▼
Repository.ObtenerPorNumero()
│
┌─────┴─────┐
│           │
No existe    Existe
│           │
▼           ▼
Resultado   ¿implements IDepositable?
ROJO            │
"Cuenta no  ┌───┴───┐
encontrada" │       │
Sí      No (CDT)
│       │
▼       ▼
Depositar() Resultado ÁMBAR
Strategy    "No acepta
│        depósitos"
▼
Resultado VERDE
"Depósito exitoso"

## Flujo de Retiro
[Ingresa número + monto]
│
▼
[Clic ↓ Retirar]
│
▼
BtnRetirar_Click()
│
▼
ServicioBancario.Retirar()
│
▼
Repository.ObtenerPorNumero()
│
┌─────┴──────┐
│            │
No existe     Existe
│            │
▼            ▼
Resultado    cuenta.Retirar(monto)
ROJO         [Polimorfismo + Strategy]
│
┌────────┼────────┐
│        │        │
Ahorros   Corriente   CDT
│        │        │
▼        ▼        ▼
Verifica  Calcula   Verifica
saldo     4x1000    plazo
(RN-01)   (RN-04)   (RN-03)
│        │        │
▼        ▼        ▼
VERDE     VERDE     ¿Plazo cumplido?
o ROJO    o ROJO    ┌────┴────┐
Sí       No
│        │
▼        ▼
VERDE    ÁMBAR
"Faltan
X meses"

## Flujo de Consulta
[Ingresa número de cuenta]
│
┌────┴────┐
│         │
[Ver Todas] [Mi Cuenta]
│         │
▼         ▼
GetResumen  GetResumen
Todas()     Cuenta()
│         │
▼         ▼
Recorre     Busca por
lista       número
[foreach]       │
│      ┌───┴───┐
▼      │       │
ObtenerInfo() Existe  No existe


      │       │


CalcularInteres()▼  ▼
│    Info +  ROJO
▼    Interés "No encontrada"
Resultado INFO
(gris azul)

## Indicadores de Color del Sistema
VERDE  (#14532D) → Operación exitosa
ROJO   (#7F1D1D) → Error o saldo insuficiente
ÁMBAR  (#78350F) → Advertencia (CDT bloqueado)
GRIS   (#1E293B) → Información neutral

Commit message: "docs: agregar diagrama de flujo de interfaz"