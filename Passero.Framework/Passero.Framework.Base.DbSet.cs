using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Passero.Framework.Base
{
    public class DbSet<T> where T : class
    {
        private readonly List<T> _localEntities = new();

        // Metodo per aggiungere un'entità
        public void Add(T entity)
        {
            _localEntities.Add(entity);
        }

        // Metodo per aggiornare un'entità
        public void Update(T entity)
        {
            // Logica per aggiornare l'entità (può essere migliorata con un meccanismo di tracking)
        }

        // Metodo per rimuovere un'entità
        public void Remove(T entity)
        {
            _localEntities.Remove(entity);
        }

        // Metodo per ottenere tutte le entità
        public IEnumerable<T> ToList()
        {
            return _localEntities;
        }

        // Metodo per eseguire query personalizzate
        public IEnumerable<T> Query(Func<T, bool> predicate)
        {
            return _localEntities.Where(predicate);
        }
        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            return Query(predicate);
        }
    }
}
