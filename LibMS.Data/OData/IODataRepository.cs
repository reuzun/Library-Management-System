using System;
using LibMs.Data.Entities;

namespace LibMs.Data
{
	public interface IODataRepository<T> where T : IEntity, new()
    {
		/// <summary>
		/// Provides all data as IQueryable for OData.
		/// </summary>
		/// <returns></returns>
		IQueryable<T> ReadAll();
	}
}

