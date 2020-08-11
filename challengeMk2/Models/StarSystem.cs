﻿using Newtonsoft.Json;

namespace challengeMk2.Models
{
    public class StarSystem
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("bodyCount")]
        public int BodyCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("requirePermit")]
        public bool RequirePermit { get; set; }

        [JsonProperty("permitName")]
        public string PermitName { get; set; }

        [JsonProperty("information")]
        public Information Information { get; set; }

        [JsonProperty("primaryStar")]
        public PrimaryStar PrimaryStar { get; set; }

        [JsonProperty("coords")]
        public Coords Coords { get; set; }


        //ID for dataSelector
        //private int dataSelectorID = 0;
        //public int DataSelectorID
        //{
        //    get => dataSelectorID;
        //    set => dataSelectorID = value;
        //}
        public int DataSelectorID { get; set; }


        //Debug
        public override string ToString()
        {
            //return base.ToString();
            return Name;
        }
    }
}
