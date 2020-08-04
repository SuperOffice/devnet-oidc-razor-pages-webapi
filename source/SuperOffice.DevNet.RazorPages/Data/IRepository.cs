using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Data
{
    public interface IRepository
    {
        Task AddAsync<T>(T entity, string requestUri);
        Task<HttpStatusCode> DeleteAsync(string requestUri);
        Task EditAsync<T>(T entity, string requestUri);
        Task<T> GetAsync<T>(string path);
    }
}
