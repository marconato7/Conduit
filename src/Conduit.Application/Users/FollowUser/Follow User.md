
Follow user

POST /api/profiles/:username/follow

Authentication required, returns a Profile

No additional parameters required

Profile

```json
{
    "profile": {
        "username": "jake",
        "bio": "I work at statefarm",
        "image": "https://api.realworld.io/images/smiley-cyrus.jpg",
        "following": false
    }
}
```

