# ILUint `class`

## Description
UInt32 || uint32 || OpCodes.Ldc_I4_M1

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
  Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILUint[[ILUint]]
  end
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
Propeus.Modulo.IL.Pilhas.ILPilha --> Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILUint
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `uint` | [`Valor`](#valor) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Executar`](#executar)() |
| `string` | [`ToString`](#tostring)() |

## Details
### Summary
UInt32 || uint32 || OpCodes.Ldc_I4_M1

### Inheritance
 - [
`ILPilha`
](../../ILPilha.md)

### Constructors
#### ILUint
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L55)
```csharp
public ILUint(ILBuilderProxy iLBuilderProxy, uint valor)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |
| `uint` | valor |   |

##### Summary
UInt32 || uint32 || OpCodes.Ldc_I4_M1

### Methods
#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L96)
```csharp
public override void Executar()
```

#### ToString
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L112)
```csharp
public override string ToString()
```

### Properties
#### Valor
```csharp
public uint Valor { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
