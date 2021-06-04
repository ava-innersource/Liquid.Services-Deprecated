Liquid Application Framework - Services
=============================================

This repository is part of the [Liquid Application Framework](https://github.com/Avanade/Liquid-Application-Framework), a modern Dotnet Core Application Framework for building cloud native microservices.

The main repository contains the examples and documentation on how to use Liquid.

Liquid.Services
----------------------
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Avanade_Liquid.Services&metric=alert_status)](https://sonarcloud.io/dashboard?id=Avanade_Liquid.Services)

This package contains the service client subsystem of Liquid, for writing client to synchronous services. It implements common patterns for resiliency such as Retry and Circuit Breaker along with several backing implementations.


Available implementations|Badges||
|:--|--|---|
|[Liquid.Services.Grpc](https://github.com/Avanade/Liquid.Services/tree/main/src/Liquid.Services.Grpc)|[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Avanade_Liquid.Services.Grpc&metric=alert_status)](https://sonarcloud.io/dashboard?id=Avanade_Liquid.Services.Grpc)| Service implementation with Grpc protocol.|
|[Liquid.Services.Http](https://github.com/Avanade/Liquid.Services/tree/main/src/Liquid.Services.Http)|[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Avanade_Liquid.Services.Http&metric=alert_status)](https://sonarcloud.io/dashboard?id=Avanade_Liquid.Services.Http)| Service implementation with Http (Rest) protocol and GraphQL.|

Usage
-----

This is a the simplest form on how to use the service client:

- Declare your request and response classes.
- Declare your service interface, inherting from one of the implemetations provided.
- Implement your service class, inherting from one of the implemetations provided.
- Configure it.
- Inject it were you want to use it.
- Invoke the methods!

```csharp

// using statemens ommited for brevity

namespace Liquid.Example.ServiceClient
{
    public class HelloRequest 
    {

        [JsonProperty("name")]
        public string Name {get; set;}
       
    }

    public class HelloResponse
    {

        [JsonProperty("message")]
        public string Message {get; set;}
       
    }

    public interface IHelloService : ILightHttpService
    {
        Task<HelloResponse> HelloAsync(HelloRequest request);
    }

    // Ideally the Interface and Implementation should be in different projects, to avoid coupling
    public class HelloService : LightHttpService, IHelloService
    {
        public HelloService(IHttpClientFactory httpClientFactory, 
                           ILoggerFactory loggerFactory, 
                           ILightContextFactory contextFactory, 
                           ILightTelemetryFactory telemetryFactory, 
                           ILightConfiguration<List<LightServiceSetting>> servicesSettings, 
                           IMapper mapperService) : base(httpClientFactory, loggerFactory, contextFactory, telemetryFactory, servicesSettings, mapperService)
        {
        }

        Task<HelloResponse> HelloAsync(HelloRequest request)
        {
            var httpResponse = await PostAsync<HelloRequest, HelloResponse>("/hello", request);

            if (httpResponse.HttpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.GetContentObjectAsync();
            }

            // process the status code and throw exceptions if needed.
            return null;
        }
    }

}
```

The configuration can be made in the appsettings.json, such as:

```json
{
    "services": [
    {
      "id": "HelloService",
      "address": "https://your-base-url-here/",
      "timeout": 5,
      "resilience": {
        "circuitBreaker": {
          "enabled": false,
          "failureThreshold": 0.9,
          "samplingDuration": 10,
          "minimumThroughput": 3,
          "durationOfBreak": 10
        },
        "retry": {
          "enabled": false,
          "attempts": 3,
          "waitDuration": 200
        }
      }
    }
  ],

}
```
To use it, in your Startup add:
```C#
services.AddDefaultTelemetry();
services.AddDefaultContext();
services.AddConfigurations(GetType().Assembly);
builder.Services.AddHttpServices(typeof(HelloService).Assembly);
```
