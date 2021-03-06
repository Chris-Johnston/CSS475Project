﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetGame.Models
{
    /// <summary>
    ///     Represents a User's login token.
    /// </summary>
    [JsonObject]
    public class UserToken
    {
        /// <summary>
        ///     Unique identifier for user tokens.
        /// </summary>
        [JsonProperty]
        public ulong UserTokenId { get; set; }

        /// <summary>
        ///     Gets or sets the User (by Id) that this
        ///     token is for.
        /// </summary>
        [JsonProperty]
        public ulong UserId { get; set; }

        /// <summary>
        ///     Gets or sets the Token.
        /// </summary>
        /// <remarks>
        ///     Tokens will be cryptographically random base-64 encoded strings. 
        /// </remarks>
        [JsonProperty]
        public string Token { get; set; }

        /// <summary>
        ///     Gets or sets when this UserToken was last used.
        ///     Updated when the Token is used correctly.
        ///     A UserToken that has not been used in more than three
        ///     days is no longer valid.
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastUsed { get; set; }

        /// <summary>
        ///     Gets when this token was created.
        ///     This should only be set once on Token creation.
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Created { get; set; }
    }
}
