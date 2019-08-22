using System;
using EventCatalog.Domain.Contracts;

namespace EventCatalog.Domain.Models
{
	public class Money : IEquatable<Money>
	{
		public Money(decimal amount, Currency currency)
		{
			Amount = amount;
			Currency = currency;
		}

		public Money() { }

		public decimal Amount { get; private set; }

		public Currency Currency { get; private set; }

		public bool Equals(Money other)
		{
			if (other == null) return false;
			if (ReferenceEquals(other, this)) return true;

			return Amount.Equals(other.Amount) && Currency.Equals(other.Currency);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Money);
		}

		public override int GetHashCode()
		{
			return Amount.GetHashCode() ^ Currency.GetHashCode();
		}
	}
}