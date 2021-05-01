using System;
using System.Threading.Tasks;

namespace MyBank.API.Services.Interface
{
    public interface IAPIService
    {


        Task<T> GetAsync<T>(UriBuilder builder);

    }

}

