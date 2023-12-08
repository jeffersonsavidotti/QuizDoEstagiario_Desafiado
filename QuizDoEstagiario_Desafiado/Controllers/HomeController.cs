using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using QuizDoEstagiario_Desafiado.Models;

public class HomeController : Controller
{
    private readonly List<QuizModel> quizzes;

    public HomeController()
    {
        // Inicialize as perguntas e respostas aqui
        quizzes = new List<QuizModel>
        {
            new QuizModel
            {
                Pergunta = "Qual destas é uma tecnologia de Front-end?",
                Respostas = new string[] { "React", "C#", "PHP", "SQL" }
            },
            new QuizModel
            {
                Pergunta = "Qual destas é uma tecnologia de Back-end?",
                Respostas = new string[] { "Golang", "CSS", "HTML", "Angular" }
            },
            new QuizModel
            {
                Pergunta = "Como deletar apenas uma linha do DB?",
                Respostas = new string[] { "DELETE FROM", "UPDATE", "DELETE", "DROP" }
            },
            new QuizModel
            {
                Pergunta = "Qual deste não é uma estrutura de repetição?",
                Respostas = new string[] { "else", "while", "for", "do while" }
            },
            new QuizModel
            {
                Pergunta = "Qual é a diferença entre 'const' e 'readonly' em C#?",
                Respostas = new string[] { "const é resolvido em tempo de compilação e readonly em tempo de execução", "const pode ser usado apenas para valores primitivos e readonly para qualquer tipo", "const é sempre estático e readonly pode ser de instância", "const é sempre uma variavel e readonly é um objeto" }
            },
        };
    }


    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("PerguntaAtual") == null)
        {
            HttpContext.Session.SetInt32("PerguntaAtual", 0);
        }

        int perguntaAtual = (int)HttpContext.Session.GetInt32("PerguntaAtual");

        if (perguntaAtual < quizzes.Count)
        {
            var quizAtual = quizzes[perguntaAtual];

            ViewBag.Pergunta = quizAtual.Pergunta;
            ViewBag.Respostas = quizAtual.Respostas;

            HttpContext.Session.SetInt32("PerguntaAtual", perguntaAtual + 1);

            return View();
        }
        else
        {
            return RedirectToAction("Result");
        }
    }

    [HttpPost]
    public IActionResult Index(string resposta)
    {
        var userAnswers = HttpContext.Session.GetString("UserAnswers") ?? string.Empty;
        userAnswers += resposta + ",";
        HttpContext.Session.SetString("UserAnswers", userAnswers);

        return RedirectToAction("Index");
    }

    public IActionResult Result()
    {
        var userAnswers = HttpContext.Session.GetString("UserAnswers")?.Split(',') ?? new string[0];
        var correctAnswers = quizzes.Select(q => q.Respostas[0]).ToArray(); // Supondo que a resposta correta está sempre na posição 0

        var score = userAnswers
        .Where((answer, index) => index < correctAnswers.Length && answer == correctAnswers[index])
        .Count();

        ViewBag.Finished = true;
        ViewBag.Score = score;
        ViewBag.Questions = quizzes;
        ViewBag.UserAnswers = userAnswers;
        ViewBag.CorrectAnswers = correctAnswers;

        return View("Index");
    }
    [HttpPost]
    public IActionResult ResetQuiz()
    {
        HttpContext.Session.Clear(); // Limpa a sessão
        return RedirectToAction("Index");
    }

}
