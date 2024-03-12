--Busca um aluno pela matrícula
SELECT MATRICULA, NOME, CPF, NASCIMENTO, SEXO FROM ALUNO WHERE MATRICULA  = :Matricula;

--Busca um aluno por uma parte do nome
SELECT MATRICULA, NOME, CPF, NASCIMENTO, SEXO FROM ALUNO WHERE LOWER(NOME) LIKE :ParteDoNome;

--Busca todos os alunos ordenando-os por matrícula
SELECT MATRICULA, NOME, CPF, NASCIMENTO, SEXO FROM ALUNO  ORDER BY MATRICULA;

--Insere um Aluno
INSERT INTO ALUNO (NOME, CPF, NASCIMENTO, SEXO) VALUES (:Nome, :CPF, :Nascimento, :Sexo);

--Atualiza um aluno com base na chave primária Matricula
UPDATE ALUNO SET NOME = :Nome, CPF = :CPF, NASCIMENTO = :Nascimento, SEXO = :Sexo WHERE MATRICULA = :Matricula;

