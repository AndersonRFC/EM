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

    public IActionResult Index(string searchString, string searchType)
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
                        var alunoPesquisado = _alunoRepository.GetByMatricula(id);
                        if (alunoPesquisado is not null)
                        {
                            ((List<Aluno>)alunos).Add(alunoPesquisado);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    alunos = _alunoRepository.GetAll();
                }
            }
            else if (searchType == "nome")
            {
                alunos = _alunoRepository.GetByContendoNoNome(searchString);
            }
            else
            {
                Console.WriteLine("Método de pesquisa inválido");
            }
        }
        else
        {
            alunos = _alunoRepository.GetAll();
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
    public IActionResult Cadastrar(Aluno aluno)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _alunoRepository.Add(aluno);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        return View("Editar", aluno);

    }

    public IActionResult Editar(int? id)
    {
        if(id == null)
        {
            return View("Editar");
        }
        else
        {
            var aluno = _alunoRepository.GetByMatricula((int)id);
            if(aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }
    }


    [HttpPost]
    public IActionResult Editar(int id, Aluno aluno)
    {
        if(id != aluno.Matricula)
        {
            return NotFound();
        }

        if(ModelState.IsValid)
        {
            try
            {
                _alunoRepository.Update(aluno);
            }
            catch(Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(aluno);
    }

    public IActionResult Excluir(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }

        var aluno = _alunoRepository.GetByMatricula((int)id);

        if(aluno == null)
        {
            return NotFound();
        }

        return View(aluno);
    }

    [HttpPost, ActionName("Excluir")]
    public IActionResult ExcluirConfirmado(int id)
    {
        _alunoRepository.Remove(new Aluno { Matricula = id });

        return RedirectToAction(nameof(Index));
    }
}
