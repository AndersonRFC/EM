using EM.Domain.Models;
using EM.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers;

public class AlunoController : Controller
{
    readonly private AlunoRepository _alunoRepository;

    public AlunoController(AlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<IActionResult> Index(string searchString, string searchType)
    {
        IEnumerable<Aluno> alunos = new List<Aluno>();

        if (!string.IsNullOrEmpty(searchString))
        {
            if (searchType == "id")
            {
                try
                {
                    if (int.TryParse(searchString, out int id))
                    {
                        var alunoPesquisado = await _alunoRepository.GetByMatricula(id);
                        if (alunoPesquisado is not null)
                        {
                            ((List<Aluno>)alunos).Add(alunoPesquisado);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    alunos = await _alunoRepository.GetAllAsync();
                }
            }
            else if (searchType == "nome")
            {
                alunos = await _alunoRepository.GetByContendoNoNome(searchString);
            }
            else
            {
                Console.WriteLine("Método de pesquisa inválido");
            }
        }
        else
        {
            alunos = await _alunoRepository.GetAllAsync();
        }

        var alunoViewModel = new AlunoViewModel
        {
            Alunos = alunos!.ToList(),
            SearchString = searchString,
            SearchType = searchType,
        };

        return View(alunoViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(Aluno aluno)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _alunoRepository.AddAsync(aluno);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        return View("Editar", aluno);

    }

    public async Task<IActionResult> Editar(int? id)
    {
        if(id == null)
        {
            return View("Editar");
        }
        else
        {
            var aluno = await _alunoRepository.GetByMatricula((int)id);
            if(aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Editar(int id, Aluno aluno)
    {
        if(id != aluno.Matricula)
        {
            return NotFound();
        }

        if(ModelState.IsValid)
        {
            try
            {
                await _alunoRepository.UpdateAsync(aluno);
            }
            catch(Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(aluno);
    }

    public async Task<IActionResult> Excluir(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }

        var aluno = await _alunoRepository.GetByMatricula((int)id);

        if(aluno == null)
        {
            return NotFound();
        }

        return View(aluno);
    }

    [HttpPost, ActionName("Excluir")]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        await _alunoRepository.RemoveAsync(new Aluno { Matricula = id });

        return RedirectToAction(nameof(Index));
    }
}
