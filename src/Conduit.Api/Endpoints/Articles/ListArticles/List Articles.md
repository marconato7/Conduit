
List Articles

GET /api/articles

Returns most recent articles globally by default, provide tag, author or favorited query parameter to filter results

Query Parameters:

Filter by tag:

?tag=AngularJS

Filter by author:

?author=jake

Favorited by user:

?favorited=jake

Limit number of articles (default is 20):

?limit=20

Offset/skip number of articles (default is 0):

?offset=0

Authentication optional, will return multiple articles, ordered by most recent first

Multiple Articles

Caution

Starting from the 2024/08/16, the endpoints retrieving a list of articles do no longer return the body of an article for performance reasons. It affcts:

    GET /api/articles
    GET /api/articles/feed

```json
{
    "articles": [
        {
            "slug": "how-to-train-your-dragon",
            "title": "How to train your dragon",
            "description": "Ever wonder how?",
            "tagList": ["dragons", "training"],
            "createdAt": "2016-02-18T03:22:56.637Z",
            "updatedAt": "2016-02-18T03:48:35.824Z",
            "favorited": false,
            "favoritesCount": 0,
            "author": {
                "username": "jake",
                "bio": "I work at statefarm",
                "image": "https://i.stack.imgur.com/xHWG8.jpg",
                "following": false
            }
        },
        {
            "slug": "how-to-train-your-dragon-2",
            "title": "How to train your dragon 2",
            "description": "So toothless",
            "tagList": ["dragons", "training"],
            "createdAt": "2016-02-18T03:22:56.637Z",
            "updatedAt": "2016-02-18T03:48:35.824Z",
            "favorited": false,
            "favoritesCount": 0,
            "author": {
                "username": "jake",
                "bio": "I work at statefarm",
                "image": "https://i.stack.imgur.com/xHWG8.jpg",
                "following": false
            }
        }
    ],
    "articlesCount": 2
}
```

