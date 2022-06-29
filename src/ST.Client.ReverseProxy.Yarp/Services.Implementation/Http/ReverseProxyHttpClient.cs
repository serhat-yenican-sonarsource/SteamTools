// https://github.com/dotnetcore/FastGithub/blob/2.1.4/FastGithub.Http/HttpClient.cs

using System.Application.Models;
using System.Net.Http.Headers;

namespace System.Application.Services.Implementation.Http;

/// <summary>
/// 表示 HTTP 客户端
/// </summary>
public class ReverseProxyHttpClient : HttpMessageInvoker
{
    /// <summary>
    /// 插入的 UserAgent 标记
    /// </summary>
    static readonly ProductInfoHeaderValue userAgent
       = new(new ProductHeaderValue(Constants.HARDCODED_APP_NAME_NEW, "1.0"));

    public ReverseProxyHttpClient(IDomainConfig domainConfig, IDomainResolver domainResolver)
        : this(new ReverseProxyHttpClientHandler(domainConfig, domainResolver), disposeHandler: true)
    {
    }

    public ReverseProxyHttpClient(HttpMessageHandler handler, bool disposeHandler)
        : base(handler, disposeHandler)
    {
    }

    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.UserAgent.Contains(userAgent))
        {
            const string message =
                $"Because {{0}} actually points to {Constants.HARDCODED_APP_NAME_NEW} itself, " +
                $"{Constants.HARDCODED_APP_NAME_NEW} has interrupted this forwarding";
            throw new ApplicationException(string.Format(message, request.RequestUri));
        }
        request.Headers.UserAgent.Add(userAgent);
        var response = await base.SendAsync(request, cancellationToken);
        response.Headers.Server.TryParseAdd(Constants.HARDCODED_APP_NAME_NEW);
        return response;
    }
}