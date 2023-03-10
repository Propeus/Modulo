# IGerenciadorArgumentos `interface`

## Description
Interface para criar instancias de modulo passando argumentos

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Interfaces
  Propeus.Modulo.Abstrato.Interfaces.IGerenciadorArgumentos[[IGerenciadorArgumentos]]
  class Propeus.Modulo.Abstrato.Interfaces.IGerenciadorArgumentos interfaceStyle;
  Propeus.Modulo.Abstrato.Interfaces.IGerenciador[[IGerenciador]]
  class Propeus.Modulo.Abstrato.Interfaces.IGerenciador interfaceStyle;
  Propeus.Modulo.Abstrato.Interfaces.IBaseModelo[[IBaseModelo]]
  class Propeus.Modulo.Abstrato.Interfaces.IBaseModelo interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
Propeus.Modulo.Abstrato.Interfaces.IGerenciador --> Propeus.Modulo.Abstrato.Interfaces.IGerenciadorArgumentos
Propeus.Modulo.Abstrato.Interfaces.IBaseModelo --> Propeus.Modulo.Abstrato.Interfaces.IGerenciador
System.IDisposable --> Propeus.Modulo.Abstrato.Interfaces.IBaseModelo
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `T` | [`Criar`](#criar-13)(`...`) |

## Details
### Summary
Interface para criar instancias de modulo passando argumentos

### Inheritance
 - [
`IGerenciador`
](./propeusmoduloabstratointerfaces-IGerenciador.md)
 - [
`IBaseModelo`
](./propeusmoduloabstratointerfaces-IBaseModelo.md)
 - `IDisposable`

### Methods
#### Criar [1/3]
```csharp
public T Criar<T>(object[] args)
where T : IModulo
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object``[]` | args |   |

#### Criar [2/3]
```csharp
public IModulo Criar(Type modulo, object[] args)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `Type` | modulo | Tipo do modulo |
| `object``[]` | args | Qualquer argumento necessário para o modulo |

##### Summary
Cria uma nova instancia do modulo usando o tipo do parametro `modulo`

##### Returns
[IModulo](./propeusmoduloabstratointerfaces-IModulo.md)

#### Criar [3/3]
```csharp
public IModulo Criar(string nomeModulo, object[] args)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeModulo | Nome do modulo |
| `object``[]` | args | Qualquer argumento necessário para o modulo |

##### Summary
Cria uma nova instancia do modulo buscando o tipo pelo nome

##### Returns
[IModulo](./propeusmoduloabstratointerfaces-IModulo.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
