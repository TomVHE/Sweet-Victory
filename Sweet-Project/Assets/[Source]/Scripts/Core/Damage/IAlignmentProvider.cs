using System;
using Core.Utilities;

namespace Core.Damage
{
	/// <summary>
	/// An interface for objects which can provide a team/alignment for damage purposes
	/// </summary>
	public interface IAlignmentProvider : ISerializableInterface
	{
		/// <summary>
		/// Gets whether this alignment can harm another
		/// </summary>
		bool CanHarm(IAlignmentProvider other);
	}

	/// <summary>
	/// Concrete serializable version of interface
	/// </summary>
	[Serializable]
	public class SerializableIAlignmentProvider : SerializableInterface<IAlignmentProvider>
	{
	}
}