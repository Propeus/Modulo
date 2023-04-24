# ModuloConstrutorAusenteException `class`

## Description
Excecao para quando o modulo nao possuir nenhum construtor publico

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Exceptions
  Propeus.Modulo.Abstrato.Exceptions.ModuloConstrutorAusenteException[[ModuloConstrutorAusenteException]]
  Propeus.Modulo.Abstrato.Exceptions.ModuloException[[ModuloException]]
  end
Propeus.Modulo.Abstrato.Exceptions.ModuloException --> Propeus.Modulo.Abstrato.Exceptions.ModuloConstrutorAusenteException
```

## Details
### Summary
Excecao para quando o modulo nao possuir nenhum construtor publico

### Inheritance
 - [
`ModuloException`
](./propeusmoduloabstratoexceptions-ModuloException.md)

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
