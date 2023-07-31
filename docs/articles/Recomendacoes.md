
## Recomendações

Assumindo que esteja utilizando todas as ferramentas que este projeto proprociona, sem nenhuma modificação do codigo original, recomenda-se:

 1. Sempre que possivel utilze o `BaseModule` para criar seus própridos modulos.
 2. Sempre crie, remova e verifique a instancias de seu modulo pelo gerenciador.
 3. Não utilize multiplas instancias de um mesmo gerenciador.
 4. Caso utilize o Gerenciador Dinamico, evite atualizacoes constantes de modulos, o sistema utiliza reflection e emit para gerar as classes em tempo de execução, logo quando um novo modulo ou contrato é descoberto, o gerenciador aciona o emit para gerar as novas versoes das classes.
 5. No gerenciador sempre crie novos modulos e evite obter um ja existente se o modulo não for de instancia unica.
 6. Evite de criar modulos que persistam dados, crie para processar algo e finalizar a sua tarefa. Modulos são volateis.
 7. Evite de usar a funcao `RemoveAllModules` do Gerenciador Core quando o Gerenciador Dinamico estiver em uso, o Gerenciador Core, enxerga todos como `Modulo`.
 8. Não utilize o prefixo `Microsoft` em seus projetos de modulo, o sistema que verifica os arquivos ignora todas as DLLs que possuem este prefixo
 
> [!WARNING]
> As recomendações acima servem para evitar possiveis comportamentos inesperados que podem ocasionar erros.

## Possibilidades

> [!WARNING]
> Considere que qualquer modificação ou implementação com base no codigo original não será de responsabilidade do autor deste projeto.

Com este conjunto de ferramente, voce pode:

- Criar um novo modelo de Modulo, desde que implemente o `IModule`
- Criar um novo Gerenciador (Core ou Dinamico ou qualquer outro), desde que implemente o `IModuleManager`
- Alterar comportamento de um Modulo sobrescrevendo os metodos existentes (Como o `status` por exemplo)
- Utilizar uma parte do projeto, caso não queira utilizar o Gerenciador dinamico por questões de segurança, voce pode, sem a necessidade de modificar modulo algum, más lembre-se, que agora deve referenciar todos os projetos antes de publicar.
- Trocar uma parte do projeto, caso não ache funcional o Gerenciador core, voce pode adicionar um novo gerenciador customizado sem interferir no funcionamento do restante do projeto.
- Voce pode incrementar novas funcionalidades ou comportamentos nos componentes ja existentes, como o `BaseModule` ou até mesmo o `ModuleManager`

> Voce tem total controle sobre a ferramente, se não quiser ou não gostar de alguma parte dele, basta retira-la ou troca-la.

## Informações

- O status do `Modulo` é controlado pelo Gerenciador, modulo e voce. O Gerenciador, tem a capacidade de manipular os seguintes estados de um modulo, `Created` e `Ready`, já o modulo em si, pode manipular os seguintes estados, `Off` e `Error` e o `Initialized` é atribuido quando voce achar melhor para o seu modulo.
- O Gerenciador dinamico é um `Modulo` e um `ModuleManager`, ou seja, ele é um modulo que se comporta como um gerenciador, perceba que tudo fica sob o Gerenciador Core, incluindo outros gerenciadores. A principal funcao do Gerenciador dinamico é gerar um modulo em tempo de execução anexando um novo contrato a ele e depois é repassado o tipo para o gerenciador core criar a instancia do mesmo.