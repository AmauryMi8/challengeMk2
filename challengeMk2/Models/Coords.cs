﻿using Newtonsoft.Json;

namespace ChallengeMk2.Models
{
    public class Coords
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }

        [JsonProperty("z")]
        public float Z { get; set; }
    }
}
