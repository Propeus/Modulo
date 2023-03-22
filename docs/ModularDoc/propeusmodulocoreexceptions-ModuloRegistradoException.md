# ModuloRegistradoException `class`

## Description
Excecao para quando o mesmo modulo for registrado mais de uma unica vez

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Core.Exceptions
  Propeus.Modulo.Core.Exceptions.ModuloRegistradoException[[ModuloRegistradoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Core.Exceptions.ModuloRegistradoException
```

## Details
### Summary
Excecao para quando o mesmo modulo for registrado mais de uma unica vez

### Inheritance
 - `Exception`

### Constructors
#### ModuloRegistradoException
```csharp
public ModuloRegistradoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
