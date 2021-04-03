﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using AutoMapper;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Liquid.Services.Attributes;
using Liquid.Services.Configuration;
using Microsoft.Extensions.Logging;

namespace Liquid.Services.Http.Tests.Services
{
    /// <summary>
    /// Custom Service for tests.
    /// </summary>
    /// <seealso cref="Liquid.Services.Http.LightHttpService" />
    /// <seealso cref="Liquid.Services.Http.Tests.Services.ICustomService" />
    [ExcludeFromCodeCoverage]
    [ServiceId("CustomService")]
    public class CustomService :  LightHttpService, ICustomService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="telemetryFactory">The telemetry factory.</param>
        /// <param name="servicesSettings">The services settings.</param>
        /// <param name="mapperService">The mapper service.</param>
        public CustomService(IHttpClientFactory httpClientFactory, 
                             ILoggerFactory loggerFactory, 
                             ILightContextFactory contextFactory, 
                             ILightTelemetryFactory telemetryFactory, 
                             ILightConfiguration<List<LightServiceSetting>> servicesSettings, 
                             IMapper mapperService) : base(httpClientFactory, loggerFactory, contextFactory, telemetryFactory, servicesSettings, mapperService)
        {
        }
    }

    /// <summary>
    /// Custom Service interface for tests.
    /// </summary>
    /// <seealso cref="Liquid.Services.Http.ILightHttpService" />
    public interface ICustomService : ILightHttpService
    {
    }
}