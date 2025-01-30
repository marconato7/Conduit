
Update User

PUT /api/user

Example request body:

{
  "user":{
    "email": "jake@jake.jake",
    "bio": "I like to skateboard",
    "image": "https://i.stack.imgur.com/xHWG8.jpg"
  }
}

Authentication required, returns the User

Accepted fields: email, username, password, image, bio

Users (for authentication)

```json
{
    "user": {
        "email": "jake@jake.jake",
        "token": "jwt.token.here",
        "username": "jake",
        "bio": "I work at statefarm",
        "image": null
    }
}
```

