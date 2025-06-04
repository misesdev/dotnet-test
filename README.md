# A5 Solutions

## Teste Funcional

**1 - Na primeria questão, dado o fluxograma abaixo segue a tabela preenchida:**

```
    inicio
    leia x
    leia y
    z <= x*y + 5
    se z <= 0 então
        Resultado <= "A"
    senão
        se z <= 100 então
            Resultado <= "B"
        senão
            Resultado <= "C"
        fim-se
    fim-se
    escrever: Z, Resultado
    fim
```

|   X    |   Y   |   Z   |   Resultado   |
|--------|-------|-------|---------------|
|   3    |   2   |  11   |       B       |
|  150   |   3   |  455  |       C       |
|   7    |  -1   |  -2   |       A       |
|  -2    |   5   |  -5   |       A       |
|   50   |   3   |  155  |       C       |


**2 - Dado a média aritimetica das notas de um aluno definir o estado:**

Fiz um fluxograma como requerido e uma implementação simples em c# em [/school](/school), 
os detalhes estão no README.md.

## Teste de Banco de Dados

**3 - Dado o diagrama da imagem, construir os comando para inserir e atualizar os campos da tabela `pessoa`:**

![diagrama](/images/diagram.png)

```sql
    INSERT INTO pessoa (nome, data_nascimento) 
        values
    ('Elisson Vale', '2025-01-01');

    UPDATE pessoa SET 
        nome = 'Mises'
    WHERE id = 12

    UPDATE pessoa SET
        data_nascimento = CONVERT(DATE, '2025-03-09')
    WHERE nome = 'Elisson Vale'
```

**4 - De a cordo com o modelo acima, construir o comando para retornar o nome da pessoa e o endereço completo
(CEP, Logradouro, Bairro, Cidade, UF, Numero e Complemento).**

```sql
    SELECT P.Id,
        P.Nome,
        PE.CEP,
        EN.Logradouro,
        EN.Bairro,
        EN.Cidade,
        EN.UF,
        PE.Numero,
        PE.Complemento
    FROM pessoa as P
        INNER JOIN pessoa_x_endereco as PE ON PE.IdPessoa = P.Id
        INNER JOIN endereco as EN ON EN.CEP = PE.CEP
    WHERE P.id = 23
        OR PE.CEP = '00000-000'
```

## Desafio

A implementação do desafio está em [/godzilla API](/godzilla/), todos os detalhes de como executar estão 
no README.md



