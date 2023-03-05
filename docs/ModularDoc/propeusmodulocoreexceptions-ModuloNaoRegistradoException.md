# ModuloNaoRegistradoException `class`

## Description
Excecao para quando o modulo for criado fora do Gerenciador

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Core.Exceptions
  Propeus.Modulo.Core.Exceptions.ModuloNaoRegistradoException[[ModuloNaoRegistradoException]]
  end
  subgraph System
System.Exception[[Exception]]
  end
System.Exception --> Propeus.Modulo.Core.Exceptions.ModuloNaoRegistradoException
```

## Details
### Summary
Excecao para quando o modulo for criado fora do Gerenciador

### Inheritance
 - `Exception`

### Constructors
#### ModuloNaoRegistradoException
```csharp
public ModuloNaoRegistradoException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
