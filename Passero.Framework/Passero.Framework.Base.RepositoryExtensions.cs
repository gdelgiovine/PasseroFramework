using System;
using System.Collections.Generic;

namespace Passero.Framework.Extensions
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Ottiene un repository dal dizionario per nome
        /// </summary>
        /// <param name="repositories">Il dizionario dei repository</param>
        /// <param name="repositoryName">Il nome del repository</param>
        /// <returns>Il repository come object, o null se non trovato</returns>
        public static object GetRepository(this Dictionary<string, object> repositories, string repositoryName)
        {
            object _repository = repositories.TryGetValue(repositoryName, out var repository) ? repository : null;
            Type repositoryType = _repository?.GetType();
            _repository = Convert.ChangeType(_repository, repositoryType);
            return _repository ?? null;
            //return repositories.TryGetValue(repositoryName, out var _repository) ? _repository : null;
        }

        /// <summary>
        /// Ottiene un repository tipizzato dal dizionario per nome
        /// </summary>
        /// <typeparam name="T">Il tipo del modello del repository</typeparam>
        /// <param name="repositories">Il dizionario dei repository</param>
        /// <param name="repositoryName">Il nome del repository</param>
        /// <returns>Repository tipizzato o null se non trovato o non del tipo corretto</returns>
        public static Repository<T> GetRepository<T>(this Dictionary<string, object> repositories, string repositoryName)
            where T : class
        {
            if (repositories.TryGetValue(repositoryName, out var repository))
            {
                return repository as Repository<T>;
            }
            return null;
        }

        /// <summary>
        /// Verifica se esiste un repository con il nome specificato
        /// </summary>
        /// <param name="repositories">Il dizionario dei repository</param>
        /// <param name="repositoryName">Il nome del repository</param>
        /// <returns>True se il repository esiste, altrimenti false</returns>
        public static bool ContainsRepository(this Dictionary<string, object> repositories, string repositoryName)
        {
            return repositories.ContainsKey(repositoryName);
        }
    }
}