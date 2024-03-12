CREATE TABLE Aluno (
    Matricula INTEGER PRIMARY KEY,
    Nome VARCHAR(100),
    CPF VARCHAR(14),
    Nascimento DATE,
    Sexo CHAR(1)
);

CREATE SEQUENCE Seq_Matricula;

SET TERM ^ ;

CREATE TRIGGER Aluno_BI FOR Aluno
BEFORE INSERT
AS
BEGIN
  IF (NEW.Matricula IS NULL) THEN
    NEW.Matricula = NEXT VALUE FOR Seq_Matricula;
END^

SET TERM ; ^

