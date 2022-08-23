using System;

namespace EventCatalog.Domain.Contracts
{
	public interface IUnitOfWork : IDisposable
	{
		IEventRepository EventRepository { get; }

		void Save();
	}
}