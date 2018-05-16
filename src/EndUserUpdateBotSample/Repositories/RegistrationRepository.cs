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

        public async Task AddAsync(Registration registration)
        {
            await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName), registration);
        }

        public async Task DeleteAsync(string id)
        {
            var registration = await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_dbName, _collectionName, id));
        }

        public async Task UpdateAsync(Registration registration)
        {
            await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_dbName, _collectionName, registration.Id), registration);
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

        public async Task<Registration> GetById(string id)
        {
            var registration = await _client.ReadDocumentAsync<Registration>(UriFactory.CreateDocumentUri(_dbName, _collectionName, id));
            return registration;
        }

        public Task<IList<Registration>> GetByStatus(string status)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Registration> registrationQuery = _client.CreateDocumentQuery<Registration>(
                UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName), queryOptions)
                .Where(r => r.Status == status);

            return Task.FromResult<IList<Registration>>(registrationQuery.ToList());
        }

        public Task<IList<Registration>> GetByPhoneNumber(string phoneNumber)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Registration> registrationQuery = _client.CreateDocumentQuery<Registration>(
                UriFactory.CreateDocumentCollectionUri(_dbName, _collectionName), queryOptions)
                .Where(r => r.PhoneNumber == phoneNumber);

            var registrations = registrationQuery.ToList();

            return Task.FromResult<IList<Registration>>(registrations); 
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_client != null) _client.Dispose();
            }
        }
    }
}