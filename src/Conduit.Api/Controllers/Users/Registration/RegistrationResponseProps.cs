namespace Conduit.Api.Controllers.Users.Registration;

public sealed record RegistrationResponseProps
(
    string Email,
    string Token,
    string Username,
    string? Bio,
    string? Image
);

// https://youtu.be/6EEltKS8AwA?si=Hi-7KR-l_axC0fjP&t=725
