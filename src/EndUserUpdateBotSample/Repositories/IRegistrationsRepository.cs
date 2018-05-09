using EndUserUpdateBotSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EndUserUpdateBotSample.Repositories
{
    public interface IRegistrationRepository
    {
        Task AddAsync(Registration registration);

        Task UpdateAsync(Registration registration);

        Task DeleteAsync(string id);

        Task<IList<Registration>> GetAll();

        Task<Registration> GetById(string id);

        Task InitStore(string databaseName, string collectionName);
    }
}