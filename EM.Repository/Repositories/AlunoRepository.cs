using EM.Domain.Models;
using FirebirdSql.Data.FirebirdClient;
using System.Linq.Expressions;

namespace EM.Repository.Repositories;

public class AlunoRepository : RepositorioAbstrato<Aluno>
{
    private readonly FbConnection _connection;

    public AlunoRepository(FbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Aluno?> GetByMatricula(int matricula)
    {
        Aluno? aluno = null;

        string sql = "SELECT MATRICULA, NOME, CPF, NASCIMENTO, SEXO FROM ALUNO WHERE MATRICULA = @Matricula;";

        using (FbCommand command = new(sql, _connection))
        {
            command.Parameters.AddWithValue("@Matricula", matricula);
            await _connection.OpenAsync();

            using (FbDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    aluno = new Aluno()
                    {
                        Matricula = reader.GetInt32(reader.GetOrdinal("MATRICULA")),
                        Nome = reader.GetString(reader.GetOrdinal("NOME")),
                        CPF = reader.GetString(reader.GetOrdinal("CPF")),
                        Nascimento = reader.GetDateTime(reader.GetOrdinal("NASCIMENTO")),
                        Sexo = reader.GetInt32(reader.GetOrdinal("SEXO")) == 0 ? EnumeradorSexo.Masculino : EnumeradorSexo.Feminino
                    };
                }
            }
            _connection.Close();
        }
        return aluno;
    }


	public async Task<IEnumerable<Aluno>> GetByContendoNoNome(string parteDoNome)
    {
        List<Aluno> alunos = [];

        string sql = "SELECT MATRICULA, NOME, CPF, NASCIMENTO, SEXO FROM ALUNO WHERE LOWER(NOME) LIKE @ParteDoNome;";

        using(FbCommand command = new(sql, _connection))
        {
            command.Parameters.AddWithValue("@ParteDoNome", "%" + parteDoNome.ToLower() + "%");
            await _connection.OpenAsync();

            using (FbDataReader reader = await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    alunos.Add( new Aluno()
                    {
                        Matricula = reader.GetInt32(reader.GetOrdinal("MATRICULA")),
                        Nome = reader.GetString(reader.GetOrdinal("NOME")),
                        CPF = reader.GetString(reader.GetOrdinal("CPF")),
                        Nascimento = reader.GetDateTime(reader.GetOrdinal("Nascimento")),
                        Sexo = reader.GetInt32(reader.GetOrdinal("SEXO")) == 0 ? EnumeradorSexo.Masculino : EnumeradorSexo.Feminino
                    });
                }
            }
            _connection.Close();
        }
        return alunos;
    }

    public override async Task AddAsync(Aluno aluno)
    {
        string sql = "INSERT INTO ALUNO (NOME, CPF, NASCIMENTO, SEXO) VALUES (@Nome, @CPF, @Nascimento, @Sexo);";
    
        using(FbCommand command = new(sql, _connection))
        {
            command.Parameters.AddWithValue("@Nome", aluno.Nome);
            command.Parameters.AddWithValue("@CPF", aluno.CPF);
            command.Parameters.AddWithValue("@Nascimento", aluno.Nascimento.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Sexo", aluno.Sexo == EnumeradorSexo.Masculino ? 0 : 1);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
    }


    public override async Task<IEnumerable<Aluno>> GetAsync(Expression<Func<Aluno, bool>> predicate)
    {
        var allAlunos = await GetAllAsync();
        return allAlunos.Where(predicate.Compile());
    }


    public override async Task<IEnumerable<Aluno>> GetAllAsync()
    {
        List<Aluno> alunos = [];

        string sql = "SELECT MATRICULA, NOME, CPF, NASCIMENTO, SEXO FROM ALUNO  ORDER BY MATRICULA;";

        using(FbCommand command = new(sql, _connection))
        {
            await _connection.OpenAsync();

            using(FbDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    alunos.Add(new Aluno()
                    {
                        Matricula = reader.GetInt32(reader.GetOrdinal("MATRICULA")),
                        Nome = reader.GetString(reader.GetOrdinal("NOME")),
                        CPF = reader.GetString(reader.GetOrdinal("CPF")),
                        Nascimento = reader.GetDateTime(reader.GetOrdinal("Nascimento")),
                        Sexo = reader.GetInt32(reader.GetOrdinal("SEXO")) == 0 ? EnumeradorSexo.Masculino : EnumeradorSexo.Feminino
                    });
                }
            }
            _connection.Close();
        }
        return alunos;
    }


    public override async Task UpdateAsync(Aluno aluno)
    {
        string sql = "UPDATE ALUNO SET NOME = @Nome, CPF = @Cpf, NASCIMENTO = @Nascimento, SEXO = @Sexo WHERE MATRICULA = @Matricula;";

        using(FbCommand command = new(sql, _connection))
        {
            command.Parameters.AddWithValue("@Nome", aluno.Nome);
            command.Parameters.AddWithValue("@CPF", aluno.CPF);
            command.Parameters.AddWithValue("@Nascimento", aluno.Nascimento.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Sexo", aluno.Sexo == EnumeradorSexo.Masculino ? 0 : 1);

            command.Parameters.AddWithValue("@Matricula", aluno.Matricula);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
    }


    public override async Task RemoveAsync(Aluno aluno)
    {
        string sql = "DELETE FROM ALUNO WHERE MATRICULA = @Matricula;";

        using (FbCommand command = new(sql, _connection))
        {
            command.Parameters.AddWithValue("@Matricula", aluno.Matricula);

            await _connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
    }
}
