
Get Current User

GET /api/user

Authentication required, returns a User that’s the current user

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

