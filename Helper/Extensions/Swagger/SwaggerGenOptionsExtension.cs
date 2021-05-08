using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helper.Extensions.Swagger
{
    public static class SwaggerGenOptionsExtension
    {
        /// <summary>
        /// Swagger添加xml文档注释
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileNames"></param>
        public static void AddXmlComments(this SwaggerGenOptions options, params string[] fileNames)
        {
            //NuGet安装：Swashbuckle.AspNetCore.SwaggerGen

            List<string> xmlFileNames = new List<string>
            {
                "DTO.xml",
            };
            if (fileNames != null && fileNames.Length > 0)
            {
                xmlFileNames.AddRange(fileNames);
            }

            foreach (var xmlFileName in xmlFileNames)
            {
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);
            }
        }
    }
}
