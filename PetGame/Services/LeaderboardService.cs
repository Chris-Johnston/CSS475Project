﻿using PetGame.Core;
using PetGame.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace PetGame
{
    public class LeaderboardService
    {
        private readonly SqlManager sqlManager;

        public LeaderboardService(SqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        public IEnumerable<LeaderboardEntry> GetLeaderboardEntries(int NumResults = 10)
        {
            //create list to hold the information
            //names of pets on leaderboard
            List<LeaderboardEntry> ScoreList = new List<LeaderboardEntry>();
            //Query db for races
            using (var conn = sqlManager.EstablishDataConnection)
            {
                //create and set the SQL query
                var cmd = conn.CreateCommand();

                //to be optimized later, if time allows
                cmd.CommandText = @"SELECT TOP (@Count) Race.Score, Race.Timestamp, Pet.[Name] AS 'PetName', Pet.PetId, [User].Username AS 'OwnerName', [User].UserId
                                    FROM Pet, Race, [User]
                                    WHERE Race.PetId = Pet.PetId AND [User].UserId = Pet.UserId ORDER BY Score DESC;";

                cmd.Parameters.AddWithValue("@Count", NumResults);

                //store the pet names, scores, and owner names in the lists
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //add each pet name/score/owner name to a new index in the list
                        ScoreList.Add(new LeaderboardEntry()
                        {
                            Score = reader.GetInt32(0),
                            Timestamp = reader.GetDateTime(1),
                            PetName = reader.GetString(2),
                            PetId = (ulong) reader.GetInt64(3),
                            OwnerName = reader.GetString(4),
                            OwnerId = (ulong) reader.GetInt64(5)
                        });
                    }
                }
            }
            //return races
            return ScoreList;
        }

        public LeaderboardEntry GetLeaderboardEntryByRaceId(ulong RaceId)
        {
            LeaderboardEntry ret = null;

            using (var conn = sqlManager.EstablishDataConnection)
            {
                var cmd = conn.CreateCommand();

                cmd.CommandText = @"SELECT Race.Score, Race.Timestamp, Pet.[Name] AS 'PetName', Pet.PetId, [User].Username AS 'OwnerName', [User].UserId
                                    FROM Pet, Race, [User]
                                    WHERE Race.PetId = Pet.PetId AND [User].UserId = Pet.UserId AND Race.RaceId = @RaceID;";

                cmd.Parameters.Add("@RaceID", SqlDbType.BigInt).Value = Convert.ToInt64(RaceId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret = new LeaderboardEntry()
                        {
                            Score = reader.GetInt32(0),
                            Timestamp = reader.GetDateTime(1),
                            PetName = reader.GetString(2),
                            PetId = (ulong)reader.GetInt64(3),
                            OwnerName = reader.GetString(4),
                            OwnerId = (ulong)reader.GetInt64(5)
                        };
                    }
                }
            }
            return ret;
        }

    }
}
