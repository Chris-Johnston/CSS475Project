﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace PetGame.Models
{
    /// <summary>
    ///     Represents a Pet's Status
    /// </summary>
    [JsonObject]
    public class PetStatus
    {
        /// <summary>
        /// Represents the Pet whose status is being requested
        /// </summary>
        [JsonProperty]
        public Pet Pet { get; set; }

        /// <summary>
        /// Represents the Pet's Happiness level as a ratio
        /// </summary>
        [JsonProperty]
        public double Happiness { get; set; }

        /// <summary>
        /// Represent's the Pet's Hunger level as a ratio
        /// </summary>
        [JsonProperty]
        public double Hunger { get; set; }
        
        /// <summary>
        /// Represents the time when the pet can be fed again
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime TimeOfNextFeeding { get; set; }

        /// <summary>
        /// Represents the time when the pet can be trained again
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime TimeOfNextTraining { get; set; }

        /// <summary>
        /// Represents the time when the pet can be raced again
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime TimeOfNextRace { get; set; }
    }
}
