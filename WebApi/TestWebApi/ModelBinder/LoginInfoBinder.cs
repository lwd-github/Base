using DTO.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi.ModelBinder
{
    public class LoginInfoBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.Result = ModelBindingResult.Success(new LoginInfo { Id = 1, Name = "Test"});
            return Task.CompletedTask;
        }
    }
}
