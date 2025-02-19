namespace Conduit.Api.Endpoints.Comments.GetCommentsFromAnArticle;

internal sealed record GetCommentsFromAnArticleResponse
(
    List<GetCommentsFromAnArticleResponseProps> Comments
);
