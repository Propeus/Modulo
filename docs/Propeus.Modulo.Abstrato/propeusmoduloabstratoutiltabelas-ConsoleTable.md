# ConsoleTable `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Util.Tabelas
  Propeus.Modulo.Abstrato.Util.Tabelas.ConsoleTable[[ConsoleTable]]
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `Type``[]` | [`ColumnTypes`](#columntypes) | `get` |
| `IList`&lt;`object`&gt; | [`Columns`](#columns) | `get, set` |
| [`ConsoleTableOptions`](./propeusmoduloabstratoutiltabelas-ConsoleTableOptions) | [`Options`](#options) | `get, protected set` |
| `IList`&lt;`object``[]`&gt; | [`Rows`](#rows) | `get, protected set` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| [`ConsoleTable`](propeusmoduloabstratoutiltabelas-ConsoleTable) | [`AddColumn`](#addcolumn)(`IEnumerable`&lt;`string`&gt; names) |
| [`ConsoleTable`](propeusmoduloabstratoutiltabelas-ConsoleTable) | [`AddRow`](#addrow)(`object``[]` values) |
| [`ConsoleTable`](propeusmoduloabstratoutiltabelas-ConsoleTable) | [`Configure`](#configure)(`Action`&lt;[`ConsoleTableOptions`](./propeusmoduloabstratoutiltabelas-ConsoleTableOptions)&gt; action) |
| `string` | [`ToMarkDownString`](#tomarkdownstring)() |
| `string` | [`ToMinimalString`](#tominimalstring)() |
| `string` | [`ToString`](#tostring)() |
| `string` | [`ToStringAlternative`](#tostringalternative)() |
| `void` | [`Write`](#write)([`Format`](./propeusmoduloabstratoutiltabelas-Format) format) |

#### Public Static methods
| Returns | Name |
| --- | --- |
| [`ConsoleTable`](propeusmoduloabstratoutiltabelas-ConsoleTable) | [`From`](#from)(`IEnumerable`&lt;`T`&gt; values) |

## Details
### Constructors
#### ConsoleTable [1/2]
```csharp
public ConsoleTable(string[] columns)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string``[]` | columns |   |

#### ConsoleTable [2/2]
```csharp
public ConsoleTable(ConsoleTableOptions options)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`ConsoleTableOptions`](./propeusmoduloabstratoutiltabelas-ConsoleTableOptions) | options |   |

### Methods
#### AddColumn
```csharp
public ConsoleTable AddColumn(IEnumerable<string> names)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;`string`&gt; | names |   |

#### AddRow
```csharp
public ConsoleTable AddRow(object[] values)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object``[]` | values |   |

#### Configure
```csharp
public ConsoleTable Configure(Action<ConsoleTableOptions> action)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `Action`&lt;[`ConsoleTableOptions`](./propeusmoduloabstratoutiltabelas-ConsoleTableOptions)&gt; | action |   |

#### From
```csharp
public static ConsoleTable From<T>(IEnumerable<T> values)
where T : 
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;`T`&gt; | values |   |

#### ToString
```csharp
public override string ToString()
```

#### ToMarkDownString
```csharp
public string ToMarkDownString()
```

#### ToMinimalString
```csharp
public string ToMinimalString()
```

#### ToStringAlternative
```csharp
public string ToStringAlternative()
```

#### Write
```csharp
public void Write(Format format)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`Format`](./propeusmoduloabstratoutiltabelas-Format) | format |   |

### Properties
#### Columns
```csharp
public IList<object> Columns { get; set; }
```

#### Rows
```csharp
public IList<object> Rows { get; protected set; }
```

#### Options
```csharp
public ConsoleTableOptions Options { get; protected set; }
```

#### ColumnTypes
```csharp
public Type ColumnTypes { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
