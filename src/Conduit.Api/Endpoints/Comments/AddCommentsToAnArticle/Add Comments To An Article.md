
Add Comments to an Article

POST /api/articles/:slug/comments

Example request body:

```json
{
    "comment": {
        "body": "His name was my name too."
    }
}
```

Authentication required, returns the created Comment

Required field: body

Single Comment

```json
{
    "comment": {
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
}
```

