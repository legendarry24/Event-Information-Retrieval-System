using System.ComponentModel;

namespace EventCatalog.Domain.Contracts
{
	public enum Currency
	{
		[Description("Ukrainian hryvnia")] UAH = 980,
		[Description("Russian ruble")] RUB = 643,
		[Description("United States dollar")] USD = 840,
		[Description("Euro")] EUR = 978
	}
}