﻿using System.IO;
using System.Text;
using System;
using Microsoft.AspNetCore.Rewrite;
using System.Text.Json;
using ArrayToExcel;
using System.Text.Json.Serialization.Metadata;
using System.Runtime.CompilerServices;

namespace WebApiExportToFile;
public class MiddlewareExportToFile : IMiddleware
{
    private static List<Type> types = new(); 
    static readonly string[] Extensions = new string[1] { ".xlsx" };
    static string key = "Export";
    public static void AddReturnTypes(params Type[]? typesReturnedByActions)
    {
        if (typesReturnedByActions?.Length > 0)
        {
            foreach (var type in typesReturnedByActions)
            {
                AddReturnType(type);
            }
        }
    }
    public static void AddReturnType(Type type)
    {
        types.Add(type);
    }
    public static void RewriteExtNeeded(RewriteContext context)
    {
        var request = context.HttpContext.Request;
        if (!(context.HttpContext.Items.ContainsKey(key) && context.HttpContext.Items[key]?.ToString() == "1"))
        {
            return;
        }
        var ext = Path.GetExtension(request.Path.Value);
        if (string.IsNullOrWhiteSpace(ext)) return;
        request.Path = request.Path.Value!.Substring(0, request.Path.Value.Length - ext.Length);

    }
    public bool ShouldIntercept(HttpContext context)
    {
        string path = context.Request.Path;
        var ext = Path.GetExtension(path);
        if (string.IsNullOrWhiteSpace(ext)) return false;
        if (!Extensions.Contains(ext, StringComparer.OrdinalIgnoreCase)) return false;
        return true;
    }
    //https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft
    static void AddMissingMemberHandling(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind == JsonTypeInfoKind.Object &&
            typeInfo.Properties.All(prop => !prop.IsExtensionData) &&
            typeInfo.OnDeserialized is null)
        {
            // Dynamically attach dictionaries to deserialized objects.
            var cwt = new ConditionalWeakTable<object, Dictionary<string, object>>();

            JsonPropertyInfo propertyInfo =
                typeInfo.CreateJsonPropertyInfo(typeof(Dictionary<string, object>), "__extensionDataAttribute");
            propertyInfo.Get = obj => cwt.TryGetValue(obj, out Dictionary<string, object>? value) ? value : null;
            propertyInfo.Set = (obj, value) => cwt.Add(obj, (Dictionary<string, object>)value!);
            propertyInfo.IsExtensionData = true;
            typeInfo.Properties.Add(propertyInfo);
            typeInfo.OnDeserialized = obj =>
            {
                if (cwt.TryGetValue(obj, out Dictionary<string, object>? dict))
                {
                    cwt.Remove(obj);
                    throw new JsonException($"JSON properties {String.Join(", ", dict.Keys)} " +
                        $"could not bind to any members of type {typeInfo.Type}");
                }
            };
        }
    }
    public object[]? StrongDeserialize(string responseContent)
    {
        if(types.Count() == 0) throw new Exception("please add some types");
        foreach( var type in types)
        {
            try
            {
                var data = JsonSerializer.Deserialize(responseContent, type, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    TypeInfoResolver = new DefaultJsonTypeInfoResolver
                    {
                        Modifiers = { AddMissingMemberHandling }
                    }
                }) as object[];
                return data;
            }
            catch(JsonException)
            {
                //do nothing 
                
            }
            
        }
        throw new Exception("no type can deserialize " +responseContent);

    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!ShouldIntercept(context))
        {
            await next(context);
            return;
        }
        var ext=Path.GetExtension(context.Request.Path.Value);
        var nameFile = context.Request.Path.Value?.Replace("/", "_");
        context.Items["Export"] = "1";
        var originalResponseBody = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;
        context.Response.Headers.Add("Content-Disposition", $"attachment; filename={nameFile}");
        await next(context);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseContent = await new StreamReader(memoryStream).ReadToEndAsync();
        context.Response.Body = originalResponseBody;

        var data = StrongDeserialize(responseContent);
        ArgumentNullException.ThrowIfNull(data);
        using var excelStream = data.ToExcelStream();
        await excelStream.CopyToAsync(context.Response.Body);

        // No need to call the next middleware since the generated content has been sent
        return;
    }
    
}    

