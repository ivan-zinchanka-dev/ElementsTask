using System;
using System.Collections.Generic;
using ElementsTask.Data.BlockFieldCore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ElementsTask.Data.PlayerProgression.Converters
{
    public class BlockFieldStateConverter : JsonConverter<Dictionary<Vector2Int, Block>>
    {
        public override void WriteJson(JsonWriter writer, Dictionary<Vector2Int, Block> value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (var pair in value)
            {
                string key = $"{pair.Key.x},{pair.Key.y}";
                writer.WritePropertyName(key);
                serializer.Serialize(writer, pair.Value);
            }
            writer.WriteEndObject();
        }

        public override Dictionary<Vector2Int, Block> ReadJson(JsonReader reader, Type objectType, 
            Dictionary<Vector2Int, Block> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var blockFieldState = new Dictionary<Vector2Int, Block>();

            JObject obj = JObject.Load(reader);
            foreach (JProperty property in obj.Properties())
            {
                string[] keyParts = property.Name.Split(',');
                if (keyParts.Length == 2 &&
                    int.TryParse(keyParts[0], out int x) &&
                    int.TryParse(keyParts[1], out int y))
                {
                    var key = new Vector2Int(x, y);
                    Block value = property.Value.ToObject<Block>(serializer);
                    blockFieldState.Add(key, value);
                }
                else
                {
                    throw new JsonSerializationException($"Invalid key format for Vector2Int: {property.Name}");
                }
            }

            return blockFieldState;
        }
    }
}