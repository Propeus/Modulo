# ModuloDescartadoException `class`

## Description
Excecao para quando o modulo é descartado pelo IDisposable.Dispose ou quando o GC coleta o objeto

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Core.Exceptions
  Propeus.Modulo.Core.Exceptions.ModuloDescartadoException[[ModuloDescartadoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Core.Exceptions.ModuloDescartadoException
```

## Details
### Summary
Excecao para quando o modulo é descartado pelo IDisposable.Dispose ou quando o GC coleta o objeto

### Inheritance
 - `Exception`

### Constructors
#### ModuloDescartadoException
```csharp
public ModuloDescartadoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
