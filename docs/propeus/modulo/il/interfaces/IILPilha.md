# IILPilha `interface`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Interfaces
  Propeus.Modulo.IL.Interfaces.IILPilha[[IILPilha]]
  class Propeus.Modulo.IL.Interfaces.IILPilha interfaceStyle;
  Propeus.Modulo.IL.Interfaces.IILExecutor[[IILExecutor]]
  class Propeus.Modulo.IL.Interfaces.IILExecutor interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
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

## Details
### Inheritance
 - [
`IILExecutor`
](./IILExecutor.md)
 - `IDisposable`

### Properties
#### Code
```csharp
public OpCode Code { get; }
```

#### Proxy
```csharp
public ILBuilderProxy Proxy { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
