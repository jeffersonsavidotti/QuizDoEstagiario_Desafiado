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
                Pergunta = "O que é Git?",
                Respostas = new string[] { "Sistema de Controle de Versão", "Banco de Dados", "Linguagem de Programação" }
            },
            new QuizModel
            {
                Pergunta = "Qual comando SQL é usado para deletar uma linha do banco de dados?",
                Respostas = new string[] { "DELETE FROM", "REMOVE", "DROP" }
            },
            new QuizModel
            {
                Pergunta = "O que é um método estático em programação?",
                Respostas = new string[] { "Um método que pertence à classe, não à instância", "Um método que só pode ser chamado de dentro de outro método", "Um método que não aceita parâmetros" }
            },
            new QuizModel
            {
                Pergunta = "Qual é a diferença entre 'const' e 'readonly' em C#?",
                Respostas = new string[] { "const é resolvido em tempo de compilação, readonly em tempo de execução", "const pode ser usado apenas para valores primitivos, readonly para qualquer tipo", "const é sempre estático, readonly pode ser de instância" }
            },
            new QuizModel
            {
                Pergunta = "O que é uma exceção em programação?",
                Respostas = new string[] { "Um evento anormal que ocorre durante a execução de um programa", "Um tipo de variável em C#", "Um método especial que lida com erros em tempo de compilação" }
            },
            // Adicione mais perguntas conforme necessário
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
