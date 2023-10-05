using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechCareer.WernerHeisenberg.Survey.Application.CQRS.Business.QuestionBusiness.Queries.GetQuestions;
using TechCareer.WernerHeisenberg.Survey.Application.Infrastructure;
using TechCareer.WernerHeisenberg.Survey.Web.Models;

namespace TechCareer.WernerHeisenberg.Survey.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(ILogger<HomeController> logger,
        IMediator mediator,
        IWebHostEnvironment webHostEnvironment
        )
    {
        _logger = logger;
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
}