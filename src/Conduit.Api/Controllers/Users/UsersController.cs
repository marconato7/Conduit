using System.Security.Claims;
using Conduit.Api.Controllers.Users.Authentication;
using Conduit.Api.Controllers.Users.FollowUser;
using Conduit.Api.Controllers.Users.GetCurrentUser;
using Conduit.Api.Controllers.Users.GetProfile;
using Conduit.Api.Controllers.Users.Registration;
using Conduit.Api.Controllers.Users.UnfollowUser;
using Conduit.Api.Controllers.Users.UpdateUser;
using Conduit.Application.Users.Authentication;
using Conduit.Application.Users.FollowUser;
using Conduit.Application.Users.GetCurrentUser;
using Conduit.Application.Users.GetProfile;
using Conduit.Application.Users.Registration;
using Conduit.Application.Users.UnfollowUser;
using Conduit.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers.Users;

[ApiController]
public class UsersController(ISender sender): ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    [Route("/api/users/login")]
    [AllowAnonymous]
    public async Task<object> Authentication(AuthenticationRequest request)
    {
        try
        {
            var command = new AuthenticationCommand
            (
                Email:    request.User.Email,
                Password: request.User.Password
            );

            var registrationCommandResult = await _sender.Send(command);
            if (registrationCommandResult.IsSuccess)
            {
                return new AuthenticationResponse
                (
                    new AuthenticationResponseProps
                    (
                        Email:    registrationCommandResult.Value.Email,
                        Token:    registrationCommandResult.Value.Token,
                        Username: registrationCommandResult.Value.Username,
                        Bio:      registrationCommandResult.Value.Bio,
                        Image:    registrationCommandResult.Value.Image
                    )
                );
            }
            else
            {
                return Results.InternalServerError();
            }
        }
        catch (Exception exception)
        {
            return Results.InternalServerError(exception.Message);
        }
    }

    [HttpPost]
    [Route("/api/users")]
    [AllowAnonymous]
    public async Task<object> Registration(RegistrationRequest request)
    {
        try
        {
            var command = new RegistrationCommand
            (
                Username: request.User.Username,
                Email:    request.User.Email,
                Password: request.User.Password
            );

            var registrationCommandResult = await _sender.Send(command);
            if (registrationCommandResult.IsSuccess)
            {
                return new RegistrationResponse
                (
                    new RegistrationResponseProps
                    (
                        Email:    registrationCommandResult.Value.Email,
                        Token:    registrationCommandResult.Value.Token,
                        Username: registrationCommandResult.Value.Username,
                        Bio:      registrationCommandResult.Value.Bio,
                        Image:    registrationCommandResult.Value.Image
                    )
                );
            }
            else
            {
                return Results.InternalServerError();
            }
        }
        catch (Exception exception)
        {
            return Results.InternalServerError(exception.Message);
        }
    }

    [HttpGet]
    [Route("/api/user")]
    [Authorize]
    public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser()
    {
        string stepOne   = HttpContext.Request.Headers.Authorization!;
        string[] stepTwo = stepOne.Split("Token ");
        string token     = stepTwo[1];

        var email = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (email is null)
        {
            return Unauthorized();
        }

        var query = new GetCurrentUserQuery(email, token);

        var getCurrentUserQueryResult = await _sender.Send(query);
        if (getCurrentUserQueryResult.IsSuccess)
        {
            var getCurrentUserResponse = new GetCurrentUserResponse
            (
                new GetCurrentUserResponseProps
                (
                    Email:    getCurrentUserQueryResult.Value.Email,
                    Token:    getCurrentUserQueryResult.Value.Token,
                    Username: getCurrentUserQueryResult.Value.Username,
                    Bio:      getCurrentUserQueryResult.Value.Bio,
                    Image:    getCurrentUserQueryResult.Value.Image
                )
            );

            return Ok(getCurrentUserResponse);
        }

        return BadRequest(getCurrentUserQueryResult.Reasons.First().Message);
    }

    [HttpPut]
    [Route("/api/user")]
    [Authorize]
    public async Task<object> UpdateUser(UpdateUserRequest request)
    {
        string stepOne   = HttpContext.Request.Headers.Authorization!;
        string[] stepTwo = stepOne.Split("Token ");
        string token     = stepTwo[1];

        var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (currentUsersEmail is null)
        {
            return Results.Unauthorized();
        }

        var updateUserCommand = new UpdateUserCommand
        (
            CurrentUsersEmail: currentUsersEmail,
            Token:             token,
            Email:             request.User.Email,
            Username:          request.User.Username,
            Password:          request.User.Password,
            Image:             request.User.Image,
            Bio:               request.User.Bio
        );

        var updateUserCommandResult = await _sender.Send(updateUserCommand);
        if (updateUserCommandResult.IsSuccess)
        {
            return new UpdateUserResponse
            (
                new UpdateUserResponseProps
                (
                    Email:    updateUserCommandResult.Value.Email,
                    Token:    updateUserCommandResult.Value.Token,
                    Username: updateUserCommandResult.Value.Username,
                    Bio:      updateUserCommandResult.Value.Bio,
                    Image:    updateUserCommandResult.Value.Image
                )
            );
        }

        return Results.InternalServerError();
    }

    [HttpGet]
    [Route("/api/profiles/{username}")]
    [AllowAnonymous]
    public async Task<IResult> GetProfile(string username)
    {
        var email = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (email is null)
        {
            // return Results.Unauthorized();
        }

        var query = new GetProfileQuery(username, email);

        var getProfileQueryResult = await _sender.Send(query);
        if (getProfileQueryResult.IsSuccess)
        {
            var getProfileResponse = new GetProfileResponse
            (
                new GetProfileResponseProps
                (
                    Username:  getProfileQueryResult.Value.Username,
                    Bio:       getProfileQueryResult.Value.Bio,
                    Image:     getProfileQueryResult.Value.Image,
                    Following: getProfileQueryResult.Value.Following
                )
            );

            return Results.Ok(getProfileResponse);
        }

        return Results.InternalServerError();
    }

    [HttpPost]
    [Route("/api/profiles/{username}/follow")]
    [Authorize]
    public async Task<IResult> FollowUser(string username)
    {
        var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (currentUsersEmail is null)
        {
            return Results.Unauthorized();
        }

        var command = new FollowUserCommand
        (
            UsernameToFollow:  username,
            CurrentUsersEmail: currentUsersEmail
        );

        var followUserCommandResult = await _sender.Send(command);
        if (followUserCommandResult.IsSuccess)
        {
            var getProfileResponse = new FollowUserResponse
            (
                new FollowUserResponseProps
                (
                    Username:  followUserCommandResult.Value.Username,
                    Bio:       followUserCommandResult.Value.Bio,
                    Image:     followUserCommandResult.Value.Image,
                    Following: followUserCommandResult.Value.Following
                )
            );

            return Results.Ok(getProfileResponse);
        }

        return Results.InternalServerError();
    }

    [HttpDelete]
    [Route("/api/profiles/{username}/follow")]
    [Authorize]
    public async Task<IResult> UnfollowUser(string username)
    {
        var currentUsersEmail = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (currentUsersEmail is null)
        {
            return Results.Unauthorized();
        }

        var unfollowUserCommand = new UnfollowUserCommand
        (
            UsernameToUnfollow: username,
            CurrentUsersEmail:  currentUsersEmail
        );

        var unfollowUserCommandResult = await _sender.Send(unfollowUserCommand);
        if (unfollowUserCommandResult.IsSuccess)
        {
            var getProfileResponse = new UnfollowUserResponse
            (
                new UnfollowUserResponseProps
                (
                    Username:  unfollowUserCommandResult.Value.Username,
                    Bio:       unfollowUserCommandResult.Value.Bio,
                    Image:     unfollowUserCommandResult.Value.Image,
                    Following: unfollowUserCommandResult.Value.Following
                )
            );

            return Results.Ok(getProfileResponse);
        }

        return Results.InternalServerError();
    }
}
