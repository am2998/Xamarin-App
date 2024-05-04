using System;
using System.Collections.Generic;

 using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Restart.Models;

namespace Restart.Services
    {

    public class CosmosDBService : ICosmosDBService
        {
        private DocumentClient _client;
        private Uri _collectionLink;

        string CollectionName = null;

        public CosmosDBService (string CName)
            {
            this.CollectionName = CName;
            _client = new DocumentClient (new Uri (Parameters.EndpointUri), Parameters.PrimaryKey);
            _collectionLink = UriFactory.CreateDocumentCollectionUri (Parameters.DatabaseName, CollectionName);
           
            }

      
        public async Task<List<Spesa>> OttieniSpese (string userid)
            {
            var items = new List<Spesa> ( );

            try
                {

                var query = _client.CreateDocumentQuery<Spesa> (_collectionLink,
                        new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where(x => x.Chiave.Equals (userid))
                         .AsDocumentQuery ( );


                while (query.HasMoreResults)
                    {
                    items.AddRange (await query.ExecuteNextAsync<Spesa> ( ));
                    }
                }

            catch (DocumentClientException ex)
                {
                Debug.WriteLine ("Get Items Error: ", ex.Message);
                }


            return items;
            }

        public async Task<List<Risparmio>> OttieniRisparmi (string userid)
            {
            var items = new List<Risparmio> ( );

            try
                {

                var query = _client.CreateDocumentQuery<Risparmio> (_collectionLink,
                        new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where (x => x.Chiave.Equals (userid))
                         .AsDocumentQuery ( );


                while (query.HasMoreResults)
                    {
                    items.AddRange (await query.ExecuteNextAsync<Risparmio> ( ));
                    }
                }

            catch (DocumentClientException ex)
                {
                Debug.WriteLine ("Get Items Error: ", ex.Message);
                }


            return items;
            }
        


    public async Task Salva<T> (T item, string id)
            {
            
      

           

            try
                {
                await _client.ReplaceDocumentAsync (UriFactory.CreateDocumentUri (Parameters.DatabaseName, CollectionName, id), item);

                }

            catch(Exception e)
                {
                await _client.CreateDocumentAsync (_collectionLink, item);
                }
                    
              //  }
           /* catch (DocumentClientException ex)
                {
                Debug.WriteLine ("Save Error: ", ex.Message);
                }

            */
            }

        public async Task Elimina (string id)
            {
            try
                {

                await _client.DeleteDocumentAsync (UriFactory.CreateDocumentUri (Parameters.DatabaseName, CollectionName, id), new RequestOptions { PartitionKey = new PartitionKey (id) });
                }
            catch (DocumentClientException ex)
                {
                Debug.WriteLine ("Delete Error: ", ex.Message);
                }

            


            }

      


        public async Task<List<Quota>> OttieniQuota (string tipo,string userid)
            {
            var items = new List<Quota> ( );

      

                var query = _client.CreateDocumentQuery<Quota> (_collectionLink,
                        new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where(x => x.Tipo.Equals(tipo) && x.Chiave.Equals(userid))
                         .AsDocumentQuery ( );


                while (query.HasMoreResults)
                    {
                    items.AddRange (await query.ExecuteNextAsync<Quota> ( ));
              
                    }
              

            return items;
            }

        public async Task<List<Categorie>> OttieniCategorie (string tipo,string userid)
            {
            var items = new List<Categorie> ( );



            var query = _client.CreateDocumentQuery<Categorie> (_collectionLink,
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where (x => x.Tipo.Equals (tipo) && x.Chiave.Equals (userid))
                     .AsDocumentQuery ( );


            while (query.HasMoreResults)
                {
                items.AddRange (await query.ExecuteNextAsync<Categorie> ( ));

                }


            return items;
            }


        public async Task<List<Utente>> OttieniUtente (string userid)
            {
            var items = new List<Utente> ( );



            var query = _client.CreateDocumentQuery<Utente> (_collectionLink,
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where (x => x.ID.Contains(userid))
                     .AsDocumentQuery ( );


            while (query.HasMoreResults)
                {
                items.AddRange (await query.ExecuteNextAsync<Utente> ( ));

                }


            return items;
            }

        public async Task<List<Utente>> OttieniUtenteRegistrato (string email) {


                var items = new List<Utente> ( );



                var query = _client.CreateDocumentQuery<Utente> (_collectionLink,
                        new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where (x => x.Email.Equals(email))
                         .AsDocumentQuery ( );


                while (query.HasMoreResults)
                    {
                    items.AddRange (await query.ExecuteNextAsync<Utente> ( ));

                    }


                return items;
                }
            }


        }
   