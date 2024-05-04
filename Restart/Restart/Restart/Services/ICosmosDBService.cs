using Restart.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restart.Services
    {

    public interface ICosmosDBService
        {
    

        Task<List<Spesa>> OttieniSpese (string userid);

        Task<List<Risparmio>> OttieniRisparmi (string userid);

        Task Salva<T> (T item, string id);

        Task Elimina (string id);

        // Task<T> GetItemAsync<T> (string coll, string tipo);

        Task<List<Quota>> OttieniQuota (string tipo, string userid);

        Task<List<Categorie>> OttieniCategorie (string tipo, string userid);

        Task<List<Utente>> OttieniUtente (string userid);

        Task<List<Utente>> OttieniUtenteRegistrato (string email);

        }

    }