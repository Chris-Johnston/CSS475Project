﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetGame.Core;
using PetGame.Models;
using PetGame.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetGame
{
    //TODO: This controller class is missing all Authorization logic, so that needs to be added

    [Route("api/[controller]")]
    public class PetController : Controller
    {
        private readonly SqlManager sqlManager;
        private readonly PetService petService;
        private readonly LoginService loginService;
        private readonly ActivityService activityService;

        public PetController(SqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
            petService = new PetService(this.sqlManager);
            loginService = new LoginService(this.sqlManager);
            activityService = new ActivityService(this.sqlManager);
        }

        // GET api/pet to return all is invalid, because that would
        // result in a lot of data being returned

        // GET api/<controller>/5
        /// <summary>
        ///     Gets a Pet by Id.
        /// </summary>
        /// <param name="id">The ID of the Pet to get from the database.</param>
        /// <returns> A pet of the given ID, or null if unspecified. </returns>
        [HttpGet("{id}"), AllowAnonymous]
        public IActionResult Get(ulong id)
        {
            var pet = petService.GetPetById(id);
            if (pet == null)
                return NotFound();
            return Json(pet);
        }

        /// /api/Pet/[PetId]/status (No Request Body)
        /// <summary>
        /// Returns the status of a single pet by its ID
        /// </summary>
        /// <param name="Request">
        /// JSON request body containing the Pet's ID
        /// </param>
        /// <returns>
        /// JSON of PetStatus object
        /// </returns>
        [HttpGet("{id}/status"), AllowAnonymous]
        public IActionResult GetPetStatusById(ulong id)
        {
            //PetStatus object to be serialized and returned
            PetStatus ret = petService.GetPetStatusById(id);
            
            //if not null, the call to GetPetStatus was successful
            if (ret != null)
            {
                return Json(ret);
            }
            //else return 404
            else
            {
                return NotFound();
            }
        }

        // in postman test with
        // where UserId matches the currently signed-in user
        /** POST /api/Pet
         * {
          "PetId": 123,
          "Name": "So fluffy boi",
          "Birthday": "2012-04-23T18:25:43.511Z",
          "Strength": 5,
          "Endurance": 55,
          "IsDead": true,
          "UserId": 35
            }
            */

        /// <summary>
        ///     Inserts a new Pet into the database.
        ///     This action requires authentication.
        /// </summary>
        /// <param name="value"> The pet to add to the database. </param>
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] Pet value)
        {
            if (value == null)
                return BadRequest("The supplied Pet cannot be null.");
            // check user
            var user = loginService.GetUserFromContext(HttpContext.User);
            if (user?.UserId == value.UserId)
            {
                try
                {
                    return Json(petService.InsertPet(value));
                }
                catch (ArgumentException)
                {
                    // likely invalid args
                    return BadRequest();
                }
                catch (SqlException)
                {
                    // could be constraint that yelled at us
                    return BadRequest();
                }
            }
            // unauthorized
            return Unauthorized();
        }

        /// <summary>
        ///     Updates a pet in the database.
        ///     Requires authorization.
        /// </summary>
        /// <param name="id">
        ///     The ID of the pet to update.
        /// </param>
        /// <param name="value">
        ///     The values of the pet to update.
        /// </param>
        // PUT api/<controller>
        [HttpPut("{id}")]
        public IActionResult Put(ulong id, [FromBody]Pet value)
        {
            if (value == null)
                return BadRequest("The supplied Pet cannot be null.");

            // validate that the pet exists and is owned by the current user
            var toUpdate = petService.GetPetById(id);
            if (toUpdate == null)
                return NotFound();

            var user = loginService.GetUserFromContext(HttpContext.User);
            if (user == null) return Unauthorized();
            else if (user.UserId != toUpdate.UserId)
            {
                return Unauthorized();
            }
            // don't necessarily care if the PetId inside value does not match
            // the id passed separately, since only the Id is going to be used
            var pet = petService.UpdatePet(id, value);
            if (pet == null) return Unauthorized();
            return Json(pet);
        }

        /// <summary>
        ///     Deletes a pet from the database.
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(ulong id)
        {
            var user = loginService.GetUserFromContext(HttpContext.User);

            if (user == null)
                return Unauthorized();

            if (petService.DeletePet(id, user.UserId))
            {
                // deleted ok
                return Ok();
            }
            // didn't delete Ok, either not found or wrong user
            return Unauthorized();
        }

        // Activity
        // GET /api/Pet/petId/Activity
        // POST /api/Pet/petId/ActivityOptions
        // {
        //"Limit": 10,
        //"After": "2012-04-23T18:25:43Z",
        //"Type": 'd'
        //   }
        [HttpGet("{petId}/Activity")]
        [HttpPost("{petId}/ActivityOptions")] // this must not be under /Activity, because that doesn't follow rest convention for POST
        public IActionResult GetRecentActivity(ulong petId, [FromBody] PetActivityRequestOptions options)
        {
            // if GET, or options not specified
            if (options == null)
                options = new PetActivityRequestOptions();

            var user = loginService.GetUserFromContext(HttpContext.User);
            if (user == null) return Unauthorized();

            var pet = petService.GetPetById(petId);
            if (pet == null) return NotFound();

            if (user.UserId != pet.UserId) return Unauthorized();

            // get all of the activities using the request options
            var results = activityService.GetActivities(petId, options.Limit, options.After, options.FixedType);
            if (results == null)
                return BadRequest();
            return Ok(results);
        }
        
        // POST /api/Pet/{petid}/Activity
        // {
