using System;

namespace EventCatalog.Domain.Contracts
{
	public interface IEntity
	{
		Guid Id { get; }
	}
}