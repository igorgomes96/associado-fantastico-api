using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace AssociadoFantastico.WebApi.Authentication
{
    public class JwtBearerOptionsConfig
    {
        public static void JwtConfiguration(
            JwtBearerOptions options,
            SigningConfigurations signingConfigurations,
            TokenConfigurations tokenConfigurations)
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            var paramsValidation = options.TokenValidationParameters;
            paramsValidation.IssuerSigningKey = signingConfigurations.Key;
            paramsValidation.ValidAudience = tokenConfigurations.Audience;
            paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

            // Valida a assinatura de um token recebido
            paramsValidation.ValidateIssuerSigningKey = true;

            // Verifica se um token recebido ainda é válido
            paramsValidation.ValidateLifetime = true;

            // Tempo de tolerância para a expiração de um token (utilizado
            // caso haja problemas de sincronismo de horário entre diferentes
            // computadores envolvidos no processo de comunicação)
            paramsValidation.ClockSkew = TimeSpan.Zero;
        }

        internal static void JwtConfiguration(JwtBearerOptions options, object signingConfigurations, object tokenConfigurations)
        {
            throw new NotImplementedException();
        }
    }
}
