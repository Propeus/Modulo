# Gerenciador

Autor: Propeus
Data da ultima atualziacao: 05/03/2023

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