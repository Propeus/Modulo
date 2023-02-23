# ILDecimal `class`

## Description
Decimal || ValueType || OpCodes.Newobj

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
  Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILDecimal[[ILDecimal]]
  end
  subgraph Propeus.Modulo.IL.Pilhas
  Propeus.Modulo.IL.Pilhas.ILPilha[[ILPilha]]
  end
Propeus.Modulo.IL.Pilhas.ILPilha --> Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILDecimal
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`ILNewObj`](../../ILNewObj.md) | [`Constutor`](#constutor) | `get` |
| [`ILBoolean`](./ILBoolean.md) | [`NumeroNegativo`](#numeronegativo) | `get` |
| [`ILSbyte`](./ILSbyte.md) | [`QuantidadePontoFlutuante`](#quantidadepontoflutuante) | `get` |
| [`ILInt`](./ILInt.md) | [`Slot1`](#slot1) | `get` |
| [`ILInt`](./ILInt.md) | [`Slot2`](#slot2) | `get` |
| [`ILInt`](./ILInt.md) | [`Slot3`](#slot3) | `get` |
| `decimal` | [`Valor`](#valor) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Executar`](#executar)() |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose)(`bool` disposing) |

## Details
### Summary
Decimal || ValueType || OpCodes.Newobj

### Inheritance
 - [
`ILPilha`
](../../ILPilha.md)

### Constructors
#### ILDecimal
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tipos/Helper.cs#L80)
```csharp
public ILDecimal(ILBuilderProxy proxy, decimal valor)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../../proxy/ILBuilderProxy.md) | proxy |   |
| `decimal` | valor |   |

##### Summary
Decimal || ValueType || OpCodes.Newobj

### Methods
#### Executar
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tipos/Helper.cs#L16707566)
```csharp
public override void Executar()
```

#### Dispose
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tipos/Helper.cs#L201)
```csharp
protected override void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing |   |

### Properties
#### Valor
```csharp
public decimal Valor { get; }
```

#### Slot1
```csharp
public ILInt Slot1 { get; }
```

#### Slot2
```csharp
public ILInt Slot2 { get; }
```

#### Slot3
```csharp
public ILInt Slot3 { get; }
```

#### NumeroNegativo
```csharp
public ILBoolean NumeroNegativo { get; }
```

#### QuantidadePontoFlutuante
```csharp
public ILSbyte QuantidadePontoFlutuante { get; }
```

#### Constutor
```csharp
public ILNewObj Constutor { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
