# Gerenciador

## Informacoes basicas
- Autor: Propeus
- Data da ultima atualziacao: 05/03/2023

## Descricao

A classe `Propeus.Modulo.Core.Gerenciador` e responsavel por gerenciar o ciclo de vida de um modulo, ele possui as seguintes funcionalidades

- Criar um módulo
- Remover um módulo
- Reciclar um módulo
- Obter um módulo existente
- Obter informações de um módulo existente

## Uso

Para criar uma instância do gerenciador, utilize o seguinte trecho de código:

```cs
using (IGerenciador gerenciador = Propeus.Modulo.Core.Gerenciador.Atual)
{
    //Escreva o seu codigo aqui
}
```

## Metodos
###  `Criar(string nomeModulo)`
#### Descricao
Cria uma nova instancia do modulo buscando o tipo pelo nome
#### Parametros
- `nomeModulo`:
  - Tipo: `string` 
  - Descricao: Nome do modulo a ser criado
  - Obrigatorio: `Sim`

#### Retorno
- `IModulo` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `TipoModuloInvalidoException`:
  - Tipo do modulo invalido
  - Tipo do modulo nao herdado de `IModulo`
  - Tipo do modulo nao possui o atributo `ModuloAttribute`
  - Parametro do construtor nao e um modulo valido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`
- `TipoModuloNaoEncontradoException`: 
  - Tipo nao encontrado pelo nome no atributo `ModuloContratoAttribute`
  - Tipo ausente no atributo `ModuloContratoAttribute`
- `TipoModuloAmbiguoException`: Mais um tipo de mesmo nome
- `ModuloInstanciaUnicaException`: Criacao de mais de uma instancia de modulo definido como instancia unica
- `ModuloConstrutorAusenteException`: Construtor ausente no modulo

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo =  (ICalculadoraModuloContrato)gerenciador.Criar("ICalculadoraModuloContrato");
                Console.WriteLine(modulo.Calcular(1,1));
            }
        }
    }
}
```

###  `Criar<T>()`
#### Descricao
Cria uma nova instancia do modulo
#### Parametros
- `T`:
  - Tipo: `IModulo` 
  - Descricao: Tipo do modulo a ser criado
  - Obrigatorio: `Sim`

#### Retorno
- `T` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `TipoModuloInvalidoException`:
  - Tipo do modulo invalido
  - Tipo do modulo nao herdado de `IModulo`
  - Tipo do modulo nao possui o atributo `ModuloAttribute`
  - Parametro do construtor nao e um modulo valido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`
- `TipoModuloNaoEncontradoException`: 
  - Tipo nao encontrado pelo nome no atributo `ModuloContratoAttribute`
  - Tipo ausente no atributo `ModuloContratoAttribute`
- `ModuloInstanciaUnicaException`: Criacao de mais de uma instancia de modulo definido como instancia unica
- `ModuloConstrutorAusenteException`: Construtor ausente no modulo

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine(modulo.Calcular(1,1));
            }
        }
    }
}
```

### `Criar(Type modulo)`
#### Descricao
Cria uma nova instancia do modulo
#### Parametros
- `modulo`:
  - Tipo: `Type` 
  - Descricao: Cria uma nova instancia do modulo usando o tipo do parametro
  - Obrigatorio: `Sim`

#### Retorno
- `IModulo` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `TipoModuloInvalidoException`:
  - Tipo do modulo invalido
  - Tipo do modulo nao herdado de `IModulo`
  - Tipo do modulo nao possui o atributo `ModuloAttribute`
  - Parametro do construtor nao e um modulo valido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`
- `TipoModuloNaoEncontradoException`: 
  - Tipo nao encontrado pelo nome no atributo `ModuloContratoAttribute`
  - Tipo ausente no atributo `ModuloContratoAttribute`
- `ModuloInstanciaUnicaException`: Criacao de mais de uma instancia de modulo definido como instancia unica
- `ModuloConstrutorAusenteException`: Construtor ausente no modulo

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
                Console.WriteLine(modulo.Calcular(1,1));
            }
        }
    }
}
```

