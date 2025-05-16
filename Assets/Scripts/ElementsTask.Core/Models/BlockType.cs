using System;
using UnityEngine;

namespace ElementsTask.Core.Models
{
    [Serializable]
    public class BlockType : IEquatable<BlockType>
    {
        [field:SerializeField]
        public string Id { get; private set; }

        public BlockType(string id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BlockType);
        }

        public bool Equals(BlockType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Id, other.Id, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return (Id != null ? StringComparer.Ordinal.GetHashCode(Id) : 0);
        }

        public static bool operator ==(BlockType left, BlockType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BlockType left, BlockType right)
        {
            return !Equals(left, right);
        }
    }
}