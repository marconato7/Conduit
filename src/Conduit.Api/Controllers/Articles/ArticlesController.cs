using Conduit.Api.Controllers.Articles.CreateArticle;
using Conduit.Application.Articles.CreateArticle;
using Conduit.Application.Articles.FavoriteArticle;
using Conduit.Application.Articles.ListArticles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Conduit.Api.Controllers.Articles;

[ApiController]
public class ArticlesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    [Route("/api/articles")]
    [AllowAnonymous]
    public async Task<IActionResult> ListArticles
    (
        string?           tag,
        string?           author,
        string?           favorited,
        int?              limit,
        int?              offset,
        CancellationToken cancellationToken
    )
    {
        var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

        var query = new ListArticlesQuery
        (
            CurrentUsersEmail: currentUsersEmail,
            Tag:               tag,
            Author:            author,
            Favorited:         favorited,
            Limit:             limit,
            Offset:            offset
        );

        var listArticlesQueryResult = await _sender.Send(query, cancellationToken);

        return Ok(listArticlesQueryResult.Value);
    }

    // [HttpGet]
    // [Route("/api/articles/feed")]
    // public async Task<IActionResult> FeedArticles(string slug)
    // {
    //     var query = new GetArticleQuery(slug);

    //     var getArticleQueryResult = await _sender.Send(query);

    //     if (getArticleQueryResult.IsFailed)
    //     {
    //         return NotFound();
    //     }

    //     return Ok(getArticleQueryResult.Value);
    // }

    // [HttpGet]
    // [Route("/api/articles/{slug}")]
    // public async Task<IActionResult> GetArticle(string slug)
    // {
    //     var query = new GetArticleQuery(slug);

    //     var getArticleQueryResult = await _sender.Send(query);

    //     if (getArticleQueryResult.IsFailed)
    //     {
    //         return NotFound();
    //     }

    //     return Ok(getArticleQueryResult.Value);
    // }

    [HttpPost]
    [Route("/api/articles")]
    [Authorize]
    public async Task<ActionResult<CreateArticleResponse>> CreateArticle(CreateArticleRequest request)
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

        var createArticleResponse = new CreateArticleResponse
        (
            new CreateArticleResponseProps
            (
                Slug:           createArticleCommandResult.Value.Slug,
                Title:          createArticleCommandResult.Value.Title,
                Description:    createArticleCommandResult.Value.Description,
                Body:           createArticleCommandResult.Value.Body,
                TagList:        createArticleCommandResult.Value.TagList,
                CreatedAt:      createArticleCommandResult.Value.CreatedAt,
                UpdatedAt:      createArticleCommandResult.Value.UpdatedAt,
                Favorited:      createArticleCommandResult.Value.Favorited,
                FavoritesCount: createArticleCommandResult.Value.FavoritesCount,
                Author:         new AuthorModelForResponse
                (
                    Username:  createArticleCommandResult.Value.Author.Username,
                    Bio:       createArticleCommandResult.Value.Author.Bio,
                    Image:     createArticleCommandResult.Value.Author.Image,
                    Following: createArticleCommandResult.Value.Author.Following
                )
            )
        );

        return Ok(createArticleResponse);
    }

    // [HttpPut]
    // [Route("/api/articles/{slug}")]
    // public void UpdateArticle(string slug)
    // {
    // }

    // // [HttpDelete]
    // // [Route("/api/articles/{slug}")]
    // // [Authorize]
    // // public async Task<ActionResult> DeleteArticle(string slug)
    // // {
    // //     var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault
    // //         (claim => claim.Type == ClaimTypes.Email)?.Value;
        
    // //     if (currentUsersEmail is null)
    // //     {
    // //         return Unauthorized();
    // //     }

    // //     var deleteArticleCommand = new DeleteArticleCommand
    // //     (
    // //         CurrentUsersEmail: currentUsersEmail,
    // //         Slug:              slug
    // //     );

    // //     var deleteArticleCommandResult = await _sender.Send(deleteArticleCommand);
    // //     if (deleteArticleCommandResult.IsFailed)
    // //     {
    // //         return BadRequest();
    // //     }

    // //     return Ok();
    // // }

    // [HttpPost]
    // [Route("/api/articles/{slug}/comments")]
    // public void AddCommentsToAnArticle(CreateArticleRequest request)
    // {
    // }

    // [HttpPost]
    // [HttpGet("/api/articles/{slug}/comments")]
    // public void GetCommentsFromAnArticle(CreateArticleRequest request)
    // {
    // }

    // [HttpDelete]
    // [Route("/api/articles/{slug}/comments/{id}")]
    // public void DeleteComment(CreateArticleRequest request)
    // {
    // }

    // [HttpPost]
    // [Route("/api/articles/{slug}/favorite")]
    // [Authorize]
    // public async Task<ActionResult> FavoriteArticle(string slug)
    // {
    //     var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault
    //         (claim => claim.Type == ClaimTypes.Email)?.Value;
        
    //     if (currentUsersEmail is null)
    //     {
    //         return Unauthorized();
    //     }

    //     var favoriteArticleCommand = new FavoriteArticleCommand
    //     (
    //         CurrentUsersEmail: currentUsersEmail,
    //         Slug:              slug
    //     );

    //     var favoriteArticleCommandResult = await _sender.Send(favoriteArticleCommand);
    //     if (favoriteArticleCommandResult.IsFailed)
    //     {
    //         return BadRequest();
    //     }

    //     // TODO
    //     return Ok();
    // }

    // [HttpDelete]
    // [Route("/api/articles/{slug}/favorite")]
    // public void UnfavoriteArticle(CreateArticleRequest request)
    // {
    // }

    // [HttpGet]
    // [Route("/api/tags")]
    // public void GetTags(CreateArticleRequest request)
    // {
    // }
}
