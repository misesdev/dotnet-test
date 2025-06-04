# Fluxograma de Prova Final

**Dado os requisitos na imagem abaixo, foi elaborado o fluxograma em seguida:**

![Questão](/images/question.png)


```
    leia nota1
    leia nota2
    leia nota3
    
    media <= (nota1 + nota2 + nota3) / 3

    se media <= 4.9 então
        notarequerida <= (7 - media) + 7
        exibir "Aluno em recuperação"
        exibir "Nota mínima de {notarequerida} para aprovação"
    se media <= 6.9 então
        notarequerida <= (7 - media) + 7
        exibir "Aluno em prova final"
        exibir "Nota mínima de {notarequerida} para aprovação"
    senão então
        exibir "Aluno aprovado!"

    exibir fim      
        
```

`Nota`

Existe um caso não coberto nos requisitos da imagem, quando um aluno tem uma média aritimética 
menor que 4, então a nota exigida para resultar em uma média aritimética entre as quantro provas
maior ou igual a 7 (média de aprovação) é maior que 10,nesse cenário o aluno estaria reprovado, exemplo:

```
    let nota1 = 4
    let nota2 = 3
    let nota3 = 4
    let media = (nota1 + nota2 + nota3) / 3 // resultado 3.66

    let nota4 = 10
    media = (nota1 + nota2 + nota3 + nota4) / 4 // resultado 5.25
```

### Implementação

`opcional(não solicitado)`

Foi implementado o sistema a partir do fluxograma oriundo dos requisitos da imagem, uma vez 
feito clone desse reporitório:

```bash
    git clone meu.reporitório
```

Então basta navegar até a parta com a implementação, e executar com o dotnet CLI:

```bash
    cd a5-solutions/school/ && dotnet run
```

Essa implementação é um programa de console.


