# ILGreaterThanOrEquals `class`

## Description
&gt;= || OpCodes.Blt_S || OpCodes.Clt_Un

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Pilhas.Logico
  Propeus.Modulo.IL.Pilhas.Logico.ILGreaterThanOrEquals[[ILGreaterThanOrEquals]]
  Propeus.Modulo.IL.Pilhas.Logico.ILLogico[[ILLogico]]
  end
Propeus.Modulo.IL.Pilhas.Logico.ILLogico --> Propeus.Modulo.IL.Pilhas.Logico.ILGreaterThanOrEquals
```

## Details
### Summary
&gt;= || OpCodes.Blt_S || OpCodes.Clt_Un

### Inheritance
 - [
`ILLogico`
](./ILLogico.md)

### Constructors
#### ILGreaterThanOrEquals [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L16707566)
```csharp
public ILGreaterThanOrEquals(ILBuilderProxy iLBuilderProxy)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |

#### ILGreaterThanOrEquals [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Tabelas/Helper.cs#L181)
```csharp
public ILGreaterThanOrEquals(ILBuilderProxy iLBuilderProxy, ILLabel label)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILBuilderProxy`](../../proxy/ILBuilderProxy.md) | iLBuilderProxy |   |
| [`ILLabel`](../saltos/ILLabel.md) | label |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
