# ILPilha `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
  subgraph Propeus.Modulo.IL.Interfaces
  Propeus.Modulo.IL.Interfaces.IILPilha[[IILPilha]]
  class Propeus.Modulo.IL.Interfaces.IILPilha interfaceStyle;
  Propeus.Modulo.IL.Interfaces.IILExecutor[[IILExecutor]]
  class Propeus.Modulo.IL.Interfaces.IILExecutor interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
Propeus.Modulo.IL.Interfaces.IILPilha --> Propeus.Modulo.IL.Pilhas.ILPilha
Propeus.Modulo.IL.Interfaces.IILExecutor --> Propeus.Modulo.IL.Interfaces.IILPilha
System.IDisposable --> Propeus.Modulo.IL.Interfaces.IILPilha
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `OpCode` | [`Code`](#code) | `get` |
| [`ILBuilderProxy`](../proxy/ILBuilderProxy.md) | [`Proxy`](#proxy) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-22)() |
| `void` | [`Executar`](#executar)() |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-12)(`bool` disposing) |

## Details
### Inheritance
 - [
`IILPilha`
](../interfaces/IILPilha.md)
 - [
`IILExecutor`
](../interfaces/IILExecutor.md)
 - `IDisposable`

### Constructors
#### ILPilha [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Dinamico/Properties/Resources.Designer.cs#L139)
```csharp
public ILPilha(ILBuilderProxy iLBuilderProxy, OpCode opCode)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |
| `OpCode` | opCode |   |

#### ILPilha [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Dinamico/Properties/Resources.Designer.cs#L148)
```csharp
public ILPilha(ILBuilderProxy iLBuilderProxy, OpCode opCode, object valor)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |
| `OpCode` | opCode |   |
| `object` | valor |   |

### Methods
#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Dinamico/Properties/Resources.Designer.cs#L184)
```csharp
public virtual void Executar()
```

#### Dispose [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Dinamico/Properties/Resources.Designer.cs#L193)
```csharp
protected virtual void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing |   |

#### Dispose [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Dinamico/Properties/Resources.Designer.cs#L202)
```csharp
public virtual void Dispose()
```

### Properties
#### Proxy
```csharp
public virtual ILBuilderProxy Proxy { get; }
```

#### Code
```csharp
public virtual OpCode Code { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
