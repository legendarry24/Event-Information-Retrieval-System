using System;

namespace EventCatalog.Domain.Models.EventAggregate
{
	public class Address : IEquatable<Address>
	{
		public Address(string city, string street, string venue)
		{
			City = city;
			Street = street;
			Venue = venue;
		}

		public Address() { }

		public string City { get; private set; }

		public string Street { get; private set; }

		public string Venue { get; private set; }

		public bool Equals(Address other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return City == other.City && 
			       Street == other.Street && 
			       Venue == other.Venue;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Address);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(City, Street, Venue);
		}
	}
}