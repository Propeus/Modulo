# ILUlong `class`

## Description
UInt64 || uint64 || OpCodes.Ldc_I4_M1

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
  Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILUlong[[ILUlong]]
  end
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
Propeus.Modulo.IL.Pilhas.ILPilha --> Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILUlong
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `ulong` | [`Valor`](#valor) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Executar`](#executar)() |
| `string` | [`ToString`](#tostring)() |

## Details
### Summary
UInt64 || uint64 || OpCodes.Ldc_I4_M1

### Inheritance
 - [
`ILPilha`
](../../ILPilha.md)

### Constructors
#### ILUlong
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L124)
```csharp
public ILUlong(ILBuilderProxy iLBuilderProxy, ulong valor)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |
| `ulong` | valor |   |

##### Summary
UInt64 || uint64 || OpCodes.Ldc_I4_M1

### Methods
#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L35)
```csharp
public override void Executar()
```

#### ToString
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L13)
```csharp
public override string ToString()
```

### Properties
#### Valor
```csharp
public ulong Valor { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
