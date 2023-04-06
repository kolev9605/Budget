﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Budget.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHealthChecks();

            return services;
        }
    }
}