# ModuloConstrutorAusenteException `class`

## Description
Excecao para quando o modulo nao possuir nenhum construtor publico

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Core.Exceptions
  Propeus.Modulo.Core.Exceptions.ModuloConstrutorAusenteException[[ModuloConstrutorAusenteException]]
  Propeus.Modulo.Core.Exceptions.ModuloException[[ModuloException]]
  end
Propeus.Modulo.Core.Exceptions.ModuloException --> Propeus.Modulo.Core.Exceptions.ModuloConstrutorAusenteException
```

## Details
### Summary
Excecao para quando o modulo nao possuir nenhum construtor publico

### Inheritance
 - [
`ModuloException`
](./propeusmodulocoreexceptions-ModuloException.md)

### Constructors
#### ModuloConstrutorAusenteException
```csharp
public ModuloConstrutorAusenteException(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Mensagem do erro |

##### Summary
Construtor padrao

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
