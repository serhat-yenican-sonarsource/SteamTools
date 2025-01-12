// https://github.com/dotnetcore/FastGithub/blob/2.1.4/FastGithub.HttpServer/GithubSshReverseProxyHandler.cs

using System.Application.Models;

namespace System.Application.Services.Implementation.HttpServer;

/// <summary>
/// Github SSH 代理处理者
/// </summary>
sealed class GithubSshReverseProxyHandler : TcpReverseProxyHandler
{
    public GithubSshReverseProxyHandler(IDomainResolver domainResolver)
        : base(domainResolver, new("github.com", IReverseProxyConfig.SshPort))
    {

    }
}
