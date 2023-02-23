# ILLong `class`

## Description
Int64 || int64 || OpCodes.Ldc_I8

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
  Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILLong[[ILLong]]
  end
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
Propeus.Modulo.IL.Pilhas.ILPilha --> Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILLong
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `long` | [`Valor`](#valor) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Executar`](#executar)() |
| `string` | [`ToString`](#tostring)() |

## Details
### Summary
Int64 || int64 || OpCodes.Ldc_I8

### Inheritance
 - [
`ILPilha`
](../../ILPilha.md)

### Constructors
#### ILLong
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/LimitedConcurrencyLevelTaskScheduler.cs#L45)
```csharp
public ILLong(ILBuilderProxy proxy, long valor)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../../proxy/ILBuilderProxy.md) | proxy |   |
| `long` | valor |   |

##### Summary
Int64 || int64 || OpCodes.Ldc_I8

### Methods
#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/LimitedConcurrencyLevelTaskScheduler.cs#L104)
```csharp
public override void Executar()
```

#### ToString
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/LimitedConcurrencyLevelTaskScheduler.cs#L129)
```csharp
public override string ToString()
```

### Properties
#### Valor
```csharp
public long Valor { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
