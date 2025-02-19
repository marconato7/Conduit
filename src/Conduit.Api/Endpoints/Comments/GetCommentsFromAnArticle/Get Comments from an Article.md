
Get Comments from an Article

GET /api/articles/:slug/comments

Authentication optional, returns multiple comments

Multiple Comments

```json
{
    "comments": [
        {
            "id": 1,
            "createdAt": "2016-02-18T03:22:56.637Z",
            "updatedAt": "2016-02-18T03:22:56.637Z",
            "body": "It takes a Jacobian",
            "author": {
                "username": "jake",
                "bio": "I work at statefarm",
                "image": "https://i.stack.imgur.com/xHWG8.jpg",
                "following": false
            }
        }
    ]
}

