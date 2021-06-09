﻿using Microsoft.AspNetCore.Mvc;
using Sulexa.CancellationTokenInjection.Models;
using System.Threading.Tasks;

namespace Sulexa.CancellationTokenInjection.TestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CancellationTokenTestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly CancellationTokenBase _cancellationTokenBase;

        public CancellationTokenTestController(CancellationTokenBase cancellationTokenBase)
        {
            this._cancellationTokenBase = cancellationTokenBase;
        }

        [HttpGet("TestCancellationAsync")]
        public async Task<ActionResult<string>> TestCancellationAsync()
        {
            await Task.Delay(10000, _cancellationTokenBase);
            return Ok("Finished without cancellation");
        }
    }
}
