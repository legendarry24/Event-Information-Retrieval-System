using System;
using System.Collections.Generic;

namespace EventCatalog.Domain.Contracts
{
	public interface IRepository<T> where T : IEntity
	{
		IEnumerable<T> GetAll();

		T GetById(Guid id);

		void Add(T entity);

		void Remove(Guid Id);
	}
}