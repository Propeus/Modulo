## Propeus.Modulo
Este projeto tem como objetivo facilitar o gerenciamento de modulo .NET e aplicar o conceito de DI, permitindo a injeção de dependencia
em tempo de execução.

## Licença

Os principais projetos são licenciados da seguinte maneira


|            Projeto           | Licenca |
|:----------------------------:|:-------:|
| Propeus.Modulo.Util          |   MIT   |
| Propeus.Modulo.Abstrato      |   MIT   |
| Propeus.Modulo.Core          |  LSUNCA |
| Propeus.Modulo.Dinamico      |  LSUNCA |
| Propeus.Modulo.IL            |   MIT   |
| Propeus.Modulo.WorkerService |   MIT   |
| Propeus.Modulo.Console       |   MIT   |
| Propeus.Modulo.CLI           |   MIT   |

Os projetos `Propeus.Modulo.Core` e `Propeus.Modulo.Dinamico` estão sob a licença `LSUNCA` para que todos possam usar de forma
**gratuita** tanto o projeto quanto as suas modificacoes e derivados.

Sendo assim qualquer empresa pode comercializar seus modulos más os gerenciadores e seu codigo-fonte devem ser distribuidos de forma **gratuita**,

É permitido a comercialização de gerenciadores que são construidos sem qualquer uso dos projetos `Propeus.Modulo.Core`, `Propeus.Modulo.Dinamico`,
suas modificaçoes e derivados.

\* As licenças podem ser alteradas conforme for necessario para manter a **gratuidade** e a liberdade de usar o codigo-fonte

## Planos

Os proximos passos deste projeto é finalizar o projeto `Propeus.Modulo.Hosting` permitindo a criação e renderização de views dinamicamente.

Após este passo, os seguintes passos estão na lista para serem executados:

1. Otimizar o codigo-fonte
2. Documentar o restante do codigo-fonte
3. Tonar o projeto como um MVP (Menor Produto Viavel)
4. Criar exemplos mais complexos e funcionais (Jogos de RPG em console, projeto web para testar modulos etc)
5. Internacionalizar os nomes de classe, interfaces, propriedades, metodos, eventos para o ingles
6. Criar documentação em ingles
7. Criar modulos para aumentar a capacidade de uso do projeto (Conexoes com cache, banco de dados, comunicacao entre programas, segurançla etc)

## Perguntas frequentes

> Existe uma documentação técnica sobre o funcinamento deste projeto?

Existe uma documentação auto-gerado baseado na documentação em codigo.

> Em que fase está este projeto?

Atuamente esta na fase de PoC (Prova de Conceito).

> Posso uar em produção?

Pode por sua conta e risco

> Por que este projeto nao usa componentes de terceiros?

A idéia ptincipal deste projeto é ser totalmente modular e possuir o minimo de dependencia possivel, então não faria sentido
adicionar componentes de proxy dinamico e injeção de dependencia de terceiros.

\* Esta pagina pode ser alterada conforme for surgindo a necessidade.