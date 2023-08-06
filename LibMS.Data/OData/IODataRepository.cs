using System;
using LibMS.Data.Entities;

namespace LibMS.Data
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

