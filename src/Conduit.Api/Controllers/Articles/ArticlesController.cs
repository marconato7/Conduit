using Microsoft.AspNetCore.Mvc;
using Conduit.Application.Articles.GetArticle;
using MediatR;
using Conduit.Application.Articles.CreateArticle;
using Conduit.Api.Controllers.Articles.CreateArticle;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Conduit.Application.Articles.ListArticles;
using Conduit.Application.Articles.FavoriteArticle;

namespace Conduit.Api.Controllers.Articles;

[ApiController]
[Route("api/articles")]
public class ArticlesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [Route("/api/articles")]
    public async Task<IActionResult> ListArticles()
    {
        var query = new ListArticlesQuery();

        var listArticlesQueryResult = await _sender.Send(query);

        return Ok(listArticlesQueryResult.Value);
    }

    [HttpGet]
    [Route("/api/articles/feed")]
    public async Task<IActionResult> FeedArticles(string slug)
    {
        var query = new GetArticleQuery(slug);

        var getArticleQueryResult = await _sender.Send(query);

        if (getArticleQueryResult.IsFailed)
        {
            return NotFound();
        }

        return Ok(getArticleQueryResult.Value);
    }

    [HttpGet]
    [Route("/api/articles/{slug}")]
    public async Task<IActionResult> GetArticle(string slug)
    {
        var query = new GetArticleQuery(slug);

        var getArticleQueryResult = await _sender.Send(query);

        if (getArticleQueryResult.IsFailed)
        {
            return NotFound();
        }

        return Ok(getArticleQueryResult.Value);
    }

    [HttpPost]
    [Route("/api/articles")]
    public async Task<ActionResult> CreateArticle(CreateArticleRequest request)
    {
        string stepOne   = HttpContext.Request.Headers.Authorization!;
        string[] stepTwo = stepOne.Split("Token ");
        string token     = stepTwo[1];

        var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault
            (claim => claim.Type == ClaimTypes.Email)?.Value;
        
        if (currentUsersEmail is null)
        {
            return Unauthorized();
        }

        var createArticleCommand = new CreateArticleCommand
        (
            CurrentUsersEmail: currentUsersEmail,
            Title:             request.Article.Title,
            Description:       request.Article.Description,
            Body:              request.Article.Body,
            TagList:           request.Article.TagList
        );

        var createArticleCommandResult = await _sender.Send(createArticleCommand);
        if (createArticleCommandResult.IsFailed)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut]
    [Route("/api/articles/{slug}")]
    public void UpdateArticle(string slug)
    {
    }

    // [HttpDelete]
    // [Route("/api/articles/{slug}")]
    // [Authorize]
    // public async Task<ActionResult> DeleteArticle(string slug)
    // {
    //     var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault
    //         (claim => claim.Type == ClaimTypes.Email)?.Value;
        
    //     if (currentUsersEmail is null)
    //     {
    //         return Unauthorized();
    //     }

    //     var deleteArticleCommand = new DeleteArticleCommand
    //     (
    //         CurrentUsersEmail: currentUsersEmail,
    //         Slug:              slug
    //     );

    //     var deleteArticleCommandResult = await _sender.Send(deleteArticleCommand);
    //     if (deleteArticleCommandResult.IsFailed)
    //     {
    //         return BadRequest();
    //     }

    //     return Ok();
    // }

    [HttpPost]
    [Route("/api/articles/{slug}/comments")]
    public void AddCommentsToAnArticle(CreateArticleRequest request)
    {
    }

    [HttpPost]
    [HttpGet("/api/articles/{slug}/comments")]
    public void GetCommentsFromAnArticle(CreateArticleRequest request)
    {
    }

    [HttpDelete]
    [Route("/api/articles/{slug}/comments/{id}")]
    public void DeleteComment(CreateArticleRequest request)
    {
    }

    [HttpPost]
    [Route("/api/articles/:slug/favorite")]
    [Authorize]
    public async Task<ActionResult> FavoriteArticle(string slug)
    {
        var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault
            (claim => claim.Type == ClaimTypes.Email)?.Value;
        
        if (currentUsersEmail is null)
        {
            return Unauthorized();
        }

        var favoriteArticleCommand = new FavoriteArticleCommand
        (
            CurrentUsersEmail: currentUsersEmail,
            Slug:              slug
        );

        var favoriteArticleCommandResult = await _sender.Send(favoriteArticleCommand);
        if (favoriteArticleCommandResult.IsFailed)
        {
            return BadRequest();
        }

        // TODO
        return Ok();
    }

    [HttpDelete]
    [Route("/api/articles/:slug/favorite")]
    public void UnfavoriteArticle(CreateArticleRequest request)
    {
    }

    [HttpGet]
    [Route("/api/tags")]
    public void GetTags(CreateArticleRequest request)
    {
    }
}
