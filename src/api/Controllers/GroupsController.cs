using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

namespace api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    [HttpGet]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    public async Task<IEnumerable<Models.Group>> Get(
        [FromServices] GraphServiceClient graphServiceClient)
    {
        var page = await graphServiceClient.Groups.Request().GetAsync();

        var groups = page.Select(g => new Models.Group
        {
            Id = g.Id,
            Name = g.DisplayName
        }).OrderBy(g => g.Name);

        return groups.ToArray();
    }

    [HttpPost("invite")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    public async Task<IActionResult> Invite(
        Models.Invitation invitation,
        [FromServices] IMailSender sender,
        [FromServices] ITokenizer tokenizer)
    {
        if (invitation.Email == null || invitation.Name == null || invitation.GroupId == null)
            return BadRequest("Value cannot be null");

        var token = await tokenizer.GetTokenAsync(Request, invitation);
        var link = tokenizer.GetLink(token);
        await sender.InviteAsync(invitation.Email, invitation.Name, link);
        
        return Ok();
    }
}
