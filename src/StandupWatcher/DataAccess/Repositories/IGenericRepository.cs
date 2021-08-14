using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using StandupWatcher.DataAccess.Models;


namespace StandupWatcher.DataAccess.Repositories
{
	public interface IGenericRepository<T> where T : Entity
	{
		IEnumerable<T> Get(
			Expression<Func<T, bool>>                 filter  = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
		);

		void Add(T entity);

		void Delete(T entity);

		void Update(T entity);

		void Save();
	}
}