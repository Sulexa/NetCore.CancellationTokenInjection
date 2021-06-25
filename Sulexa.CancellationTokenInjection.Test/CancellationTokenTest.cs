﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sulexa.CancellationTokenInjection.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Sulexa.CancellationTokenInjection.Test
{
    [TestClass]
    public class CancellationTokenTest
    {

        [TestMethod]
        public async Task CancellationToken_Not_Cancel_Success()
        {
            var provider = Utils.CreateServiceProviderForHttpContextCancellationToken();

            var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            var cancelationTokenBase = provider.GetRequiredService<CancellationTokenBase>();
            var cancellationTokenSource = new CancellationTokenSource();

            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.RequestAborted = cancellationTokenSource.Token;

            await Task.Delay(2000, cancelationTokenBase);
        }

        [TestMethod]
        public async Task CancellationToken_Cancel_ThrowTaskCanceledException()
        {
            var provider = Utils.CreateServiceProviderForHttpContextCancellationToken();

            var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            var cancelationTokenBase = provider.GetRequiredService<CancellationTokenBase>();
            var cancellationTokenSource = new CancellationTokenSource();

            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.RequestAborted = cancellationTokenSource.Token;

            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                cancellationTokenSource.Cancel();
            });

            await Assert.ThrowsExceptionAsync<TaskCanceledException>(() => Task.Delay(2000, cancelationTokenBase));
         }
    }
}
