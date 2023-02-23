# ModuloAssemblyLoadContext `class`

## Description
AssemblyLoadContext customizado

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph Propeus.Modulo.Dinamico
  Propeus.Modulo.Dinamico.ModuloAssemblyLoadContext[[ModuloAssemblyLoadContext]]
  end
  subgraph System.Runtime.Loader
System.Runtime.Loader.AssemblyLoadContext[[AssemblyLoadContext]]
  end
System.Runtime.Loader.AssemblyLoadContext --> Propeus.Modulo.Dinamico.ModuloAssemblyLoadContext
```

## Details
### Summary
AssemblyLoadContext customizado

### Inheritance
 - `AssemblyLoadContext`

### Constructors
#### ModuloAssemblyLoadContext
[*Source code*](https://github.com///blob//src/Propeus.Modulo.Dinamico/ModuloAssemblyLoadContext.cs#L13)
```csharp
public ModuloAssemblyLoadContext()
```
##### Summary
Construtor padrão

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
