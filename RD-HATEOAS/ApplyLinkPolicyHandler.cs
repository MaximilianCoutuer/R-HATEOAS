using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDHATEOAS
{
    public class ApplyLinkPolicyHandler : AuthorizationHandler<LinkToSelfRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LinkToSelfRequirement requirement, string resource)
        {
            if (true)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class LinkToSelfRequirement : IAuthorizationRequirement { }
}
