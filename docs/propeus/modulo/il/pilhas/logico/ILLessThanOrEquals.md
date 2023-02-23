# ILLessThanOrEquals `class`

## Description
&lt;= || OpCodes.Bgt_Un_S || OpCodes.Cgt_Un

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Logico
  Propeus.Modulo.IL.Pilhas.Logico.ILLessThanOrEquals[[ILLessThanOrEquals]]
  Propeus.Modulo.IL.Pilhas.Logico.ILLogico[[ILLogico]]
  end
Propeus.Modulo.IL.Pilhas.Logico.ILLogico --> Propeus.Modulo.IL.Pilhas.Logico.ILLessThanOrEquals
```

## Details
### Summary
&lt;= || OpCodes.Bgt_Un_S || OpCodes.Cgt_Un

### Inheritance
 - [
`ILLogico`
](./ILLogico.md)

### Constructors
#### ILLessThanOrEquals [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L233)
```csharp
public ILLessThanOrEquals(ILBuilderProxy iLBuilderProxy)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |

#### ILLessThanOrEquals [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L242)
```csharp
public ILLessThanOrEquals(ILBuilderProxy iLBuilderProxy, ILLabel label)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |
| [`ILLabel`](../saltos/ILLabel.md) | label |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
