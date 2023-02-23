# BaseModelo `class`

## Description
Classe com o modelo base para todo o projeto

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato
  Propeus.Modulo.Abstrato.BaseModelo[[BaseModelo]]
  end
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IBaseModelo[[IBaseModelo]]
  class Propeus.Modulo.Abstrato.Interfaces.IBaseModelo interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
Propeus.Modulo.Abstrato.Interfaces.IBaseModelo --> Propeus.Modulo.Abstrato.BaseModelo
System.IDisposable --> Propeus.Modulo.Abstrato.Interfaces.IBaseModelo
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `bool` | [`Disposed`](#disposed)<br>Informa se as refencias já foram liberadas do objeto. | `get` |
| [`Estado`](./propeusmoduloabstrato-Estado) | [`Estado`](#estado)<br>Representa o estado do objeto. É definido inicialmente como 1 (iniciado) ou 0 (disposed), podendo ser alterado pelo usuario final ou pela regra de negocio | `get, set` |
| `string` | [`Id`](#id)<br>Representação alfanumerica e unica do objeto. | `get` |
| `string` | [`Nome`](#nome)<br>Representação amigavel do objeto. Caso seja nulo o nome da classe herdado será informado na propriedade. | `get, protected set` |
| `string` | [`Versao`](#versao)<br>Versão do assembly onde o modulo foi carregado. {Maj}.{Min}.{Build} | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-22)() |
| `string` | [`ToString`](#tostring)()<br>Exibe informações basicas sobre o modelo |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-12)(`bool` disposing) |

## Details
### Summary
Classe com o modelo base para todo o projeto

### Inheritance
 - [
`IBaseModelo`
](./propeusmoduloabstratointerfaces-IBaseModelo)
 - `IDisposable`

### Constructors
#### BaseModelo [1/2]
```csharp
public BaseModelo()
```
##### Summary
Inicia um modelo basico

#### BaseModelo [2/2]
```csharp
public BaseModelo(string nome)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nome | Nome do modelo |

##### Summary
Inicia um modelo com um nome customizado

### Methods
#### ToString
```csharp
public override string ToString()
```
##### Summary
Exibe informações basicas sobre o modelo

##### Returns


#### Dispose [1/2]
```csharp
protected virtual void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing |   |

#### Dispose [2/2]
```csharp
public virtual void Dispose()
```

### Properties
#### Versao
```csharp
public virtual string Versao { get; }
```
##### Summary
Versão do assembly onde o modulo foi carregado. {Maj}.{Min}.{Build}

#### Disposed
```csharp
public virtual bool Disposed { get; }
```
##### Summary
Informa se as refencias já foram liberadas do objeto.

#### Estado
```csharp
public Estado Estado { get; set; }
```
##### Summary
Representa o estado do objeto. É definido inicialmente como 1 (iniciado) ou 0 (disposed), podendo ser alterado pelo usuario final ou pela regra de negocio

#### Nome
```csharp
public string Nome { get; protected set; }
```
##### Summary
Representação amigavel do objeto. Caso seja nulo o nome da classe herdado será informado na propriedade.

#### Id
```csharp
public virtual string Id { get; }
```
##### Summary
Representação alfanumerica e unica do objeto.

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
