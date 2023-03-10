# Constantes `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Geradores
  Propeus.Modulo.IL.Geradores.Constantes[[Constantes]]
  end
```

## Members
### Methods
#### Public Static methods
| Returns | Name |
| --- | --- |
| `string` | [`GerarNome`](#gerarnome)(`string` const) |
| `string` | [`GerarNomeCampo`](#gerarnomecampo)(`string` nomeClasse) |
| `string` | [`GerarNomeMetodo`](#gerarnomemetodo)(`string` nomeClasse) |
| `string` | [`GerarNomeModulo`](#gerarnomemodulo)() |
| `string` | [`GerarNomeParametro`](#gerarnomeparametro)(`string` nomeMetodo) |
| `string` | [`GerarNomePropriedade`](#gerarnomepropriedade)(`string` nomeClasse) |
| `string` | [`GerarNomeVariavel`](#gerarnomevariavel)(`string` nomeClasse) |

## Details
### Methods
#### GerarNomeCampo
```csharp
public static string GerarNomeCampo(string nomeClasse)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeClasse |   |

#### GerarNomeModulo
```csharp
public static string GerarNomeModulo()
```

#### GerarNome
```csharp
public static string GerarNome(string const)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | const |   |

#### GerarNomeMetodo
```csharp
public static string GerarNomeMetodo(string nomeClasse)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeClasse |   |

#### GerarNomeParametro
```csharp
public static string GerarNomeParametro(string nomeMetodo)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeMetodo |   |

#### GerarNomePropriedade
```csharp
public static string GerarNomePropriedade(string nomeClasse)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeClasse |   |

#### GerarNomeVariavel
```csharp
public static string GerarNomeVariavel(string nomeClasse)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | nomeClasse |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