//    "activityId": 0,
//    "petId": 0,
//    "timestamp": "2012-04-23T18:25:43Z",
//    "type": 116
//}
    // creates a new activity
    [HttpPost("{petId}/Activity")]
        public IActionResult PostNewActivity(ulong petid, Activity activity)
        {
            // ensure validity of params
            if (activity == null)
                return BadRequest();

            var user = loginService.GetUserFromContext(HttpContext.User);
            if (user == null) return Unauthorized();

            var pet = petService.GetPetById(petid);
            if (pet == null) return NotFound();

            if (user.UserId != pet.UserId) return Unauthorized();

            // enforce the pet id
            activity.PetId = petid;
            var result = activityService.InsertActivity(activity);
            if (result == null)
                return BadRequest();
            activityService.UpdatePetFromActivity(activity.Type, petid, petService);
            return Ok(result);
        }
        // TODO, need to require authorization, check that users may only modify their own pets

        // POST /api/Pet/{petId}/Activity/{type}
        // no request body required
        // creates a new activity of the given type, using the current time
        [HttpPost("{petId}/Activity/{type}")]
        public IActionResult PostNewActivity(ulong petid, char type)
        {
            ActivityType t = ActivityType.Default;
            try
            {
                t = (ActivityType)type;
            }
            catch (Exception)
            {
                // invalid type
                return BadRequest();
            }

            var user = loginService.GetUserFromContext(HttpContext.User);
            if (user == null) return Unauthorized();

            var pet = petService.GetPetById(petid);
            if (pet == null) return NotFound();

            if (user.UserId != pet.UserId) return Unauthorized();

            // double check the pet status, ensure that they can actually perform actions
            var status = petService.GetPetStatusById(petid);
            if (status.ServerTime < status.TimeOfNextAction)
            {
                // server time is before we can actually do the next action
                return BadRequest();
            }
            var result = activityService.MakeActivityForPet(petid, t);
            if (result == null)
                return BadRequest();

            activityService.UpdatePetFromActivity(t, petid, petService);
            return Ok(result);
        }

        // creates a new score
        [HttpPost("{petid}/race/{score}")]
        public IActionResult PostNewScore(ulong petid, int score)
        {
            var u = loginService.GetUserFromContext(HttpContext.User);
            if (u == null) return Unauthorized();

            // get the pet
            var pet = petService.GetPetById(petid);
            if (pet == null)
                return NotFound();
            // check ownership
            if (pet.UserId != u.UserId)
                return Unauthorized();

            if (score <= 0)
                return BadRequest();

            // TODO: Consider limiting this based on when the last score was inserted and when the last activity for this pet appears
            // could use the activity id as a way to do this.

            RaceService race = new RaceService(this.sqlManager);
            var r = race.InsertRace(new Race()
            {
                PetId = petid,
                RaceId = 0,
                Score = score,
                Timestamp = DateTime.UtcNow
            });

            // also post a new activity
            activityService.MakeActivityForPet(petid, ActivityType.Race);

            // if the score was podium
            var rank = race.GetRaceRank(r.RaceId);
            if (rank != -1 && rank < 4)
            {
                activityService.MakeActivityForPet(petid, ActivityType.RaceHighScore);
                activityService.UpdatePetFromActivity(ActivityType.RaceHighScore, petid, petService);

                // new high score was posted
                var n = new NotificationService();
                if (u.PhoneNumber != null)
                {
                    n.SendMessage(u.PhoneNumber, $"Great job {u.Username}, you just placed #{rank} on the leaderboard!");
                }
                n.SendDiscordNotifyHighScore(rank, score, pet, u);
            }
            else
            {
                activityService.UpdatePetFromActivity(ActivityType.Race, petid, petService);
            }

            return Json(r);
        }
    }
}
