# ModuloException `class`

## Description
Excecao generica

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Exceptions
  Propeus.Modulo.Abstrato.Exceptions.ModuloException[[ModuloException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Abstrato.Exceptions.ModuloException
```

## Details
### Summary
Excecao generica

### Inheritance
 - `Exception`

### Constructors
#### ModuloException
```csharp
public ModuloException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