###  `Existe(Type modulo)`
#### Descricao
Verifica se existe alguma instancia do tipo no gerenciador
#### Parametros
- `modulo`:
  - Tipo: `Type` 
  - Descricao: Tipo da instancia do modulo a ser verificado
  - Obrigatorio: `Sim`

#### Retorno
- `bool` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `TipoModuloInvalidoException`:
  - Tipo do modulo invalido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
                Console.WriteLine(gerenciador.Existe(typeof(ICalculadoraModuloContrato)));
            }
        }
    }
}
```
###  `Existe(IModulo modulo)`
#### Descricao
Verifica se a instancia do modulo existe no genrenciador
#### Parametros
- `modulo`:
  - Tipo: `IModulo` 
  - Descricao: A instancia do modulo
  - Obrigatorio: `Sim`

#### Retorno
- `bool` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
                Console.WriteLine(gerenciador.Existe(modulo));
            }
        }
    }
}
```
###  `Existe(string id)`
#### Descricao
Verifica se existe alguma instancia com o id no gerenciador
#### Parametros
- `modulo`:
  - Tipo: `string` 
  - Descricao: Identificação unica do modulo
  - Obrigatorio: `Sim`

#### Retorno
- `bool` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
                Console.WriteLine(gerenciador.Existe(modulo.Id));
            }
        }
    }
}
```

### `ObterInfo(Type modulo)`
#### Descricao
Obtem o IModuloTipo do tipo do modulo
#### Parametros
- `modulo`:
  - Tipo: `Type` 
  - Descricao: Qualquer tipo herdado de IModulo
  - Obrigatorio: `Sim`

#### Retorno
- `IModuloTipo` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `TipoModuloInvalidoException`:
  - Tipo do modulo invalido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
                Console.WriteLine((gerenciador as IGerenciadorInformacao).ObterInfo(typeof(ICalculadoraModuloContrato)));
            }
        }
    }
}
```

### `ObterInfo<T>()`
#### Descricao
Obtem o IModuloTipo do tipo do modulo
#### Parametros
- `T`:
  - Tipo: `IModulo` 
  - Descricao: Qualquer tipo herdado de IModulo
  - Obrigatorio: `Sim`

#### Retorno
- `IModuloTipo` 

#### Excecoes
- `TipoModuloInvalidoException`:
  - Tipo do modulo invalido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine((gerenciador as IGerenciadorInformacao).ObterInfo<ICalculadoraModuloContrato>());
            }
        }
    }
}
```

### `ObterInfo(string id)`
#### Descricao
Obtem  o IModuloTipo do modulo pelo id
#### Parametros
- `id`:
  - Tipo: `string` 
  - Descricao: Identificação unica do modulo
  - Obrigatorio: `Sim`

#### Retorno
- `IModuloTipo` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine((gerenciador as IGerenciadorInformacao).ObterInfo(modulo.Id));
            }
        }
    }
}
```

### `Obter<T>()`
#### Descricao
Obtem a instancia de T caso exista
#### Parametros
- `T`:
  - Tipo: `IModulo` 
  - Descricao: Qualquer tipo herdado de IModulo
  - Obrigatorio: `Sim`

#### Retorno
- `T` 

#### Excecoes
- `TipoModuloInvalidoException`: Tipo do modulo invalido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine(gerenciador.Obter<ICalculadoraModuloContrato>());
            }
        }
    }
}
```

### `Obter(Type modulo)`
#### Descricao
Obtem a instancia do tipo informado caso exista
#### Parametros
- `modulo`:
  - Tipo: `Type` 
  - Descricao: Qualquer tipo herdado de IModulo
  - Obrigatorio: `Sim`

#### Retorno
- `IModulo` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `ModuloDescartadoException`: Instancia do modulo foi coletado pelo `GC` ou acionou o `Dispose()`
- `TipoModuloInvalidoException`: Tipo do modulo invalido
- `ModuloContratoNaoEncontratoException`: Tipo da interface de contrato nao possui o atributo `ModuloContratoAttribute`
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
                Console.WriteLine(gerenciador.Obter(typeof(ICalculadoraModuloContrato)));
            }
        }
    }
}
```

