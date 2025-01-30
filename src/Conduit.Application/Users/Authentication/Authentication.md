
Authentication:

POST /api/users/login

Example request body:

```json
{
    "user":{
        "email": "jake@jake.jake",
        "password": "jakejake"
    }
}
```

No authentication required, returns a User

Required fields: email, password

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

