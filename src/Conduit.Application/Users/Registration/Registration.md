Registration:

POST /api/users

Example request body:

```json
{
    "user":{
        "username": "Jacob",
        "email": "jake@jake.jake",
        "password": "jakejake"
    }
}
```

No authentication required, returns a User

Required fields: email, username, password

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