### `Obter(string id)`
#### Descricao
Obtem a instancia do tipo informado caso exista
#### Parametros
- `id`:
  - Tipo: `string` 
  - Descricao: Identificação unica do modulo
  - Obrigatorio: `Sim`

#### Retorno
- `IModulo` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado
- `ModuloDescartadoException`: Instancia do modulo foi coletado pelo `GC` ou acionou o `Dispose()`

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = (ICalculadoraModuloContrato)gerenciador.Criar(typeof(ICalculadoraModuloContrato));
                Console.WriteLine(gerenciador.Obter(modulo.Id));
            }
        }
    }
}
```

### `Remover<T>()`
#### Descricao
Remove o modulo instanciado
#### Parametros
- `T`:
  - Tipo: `IModulo` 
  - Descricao: Qualquer tipo herdado de IModulo
  - Obrigatorio: `Sim`

#### Retorno
- `void` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine(gerenciador.Remover(modulo));
            }
        }
    }
}
```

### `Remover(string id)`
#### Descricao
Remove o modulo instanciado
#### Parametros
- `id`:
  - Tipo: `string` 
  - Descricao: Identificação unica do modulo
  - Obrigatorio: `Sim`

#### Retorno
- `void` 

#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado
- `ModuloDescartadoException`: Instancia do modulo foi coletado pelo `GC` ou acionou o `Dispose()`

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine(gerenciador.Remover(modulo.Id));
            }
        }
    }
}
```

### `RemoverTodos()`
#### Descricao
Remove todos os modulos
#### Parametros
N/A
#### Retorno
- `void` 
#### Excecoes
N/A

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine(gerenciador.RemoverTodos());
            }
        }
    }
}
```

### `Reciclar<T>(T modulo)`
#### Descricao
Realiza uma reciclagem do modulo 
#### Parametros
- `T`:
  - Tipo: `IModulo` 
  - Descricao: Qualquer tipo herdado de IModulo
  - Obrigatorio: `Sim`
#### Retorno
- `T` 
#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine(gerenciador.Reciclar<ICalculadoraModuloContrato>());
            }
        }
    }
}
```

### `Reciclar(string id)`
#### Descricao
Realiza uma reciclagem do modulo 
#### Parametros
- `id`:
  - Tipo: `string` 
  - Descricao: Identificação unica do modulo
  - Obrigatorio: `Sim`
#### Retorno
- `IModulo` 
#### Excecoes
- `ArgumentNullException`: Parametro nulo
- `ModuloNaoEncontradoException`: Instancia do modulo nao foi encontrado
- `ModuloDescartadoException`: Instancia do modulo foi coletado pelo `GC` ou acionou o `Dispose()`

#### Exemplo
```cs
using System;
using Propeus.Modulo.Abstrato.Atributos;
using Propeus.Modulo.Core.Gerenciador;

namespace Propeus.Modulo.Exemplo
{
    [Modulo]
    public class CalculadoraModulo : ICalculadoraModuloContrato
    {
        public ModuloTesteA(IGerenciador gerenciador) : base(gerenciador, false)
        {
            
        }
        
        public int Calcular(int a, int b)
        {
            return a+b;
        }
    }

    [ModuloContrato(typeof(CalculadoraModulo))]
    public interface ICalculadoraModuloContrato : IModulo
    {
        public int Calcular(int a, int b);
    }
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            using(Gerenciador gerenciador = Gereciador.Atual)
            {
                ICalculadoraModuloContrato modulo = gerenciador.Criar<ICalculadoraModuloContrato>();
                Console.WriteLine(gerenciador.Reciclar(modulo.Id));
            }
        }
    }
}
```