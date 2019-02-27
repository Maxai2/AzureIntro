using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using VideoConverter.Api.Models;
using VideoConverter.Api.Utils;

namespace VideoConverter.Api.Data {
    public class CosmosDbAsyncRepository<TEntity> 
        : IAsyncRepository<TEntity> where TEntity : ModelBase, new () {
        private readonly CosmosDbOptions _options;
        private readonly DocumentClient _client;
        private readonly Uri _collectionUri;

        public CosmosDbAsyncRepository (CosmosDbOptions options) {
            _options = options;
            _client = new DocumentClient (new Uri (_options.Endpoint), _options.Key);
            _collectionUri = UriFactory.CreateDocumentCollectionUri (_options.DatabaseId, _options.CollectionId);
        }

        public async Task Create (TEntity entity) {
            await _client.CreateDocumentAsync (_collectionUri, entity);
        }

        public async Task<TEntity> Find (string id) {
            var query = _client.CreateDocumentQuery<TEntity> (_collectionUri);
            return await query.ToAsyncEnumerable ().FirstOrDefault (item => item.Id == id);
        }

        public async Task<IEnumerable<TEntity>> Get () {
            var query = _client.CreateDocumentQuery<TEntity> (_collectionUri, new FeedOptions {
                MaxItemCount = -1,
                EnableCrossPartitionQuery = true
            }).AsDocumentQuery ();
            var items = new List<TEntity> ();
            while (query.HasMoreResults) {
                items.AddRange (await query.ExecuteNextAsync<TEntity> ());
            }
            return items;
        }

        public async Task<IEnumerable<TEntity>> Get (Expression<Func<TEntity, bool>> predicate) {
            var query = _client.CreateDocumentQuery<TEntity> (_collectionUri, new FeedOptions {
                MaxItemCount = -1,
                EnableCrossPartitionQuery = true
            }).Where (predicate).AsDocumentQuery ();
            var items = new List<TEntity> ();
            while (query.HasMoreResults) {
                items.AddRange (await query.ExecuteNextAsync<TEntity> ());
            }
            return items;
        }

        public async Task Remove (TEntity entity) {
            var documentUri = UriFactory.CreateDocumentUri (_options.DatabaseId, _options.CollectionId, entity.Id);
            var response = await _client.DeleteDocumentAsync (documentUri);
        }

        public async Task Update (TEntity entity) {
            var documentUri = UriFactory.CreateDocumentUri (_options.DatabaseId, _options.CollectionId, entity.Id);
            await _client.ReplaceDocumentAsync (documentUri, entity);
        }
    }
}