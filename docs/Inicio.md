## Propeus.Modulo
Este projeto tem como objetivo facilitar o gerenciamento de modulo .NET e aplicar o conceito de DI, permitindo a inje��o de dependencia
em tempo de execu��o.

## Licen�a

Os principais projetos s�o licenciados da seguinte maneira


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

Os projetos `Propeus.Modulo.Core` e `Propeus.Modulo.Dinamico` est�o sob a licen�a `LSUNCA` para que todos possam usar de forma
**gratuita** tanto o projeto quanto as suas modificacoes e derivados.

Sendo assim qualquer empresa pode comercializar seus modulos m�s os gerenciadores e seu codigo-fonte devem ser distribuidos de forma **gratuita**,

� permitido a comercializa��o de gerenciadores que s�o construidos sem qualquer uso dos projetos `Propeus.Modulo.Core`, `Propeus.Modulo.Dinamico`,
suas modifica�oes e derivados.

\* As licen�as podem ser alteradas conforme for necessario para manter a **gratuidade** e a liberdade de usar o codigo-fonte

## Planos

Os proximos passos deste projeto � finalizar o projeto `Propeus.Modulo.Hosting` permitindo a cria��o e renderiza��o de views dinamicamente.

Ap�s este passo, os seguintes passos est�o na lista para serem executados:

1. Otimizar o codigo-fonte
2. Documentar o restante do codigo-fonte
3. Tonar o projeto como um MVP (Menor Produto Viavel)
4. Criar exemplos mais complexos e funcionais (Jogos de RPG em console, projeto web para testar modulos etc)
5. Internacionalizar os nomes de classe, interfaces, propriedades, metodos, eventos para o ingles
6. Criar documenta��o em ingles
7. Criar modulos para aumentar a capacidade de uso do projeto (Conexoes com cache, banco de dados, comunicacao entre programas, seguran�la etc)

## Perguntas frequentes

> Existe uma documenta��o t�cnica sobre o funcinamento deste projeto?

Existe uma documenta��o auto-gerado baseado na documenta��o em codigo.

> Em que fase est� este projeto?

Atuamente esta na fase de PoC (Prova de Conceito).

> Posso uar em produ��o?

Pode por sua conta e risco

> Por que este projeto nao usa componentes de terceiros?

A id�ia ptincipal deste projeto � ser totalmente modular e possuir o minimo de dependencia possivel, ent�o n�o faria sentido
adicionar componentes de proxy dinamico e inje��o de dependencia de terceiros.

\* Esta pagina pode ser alterada conforme for surgindo a necessidade.