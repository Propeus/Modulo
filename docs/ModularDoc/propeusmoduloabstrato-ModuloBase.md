# ModuloBase `class`

## Description
Classe base para o modulo

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato
  Propeus.Modulo.Abstrato.ModuloBase[[ModuloBase]]
  Propeus.Modulo.Abstrato.BaseModelo[[BaseModelo]]
  end
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IModulo[[IModulo]]
  class Propeus.Modulo.Abstrato.Interfaces.IModulo interfaceStyle;
  Propeus.Modulo.Abstrato.Interfaces.IBaseModelo[[IBaseModelo]]
  class Propeus.Modulo.Abstrato.Interfaces.IBaseModelo interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
Propeus.Modulo.Abstrato.Interfaces.IModulo --> Propeus.Modulo.Abstrato.ModuloBase
Propeus.Modulo.Abstrato.Interfaces.IBaseModelo --> Propeus.Modulo.Abstrato.Interfaces.IModulo
System.IDisposable --> Propeus.Modulo.Abstrato.Interfaces.IBaseModelo
Propeus.Modulo.Abstrato.BaseModelo --> Propeus.Modulo.Abstrato.ModuloBase
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`IGerenciador`](./propeusmoduloabstratointerfaces-IGerenciador.md) | [`Gerenciador`](#gerenciador)<br>Gerenciador que está manipulando o modulo | `get` |
| `bool` | [`InstanciaUnica`](#instanciaunica)<br>Informa se o modulo é instancia unica | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `string` | [`ToString`](#tostring)()<br>Exibe informacoes basicas sobre o modulo |

## Details
### Summary
Classe base para o modulo

### Inheritance
 - [
`IModulo`
](./propeusmoduloabstratointerfaces-IModulo.md)
 - [
`IBaseModelo`
](./propeusmoduloabstratointerfaces-IBaseModelo.md)
 - `IDisposable`
 - [
`BaseModelo`
](./propeusmoduloabstrato-BaseModelo.md)

### Constructors
#### ModuloBase
```csharp
public ModuloBase(IGerenciador gerenciador, bool instanciaUnica)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IGerenciador`](./propeusmoduloabstratointerfaces-IGerenciador.md) | gerenciador | Gerenciador que irá controlar o modulo |
| `bool` | instanciaUnica | Informa se a instancia é unica ou multipla |

##### Summary
Inicializa um modulo

### Methods
#### ToString
```csharp
public override string ToString()
```
##### Summary
Exibe informacoes basicas sobre o modulo

##### Returns


### Properties
#### InstanciaUnica
```csharp
public virtual bool InstanciaUnica { get; }
```
##### Summary
Informa se o modulo é instancia unica

#### Gerenciador
```csharp
public IGerenciador Gerenciador { get; }
```
##### Summary
Gerenciador que está manipulando o modulo

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
