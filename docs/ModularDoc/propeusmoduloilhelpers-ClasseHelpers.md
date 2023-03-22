# ClasseHelpers `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.IL.Helpers
  Propeus.Modulo.IL.Helpers.ClasseHelpers[[ClasseHelpers]]
  end
```

## Members
### Methods
#### Public Static methods
| Returns | Name |
| --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | [`CriarClasse`](#criarclasse)([`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) iLGerador, `string` nome, `string` namespace, `Type` tipoBase, `Type``[]` interfaces, [`Token`](./propeusmoduloilenums-Token.md)`[]` token) |
| [`ILDelegate`](./propeusmoduloilgeradores-ILDelegate.md) | [`CriarDelegate`](#criardelegate-12)(`...`) |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | [`CriarProxyClasse`](#criarproxyclasse-12)(`...`) |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | [`ObterClasseProvider`](#obterclasseprovider)([`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) iLModulo, `string` nome, `string` namespace) |
| `dynamic` | [`ObterInstancia`](#obterinstancia-12)(`...`) |
| `Type` | [`ObterTipoGerado`](#obtertipogerado)([`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) iLClasseProvider) |

## Details
### Methods
#### CriarProxyClasse [1/2]
```csharp
public static ILClasseProvider CriarProxyClasse(ILModulo iLGerador, Type classe, Type[] interfaces)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) | iLGerador |   |
| `Type` | classe |   |
| `Type``[]` | interfaces |   |

#### CriarProxyClasse [2/2]
```csharp
public static ILClasseProvider CriarProxyClasse<TClasse>(ILModulo iLGerador, Type[] interfaces)
where TClasse : 
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) | iLGerador |   |
| `Type``[]` | interfaces |   |

#### CriarClasse
```csharp
public static ILClasseProvider CriarClasse(ILModulo iLGerador, string nome, string namespace, Type tipoBase, Type[] interfaces, Token[] token)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) | iLGerador |   |
| `string` | nome |   |
| `string` | namespace |   |
| `Type` | tipoBase |   |
| `Type``[]` | interfaces |   |
| [`Token`](./propeusmoduloilenums-Token.md)`[]` | token |   |

#### CriarDelegate [1/2]
```csharp
public static ILDelegate CriarDelegate(ILModulo ilGerador, Type tipoSaida, string nomeDelegate, ILParametro[] parametros)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) | ilGerador |   |
| `Type` | tipoSaida |   |
| `string` | nomeDelegate |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` | parametros |   |

#### CriarDelegate [2/2]
```csharp
public static ILDelegate CriarDelegate(ILClasseProvider iLClasse, Type tipoSaida, string nomeDelegate, ILParametro[] parametros)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | iLClasse |   |
| `Type` | tipoSaida |   |
| `string` | nomeDelegate |   |
| [`ILParametro`](./propeusmoduloilgeradores-ILParametro.md)`[]` | parametros |   |

#### ObterClasseProvider
```csharp
public static ILClasseProvider ObterClasseProvider(ILModulo iLModulo, string nome, string namespace)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILModulo`](./propeusmoduloilgeradores-ILModulo.md) | iLModulo |   |
| `string` | nome |   |
| `string` | namespace |   |

#### ObterInstancia [1/2]
```csharp
public static dynamic ObterInstancia(ILClasseProvider iLClasse, object[] args)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | iLClasse |   |
| `object``[]` | args |   |

#### ObterInstancia [2/2]
```csharp
public static TInterface ObterInstancia<TInterface>(ILClasseProvider iLClasse, object[] args)
where TInterface : 
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | iLClasse |   |
| `object``[]` | args |   |

#### ObterTipoGerado
```csharp
public static Type ObterTipoGerado(ILClasseProvider iLClasseProvider)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ILClasseProvider`](./propeusmoduloilgeradores-ILClasseProvider.md) | iLClasseProvider |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
