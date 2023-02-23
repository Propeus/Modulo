# ILByte `class`

## Description
Byte || uint8 || OpCodes.Ldc_I4

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos
  Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILByte[[ILByte]]
  Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILInt[[ILInt]]
  end
Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILInt --> Propeus.Modulo.IL.Pilhas.Tipos.TiposPrimitivos.ILByte
```

## Details
### Summary
Byte || uint8 || OpCodes.Ldc_I4

### Inheritance
 - [
`ILInt`
](./ILInt.md)

### Constructors
#### ILByte
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Dinamico/ModuloInformacao.cs#L162)
```csharp
public ILByte(ILBuilderProxy proxy, byte valor)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../../proxy/ILBuilderProxy.md) | proxy |   |
| `byte` | valor |   |

##### Summary
Byte || uint8 || OpCodes.Ldc_I4

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
