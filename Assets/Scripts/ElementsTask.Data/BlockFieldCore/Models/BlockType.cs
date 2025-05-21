using System;
using Newtonsoft.Json;
using UnityEngine;

namespace ElementsTask.Data.BlockFieldCore.Models
{
    [Serializable]
    public class BlockType : IEquatable<BlockType>
    {
        public static readonly BlockType Empty = new BlockType(EmptyId);
        private const string EmptyId = "Empty";
        
        [field:SerializeField]
        [JsonProperty]
        public string Id { get; private set; }

        public BlockType()
        {
            Id = EmptyId;
        }
        
        private BlockType(string id)
        {
            Id = id;
        }

        public static BlockType Parse(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return Empty;
            }

            return new BlockType(id);
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