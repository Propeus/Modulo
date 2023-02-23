# TaskJob `class`

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Abstrato.Util.Thread
  Propeus.Modulo.Abstrato.Util.Thread.TaskJob[[TaskJob]]
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
System.IDisposable --> Propeus.Modulo.Abstrato.Util.Thread.TaskJob
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `int` | [`Aguardando`](#aguardando) | `get` |
| `int` | [`Completado`](#completado) | `get` |
| `int` | [`EmExecucao`](#emexecucao) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `Task` | [`AddJob`](#addjob)(`Action`&lt;`object`&gt; action, `TimeSpan` period, `string` nomeJob) |
| `void` | [`Dispose`](#dispose-22)() |
| `bool` | [`IsCompleted`](#iscompleted)() |
| `string` | [`ToString`](#tostring)() |
| `string` | [`ToStringRunning`](#tostringrunning)() |
| `Task` | [`WaitAll`](#waitall)() |

#### Protected  methods
| Returns | Name |
| --- | --- |
| `void` | [`Dispose`](#dispose-12)(`bool` disposing) |

## Details
### Inheritance
 - `IDisposable`

### Constructors
#### TaskJob
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L25)
```csharp
public TaskJob(int threads)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `int` | threads |   |

### Methods
#### WaitAll
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L49)
```csharp
public Task WaitAll()
```

#### IsCompleted
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L55)
```csharp
public bool IsCompleted()
```

#### AddJob
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L16707566)
```csharp
public Task AddJob(Action<object> action, TimeSpan period, string nomeJob)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `Action`&lt;`object`&gt; | action |   |
| `TimeSpan` | period |   |
| `string` | nomeJob |   |

#### ToStringRunning
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L96)
```csharp
public string ToStringRunning()
```

#### ToString
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L112)
```csharp
public override string ToString()
```

#### Dispose [1/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L124)
```csharp
protected virtual void Dispose(bool disposing)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | disposing |   |

#### Dispose [2/2]
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Abstrato/Util/Thread/TaskJob.cs#L153)
```csharp
public virtual void Dispose()
```

### Properties
#### EmExecucao
```csharp
public int EmExecucao { get; }
```

#### Completado
```csharp
public int Completado { get; }
```

#### Aguardando
```csharp
public int Aguardando { get; }
```

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
