# ILGerador `class`

## Description
Classe para montagem inicial do Assembly

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Geradores
  Propeus.Modulo.IL.Geradores.ILGerador[[ILGerador]]
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
System.IDisposable --> Propeus.Modulo.IL.Geradores.ILGerador
```

## Members
### Properties
#### Internal  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) | [`Modulo`](#modulo) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| [`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) | [`CriarModulo`](#criarmodulo)(`string` nomeModulo) |
| `void` | [`Dispose`](#dispose-22)() |
| `string` | [`ToString`](#tostring)() |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-12)(`bool` disposing) |

## Details
### Summary
Classe para montagem inicial do Assembly

### Inheritance
 - `IDisposable`

### Constructors
#### ILGerador
```csharp
public ILGerador(string nomeAssembly)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeAssembly | Nome do assembly |

##### Summary
Construtor do gerador de IL

### Methods
#### CriarModulo
```csharp
public ILModulo CriarModulo(string nomeModulo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeModulo |   |

#### ToString
```csharp
public override string ToString()
```

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
#### Modulo
```csharp
internal ILModulo Modulo { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
