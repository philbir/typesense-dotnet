using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Typesense.Setup;

public static class TypesenseExtension
{
    /// <param name="serviceCollection">
    /// The collection of services to be configured for the Typesense client.
    /// </param>
    /// <param name="config">
    /// The configuration for the Typesense client.
    /// </param>
    /// <param name="enableHttpCompression">
    /// If set to true, HTTP compression is enabled, lowering response times and reducing traffic for externally hosted Typesense, like Typesense Cloud
    /// Set to false by default to mimic the old behavior, and not add compression processing overhead on locally hosted Typesense
    /// </param>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddTypesenseClient(
        this IServiceCollection serviceCollection, 
        Action<JsonSerializerOptions>? configureJsonOptions)
    {
        serviceCollection.AddHttpClient(TypesenseClient.HttpClientName, (provider, client) =>
        {
            TypesenseOptions options = provider.GetRequiredService<IOptions<TypesenseOptions>>().Value;
            client.BaseAddress = new Uri(options.Url);
            client.DefaultRequestHeaders.Add("x-typesense-api-key", options.ApiKey);
            client.DefaultRequestVersion = HttpVersion.Version30;
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });

        serviceCollection.AddSingleton<ITypesenseClient, TypesenseClient>();
        return serviceCollection;
    }
}