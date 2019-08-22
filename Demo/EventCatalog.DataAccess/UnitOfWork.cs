using EventCatalog.DataAccess.Repositories;
using EventCatalog.Domain.Contracts;

namespace EventCatalog.DataAccess
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly EventCatalogContext _context;

		public UnitOfWork(EventCatalogContext context)
		{
			_context = context;
			EventRepository = new EventRepository(_context);
		}

		public IEventRepository EventRepository { get; }

		public void Save()
		{
			_context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}