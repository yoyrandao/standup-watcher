using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using StandupWatcher.DataAccess.Models;

namespace StandupWatcher.DataAccess.Repositories
{
	public sealed class GenericRepository<T> : IGenericRepository<T> where T : Entity
	{
		public GenericRepository(DatabaseContext context)
		{
			_context = context;
			_collection = _context.Set<T>();
		}

		#region Implementation of IGenericRepository<T>

		public IEnumerable<T> Get(
			Expression<Func<T, bool>>                 filter  = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
		)
		{
			var collectionQuery = _collection.AsQueryable();

			if (filter is not null)
				collectionQuery = collectionQuery.Where(filter);

			return orderBy is not null ? orderBy(collectionQuery).ToList() : collectionQuery.ToList();
		}

		public void Add(T entity)
		{
			_collection.Add(entity);
		}

		public void Delete(T entity)
		{
			_collection.Remove(entity);
		}

		public void Update(T entity)
		{
			_collection.Update(entity);
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		#endregion

		private readonly DbSet<T> _collection;

		private readonly DatabaseContext _context;
	}
}