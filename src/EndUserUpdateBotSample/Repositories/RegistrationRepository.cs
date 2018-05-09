using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EndUserUpdateBotSample.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace EndUserUpdateBotSample.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly DocumentClient _client;
        private readonly string _dbName;
        private readonly string _collectionName;

        public RegistrationRepository(DocumentClient client, string dbName, string collectionName)
        {
            _client = client;
            _dbName = dbName;
            _collectionName = collectionName;
        }

        public Task AddAsync(Registration registration)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Registration registration)
        {
            throw new NotImplementedException();
        }

        public async Task InitStore(string dbName, string collectionName)
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = dbName });
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(dbName), new DocumentCollection { Id = collectionName });
        }

        public Task<IList<Registration>> GetAll()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Registration> registrationQuery = _client.CreateDocumentQuery<Registration>(
                UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName), queryOptions);

            return Task.FromResult<IList<Registration>>(registrationQuery.ToList());
        }

        public Task<Registration> GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}