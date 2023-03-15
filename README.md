# Propeus.Modulo

[![.NET](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml) [![SonarCloud analysis](https://github.com/Propeus/Modulo/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/sonarcloud.yml)

Este projeto tem como objetivo criar e gerenciar módulos .NET dinamicamente, permitindo que sejam modificados, melhorados ou removidos em tempo de execução, sem a necessidade de realizar um CI/CD ou mesmo paralisar um sistema para realizar o deploy. Em poucas palavras, este projeto tem como objetivo ser um "Docker" para .NET.

O projeto não requer instalação, pois é adicionado como componente e inicializado junto com a aplicação web, console ou worker service.

## Licença

Este projeto é licenciado sob a licença LSUNCA (Licença de Software de Uso Não-Comercial e Atribuição). Consulte o arquivo LICENSE para obter mais informações.

## Como Contribuir

Para contribuir com este projeto, siga as instruções abaixo:

1. Faça um fork deste repositório
2. Crie uma branch para suas alterações (`git checkout -b feature/nome-da-feature`)
3. Faça suas alterações e adicione seus commits (`git commit -am 'Adicionando uma nova feature'`)
4. Faça um push para a branch (`git push origin feature/nome-da-feature`)
5. Abra um pull request para a branch `master` deste repositório

## Estado do Projeto

Este projeto está em fase de prova de conceito (PoC) e pode ser testado usando o projeto "Propeus.Modulo.Playgroud". Estamos trabalhando para desenvolver o projeto e testá-lo completamente antes de disponibilizá-lo para uso. Fique à vontade para contribuir e nos ajudar a torná-lo uma realidade!

## Exemplos

Aqui estão alguns exemplos de como usar o projeto "Propeus.Modulo.Core":

### Exemplo para criar um módulo e inicializar uma instância dele

Crie uma interface de contrato:

```cs
[ModuloContrato(typeof(ModuloSimples))]
public interface IModuloContrato : IModulo
{
    string OlaMundo();
}
```
Crie uma classe que herde de ModuloBase e da interface de contrato:

```cs
[Modulo]
public class ModuloSimples : ModuloBase, IModuloContrato
{
    public ModuloSimples(IGerenciador gerenciador, bool instanciaUnica = false) : base(gerenciador, instanciaUnica)
    {
    }

    public string OlaMundo()
    {
        return "Ola Mundo";
    }

    public override string ToString()
    {
        return this.OlaMundo();
    }
}
```

Crie uma instância do módulo pelo gerenciador e chame o método .ToString() do objeto:
```cs
var modulo = Propeus.Modulo.Core.Gerenciador.Atual.Criar<IModuloContrato>();
Console.WriteLine(modulo.ToString());
```

### Exemplo para remover um módulo
Para remover um módulo, basta usar o método Remover<T>() do gerenciador de módulos. Por exemplo, para remover o módulo ModuloSimples criado no exemplo anterior, você pode fazer o seguinte:

```cs
Propeus.Modulo.Core.Gerenciador.Atual.Remover<ModuloSimples>();
```

Lembre-se de que, se o módulo estiver em uso, você deve parar de usá-lo antes de removê-lo