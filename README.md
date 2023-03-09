# Propeus.Modulo

[![.NET](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/dotnet.yml) [![SonarCloud analysis](https://github.com/Propeus/Modulo/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/Propeus/Modulo/actions/workflows/sonarcloud.yml)

Este projeto tem como objetivo criar e gerenciar m�dulos .NET dinamicamente, permitindo que sejam modificados, melhorados ou removidos em tempo de execu��o, sem a necessidade de realizar um CI/CD ou mesmo paralisar um sistema para realizar o deploy. Em poucas palavras, este projeto tem como objetivo ser um "Docker" para .NET.

O projeto n�o requer instala��o, pois � adicionado como componente e inicializado junto com a aplica��o web, console ou worker service.

## Licen�a

Este projeto � licenciado sob a licen�a LSUNCA (Licen�a de Software de Uso N�o-Comercial e Atribui��o). Consulte o arquivo LICENSE para obter mais informa��es.

## Como Contribuir

Para contribuir com este projeto, siga as instru��es abaixo:

1. Fa�a um fork deste reposit�rio
2. Crie uma branch para suas altera��es (`git checkout -b feature/nome-da-feature`)
3. Fa�a suas altera��es e adicione seus commits (`git commit -am 'Adicionando uma nova feature'`)
4. Fa�a um push para a branch (`git push origin feature/nome-da-feature`)
5. Abra um pull request para a branch `master` deste reposit�rio

## Estado do Projeto

Este projeto est� em fase de prova de conceito (PoC) e pode ser testado usando o projeto "Propeus.Modulo.Playgroud". Estamos trabalhando para desenvolver o projeto e test�-lo completamente antes de disponibiliz�-lo para uso. Fique � vontade para contribuir e nos ajudar a torn�-lo uma realidade!

## Exemplos

Aqui est�o alguns exemplos de como usar o projeto "Propeus.Modulo.Core":

### Exemplo para criar um m�dulo e inicializar uma inst�ncia dele

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

Crie uma inst�ncia do m�dulo pelo gerenciador e chame o m�todo .ToString() do objeto:
```cs
var modulo = Propeus.Modulo.Core.Gerenciador.Atual.Criar<IModuloContrato>();
Console.WriteLine(modulo.ToString());
```

### Exemplo para remover um m�dulo
Para remover um m�dulo, basta usar o m�todo Remover<T>() do gerenciador de m�dulos. Por exemplo, para remover o m�dulo ModuloSimples criado no exemplo anterior, voc� pode fazer o seguinte:

```cs
Propeus.Modulo.Core.Gerenciador.Atual.Remover<ModuloSimples>();
```

Lembre-se de que, se o m�dulo estiver em uso, voc� deve parar de us�-lo antes de remov�-lo